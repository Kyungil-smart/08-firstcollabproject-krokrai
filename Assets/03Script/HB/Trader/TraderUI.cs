using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TraderUI : MonoBehaviour
{
    [Header("필터 UI")]
    public GameObject filterPanel;                  // 필터 등급 창
    public FilterManager filterManager;
    public TextMeshProUGUI filterButtonText;        // 필터 버튼의 텍스트

    [Header("물고기 선택")]
    public Transform content;                       // 물고기 종류가 들어있는 content 오브젝트 담기
    public Toggle selectAllButton;                  // 전체 선택 버튼

    [Header("판매 버튼")]
    public Button sellButton;                       // 판매 버튼

    [Header("경제 시스템")]
    public TextMeshProUGUI goldText;                // 현재 보유 골드TMP
    public TextMeshProUGUI totalPriceText;          // 선택된 물고기의 합계 금액TMP

    private List<Toggle> fishToggles = new List<Toggle>();  // 물고기 슬롯을 담는 List
    private bool isFilterMode = false;              // 현재 필터창이 열려 있는지
    private bool _isUpdatingAll = false;            // 무한 루프 방지
    private int currentGold = 0;                    // 현재 플레이어의 보유 골드
    private int currentSelectedPrice = 0;           // 선택된 물고기의 합계 금액
    

    private void Awake()
    {
        // 초기 물고기 리스트 세팅
        InitFIshList();

        // Select All 버튼 리스트너 등록
        if (selectAllButton != null)
        {
            selectAllButton.onValueChanged.AddListener(OnSelectAllButtonClicked);
        }

        // 초기 골드 창
        UpdateGoldUI();

        // 초기 판매버튼 상태 초기화
        SellButtonState();
    }

    private void InitFIshList()
    {
        if (content == null) return;

        fishToggles.Clear();

        // Content하위 오브젝트의 Toggle 컴포넌트를 배열로 가져옴
        Toggle[] findToggles = content.GetComponentsInChildren<Toggle>(false);
        // 배열 데이터를 fishToggles 리스트에 담음
        fishToggles.AddRange(findToggles);

        // 개별의 물고기 토글에 리스너 등록
        foreach (Toggle ft in fishToggles)
        {
            ft.onValueChanged.RemoveAllListeners();
            ft.onValueChanged.AddListener((isOn) => OnFishToggleChanged(isOn));
        }
    }

    public void OnSelectAllButtonClicked(bool isOn)
    {
        if (_isUpdatingAll || fishToggles.Count == 0) return;

        _isUpdatingAll = true;

        foreach (Toggle fishToggle in fishToggles)
        {
            if (fishToggle != null)
            {
                // 물고기 이미지의 그래픽 설정에 의해 하이라이트가 켜짐/꺼짐
                fishToggle.isOn = isOn;
            }
        }
        _isUpdatingAll = false;

        Debug.Log($"전체 선택 체크: {isOn} / 하이라이트 효과 적용");

        SellButtonState();
    }

    private void OnFishToggleChanged(bool isOn)
    {
        // 전체 선택 시 하단 로직 스킵
        if(_isUpdatingAll) return;
        
        // 모든 물고기가 선택됐는지
        bool allSelected = true;
        
        // 리스트에 담긴 물고기 확인
        foreach (Toggle ft in fishToggles)
        {
            // 물고기가 있고 선택이 되지 않은 오브젝트가 있다면 전체 선택이 아님
            if (ft != null && !ft.isOn)
            {
                allSelected = false;
                break;
            }
        }

        // 체크 결과에 따라 전체 선택 여부 체크, 다시 로직이 실행되지 않도록 끔
        _isUpdatingAll = true;
        selectAllButton.isOn = allSelected;
        _isUpdatingAll = false;

        SellButtonState();
    }

    // 판매 버튼 활성화 제어
    private void SellButtonState()
    {
        if (sellButton == null) return;

        bool isFishSelected = false;
        int total = 0;
        
        // 리스트에 물고기가 체크되어있는지 확인
        foreach (Toggle ft in fishToggles)
        {
            if (ft != null && ft.isOn)
            {
                isFishSelected = true;
                
                // 선택된 물고기 FishRarity스크립트에서 가격을 가져와 더함
                FishData data = ft.GetComponentInParent<FishSlot>().fishData;
                if(data != null)
                {
                    total += data.price;
                }
            }
        }

        // 팔았을 때 합계 UI
        currentSelectedPrice = total;
        totalPriceText.text= $"Total Peice: {total} Gold";

        // 체크된 게 있다면 버튼 활성화, 아니면 비활성화
        sellButton.interactable = isFishSelected;
    }

    public void OnSellButtonClicked()
    {
        // 현재 보유 골드 증가
        currentGold += currentSelectedPrice;
        UpdateGoldUI();

        // 판매된 물고기 역순으로 삭제
        for (int i = fishToggles.Count - 1; i >= 0; i--)
        {
            if(fishToggles[i] != null && fishToggles[i].isOn)
            {
                GameObject fishObj = fishToggles[i].gameObject;
                fishToggles.RemoveAt(i);
                Destroy(fishObj);
            }
        }
        // 리스트가 변해서 UI갱신
        RefreshFishImages();

        // 판매 후 total price 초기화
        currentSelectedPrice = 0;
        totalPriceText.text = "Total Price: 0 Gold";

        // 판매 후 선택된 게 없음으로 Sell버튼 비활성화
        sellButton.interactable = false;

        Debug.Log($"판매 완료 현재 골드: {currentGold}");
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"{currentGold} Gold";
    }

    public void OnFilterButtonClicked()
    {
        isFilterMode = !isFilterMode;

        if (isFilterMode)
        {
            // 필터창 열기
            filterPanel.SetActive(true);
            // 버튼 글자 변경
            filterButtonText.text = "Confirm";
            // 처음 열 때 A만 체크되도록 초기화 호출
            filterManager.ResetFilter();
        }
        else
        {
            // 닫을 때 필터링해서 UI에 띄우기
            ApplyFilter();
            // 필터창이 열려있는 경우
            filterPanel.SetActive(false);
            // 다시 필터 버튼으로 원복
            filterButtonText.text = "Filter";
        }
    }

    private void ApplyFilter()
    {
        // FilterManager 에서 선택된 등급 Enum리스트 받기
        List<EFish_Rarity> selectedRates = filterManager.SelectedRates();

        // Content 하위 오브젝트 체크
        foreach (Transform child in content)
        {
            FishData data = child.GetComponent<FishSlot>().fishData;

            if (data != null)
            {
                // 물고기 등급이 선택된 리스트에 포함되어 있는지
                // 포함됐다면 true(활성화),아니면 false(비활성화)
                bool showFish = selectedRates.Contains(data.fishRarity);
                child.gameObject.SetActive(showFish);
            }
        }

        // 전체 선택 상태가 꼬이지 않게 리스트를 다시 갱신
        RefreshFishImages();
        
    }

    private void RefreshFishImages()
    {
        InitFIshList();

        OnFishToggleChanged(true);
        SellButtonState();
    }

    public void CloseTrader()
    {
        gameObject.SetActive(false);
    }
}
