using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TraderUI : MonoBehaviour
{
    [Header("필터 UI")]
    public GameObject filterPanel;                  // 필터 등급 창
    public FilterManager filterManager;
    public TextMeshProUGUI filterButtonText;        // 필터 버튼의 텍스트

    [Header("물고기 선택")]
    public Transform content;                       // 물고기 종류가 들어있는 content 오브젝트 담기
    public Toggle selectAllButton;                  // 전체 선택 버튼
    public GameObject fishSlotPrefab;               // 물고기 슬롯 프리팹

    [Header("판매, 경제 시스템")]
    public Button sellButton;                       // 판매 버튼

    public TextMeshProUGUI goldText;                // 현재 보유 골드TMP
    public TextMeshProUGUI totalPriceText;          // 선택된 물고기의 합계 금액TMP

    private List<Toggle> fishToggles = new List<Toggle>();  // 물고기 슬롯을 담는 List
    private bool isFilterMode = false;              // 현재 필터창이 열려 있는지
    private bool _isUpdatingAll = false;            // 무한 루프 방지
    private long currentSelectedPrice = 0;          // 선택된 물고기의 합계 금액

    private void Awake()
    {
        // DataTower에서 돈 관련 내용 구독
        if (DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney += HandleMoneyChanged;

            // DataTower 돈 추가기능 필요
            UpdateGoldUI(DataTower.instance.money);
        }

        // Select All 버튼 리스트너 등록
        if (selectAllButton != null)
        {
            selectAllButton.onValueChanged.AddListener(OnSelectAllButtonClicked);
        }

        // 초기 판매버튼 상태 초기화
        SellButtonState();
    }
    
    private void OnEnable()
    {
        RefreshTraderList();
    }
    
    // DataTower의 내용을 UI로 출력
    public void RefreshTraderList()
    {
        if(DataTower.instance == null || content == null) return;

        // 기존 슬롯 제거, UI초기화
        foreach (Transform child in content) Destroy(child.gameObject);
        fishToggles.Clear();
        _isUpdatingAll = true;

        // DataTower.Items에 있는 물고기 만큼 슬롯 생성
        foreach (FishData data in DataTower.instance.Items)
        {
            GameObject go = Instantiate(fishSlotPrefab, content);
            FishSlot slot = go.GetComponent<FishSlot>();

            // 슬롯에 등급, 가격 데이터 부여
            if (slot != null)
            {
                // [수정] 이름에 맞게 SetupTrader 호출
                slot.SetupTrader(data, this);

                Toggle toggle = go.GetComponent<Toggle>();
                
                if(toggle != null)
                {
                    fishToggles.Add(toggle);
                    toggle.onValueChanged.AddListener((isOn) => OnFishToggleChanged(isOn));
                }
            }
        }

        // 슬롯들 갱신
        _isUpdatingAll = false;
        ApplyFilter();
        SellButtonState();
    }

    public void OnSelectAllButtonClicked(bool isOn)
    {
        if (_isUpdatingAll || fishToggles.Count == 0) return;

        _isUpdatingAll = true;

        foreach (Toggle toggle in fishToggles)
        {
            // 활성화된 슬롯만 체크하거나, 전체 체크
            if (toggle != null && toggle.gameObject.activeInHierarchy)
            {
                toggle.isOn = isOn;
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
        int activeCount = 0;
        
        // 리스트에 담긴 물고기 확인
        foreach (Toggle toggle in fishToggles)
        {
            // 활성화된 슬롯만 체크
            if (toggle != null && toggle.gameObject.activeInHierarchy)
            {
                activeCount++;
                if (!toggle.isOn)
                {
                    allSelected = false;
                    break;
                }
            }
        }

        // 체크 결과에 따라 전체 선택 여부 체크, 다시 로직이 실행되지 않도록 끔
        _isUpdatingAll = true;
        if (selectAllButton != null)
        {
            selectAllButton.isOn = (activeCount > 0 && allSelected);
        }
        _isUpdatingAll = false;

        SellButtonState();
    }

    public void SellButtonState()
    {
        if (sellButton == null) return;

        bool isAnySelected = false;
        int total = 0;
        
        // 리스트에 물고기가 체크되어있는지 확인
        foreach (Toggle toggle in fishToggles)
        {
            if (toggle != null && toggle.isOn && toggle.gameObject.activeInHierarchy)
            {
                isAnySelected = true;
                
                FishSlot slot = toggle.GetComponent<FishSlot>();

                if(slot != null && slot.GetFishData() != null)
                {
                    total += slot.GetFishData().price;
                }
            }
        }

        // 팔았을 때 합계 UI
        currentSelectedPrice = total;
        if (totalPriceText != null)
        {
            totalPriceText.text= $"Total Price: {total} Gold";
        }

        // 체크된 게 있다면 버튼 활성화, 아니면 비활성화
        sellButton.interactable = isAnySelected;
    }
    
    public void OnSellButtonClicked()
    {
        if (DataTower.instance == null || currentSelectedPrice <= 0) return;
        
        DataTower.instance.TryMoenyChanged((ulong)currentSelectedPrice);

        // 판매된 물고기 역순으로 삭제
        for (int i = fishToggles.Count - 1; i >= 0; i--)
        {
            if(fishToggles[i] != null && fishToggles[i].isOn)
            {
                FishSlot slot = fishToggles[i].GetComponent<FishSlot>();
                if (slot != null && slot.GetFishData() != null)
                {
                    DataTower.instance.Items.Remove(slot.GetFishData());
                }

                Destroy(fishToggles[i].gameObject);
                fishToggles.RemoveAt(i);
            }
        }

        // 판매 후 total price 초기화
        currentSelectedPrice = 0;
        totalPriceText.text = "Total Price: 0 Gold";
        
        // 판매 후 선택된 게 없음으로 Sell버튼 비활성화
        sellButton.interactable = false;

        if(selectAllButton != null) selectAllButton.isOn = false;
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
            if(filterManager != null) filterManager.ResetFilter();
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
        if (filterManager == null) return;
        // FilterManager 에서 선택된 등급 Enum리스트 받기
        List<EFish_Rarity> selectedRates = filterManager.GetSelectedRates();

        _isUpdatingAll = true;
        // Content 하위 오브젝트 체크
        foreach (Toggle toggle in fishToggles)
        {
            FishSlot slot = toggle.GetComponent<FishSlot>();

            if (slot != null && slot.GetFishData() != null)
            {
                // 물고기 등급이 선택된 리스트에 포함되어 있는지
                // 포함됐다면 true(활성화),아니면 false(비활성화)
                bool showFish = selectedRates.Contains(slot.GetFishData().fishRarity);
                toggle.gameObject.SetActive(showFish);

                if(!showFish) toggle.isOn = false;
            }
        }

        _isUpdatingAll = false;

        if(selectAllButton != null)
        {
            selectAllButton.isOn = false;
            SellButtonState();
        }
    }

    private void UpdateGoldUI(ulong money)
    {
        // "N0"로 세자리 당 ',' 찍어주기 
        if (goldText != null) goldText.text = $"{money.ToString("N0")} Gold";
    }

    private void HandleMoneyChanged(ulong newMoney)
    {
        UpdateGoldUI(newMoney);
    }

    private void OnDestroy()
    {
        if(DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney -= HandleMoneyChanged;
        }        
    }
}