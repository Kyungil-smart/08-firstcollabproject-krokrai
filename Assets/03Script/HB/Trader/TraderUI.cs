using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TraderUI : MonoBehaviour
{
    [Header("스크립트 참조")]
    public TraderListManager listManager;
    public SelectionManager selectionManager;
    public FilterManager filterManager;
    
    [Header("필터 UI")]
    public GameObject filterPanel;                  // 필터 등급 창
    public TextMeshProUGUI filterButtonText;        // 필터 버튼의 텍스트

    [Header("물고기 선택")]
    public Toggle selectAllButton;                  // 전체 선택 버튼

    [Header("판매, 경제 시스템")]
    public Button sellButton;                       // 판매 버튼
    public TextMeshProUGUI goldText;                // 현재 보유 골드TMP
    public TextMeshProUGUI totalPriceText;          // 선택된 물고기의 합계 금액TMP

    private bool isFilterMode = false;              // 현재 필터창이 열려 있는지
    private bool _isUpdatingAll = false;            // 무한 루프 방지

    private bool _isInitialized = false;            // UI창 초기화 여부

    private void Awake()
    {
        // Select All 버튼 리스트너 등록
        if (selectAllButton != null)
        {
            selectAllButton.onValueChanged.RemoveAllListeners();
            selectAllButton.onValueChanged.AddListener(OnSelectAllButtonClicked);

            _isUpdatingAll = true;
            selectAllButton.isOn = false;
            _isUpdatingAll = false;
        }

    }

    private void Start()
    {
        if (DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney -= UpdateGoldUI;
            DataTower.instance.OnChangedMoney += UpdateGoldUI;

            Debug.Log($"Start 시점 잔액 : {DataTower.instance.money}");
            UpdateGoldUI(DataTower.instance.money);
        }
        else
        {
            Debug.Log("Start시점에도 DataTower없음");
        }

        // 처음으로 창이 켜질 때 실행
        InitTrader();
        _isInitialized = true;
    }

    private void OnDisable()
    {
        if(DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney -= UpdateGoldUI;
        }
    }

    private void OnEnable()
    {
        // 다시 켜질 때 실행
        if (_isInitialized && DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney -= UpdateGoldUI;
            DataTower.instance.OnChangedMoney += UpdateGoldUI;
            InitTrader();
        }
    }

    private void InitTrader()
    {
        if(DataTower.instance != null)
        {
            // 보유 골드 갱신
            UpdateGoldUI(DataTower.instance.money);

            // 물고기 정보를 DataTower 인벤토리에서 받아옴
            listManager.RefreshList(DataTower.instance.Items, this);

            if (filterManager != null)
            {   
                // 필터 등급에 맞는 물고기만 보여줌
                listManager.ApplyFilter(filterManager.GetSelectedRates());
            }
            
            OnSlotChanged();
        }
    }

    private void OnDestroy()
    {
        if (DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney -= UpdateGoldUI;
        }    
    }


    // 슬롯의 토글이 바뀌면 FishSlot에서 이 함수 호출
    public void OnSlotChanged()
    {
        long total = selectionManager.TotalSelectedPrice(listManager.GetAllSlots());
        totalPriceText.text = $"Total Price: {total:N0} Gold";
        sellButton.interactable = total > 0;

        UpdateSelectAllToggleState(); 
    }

    public void OnSellButtonClicked()
    {
        var allSlots = listManager.GetAllSlots();
        // 현재 선택된 슬롯 가져옴
        var selected = new List<FishSlot>(selectionManager.SelectedSlots(allSlots));

        if (selected.Count == 0) return;
        
        // 총 판매 가격 계산
        long totalPrice = selectionManager.TotalSelectedPrice(allSlots);

        // 데이터 타워에서 돈 추가 (DataTower에 돈 추가는 false)
        if (DataTower.instance.TryMoenyChanged((ulong)totalPrice, false))
        {
            // 판매된 물고기 삭제
            foreach (var slot in selected)
            {
                FishData data = slot.GetFishData();

                // 인벤토리 데이터를 소스에서 제거
                DataTower.instance.Items.Remove(data);
                
                listManager.GetAllSlots().Remove(slot);
                // 리스트에서 제거
                allSlots.Remove(slot);

                // 오브젝트 파괴
                Destroy(slot.gameObject);
            }

            ResetSelectAllButton();

            OnSlotChanged();

            Debug.Log($"{totalPrice} Gold, 판매 완료");
        }
       
    }

    private void ResetSelectAllButton()
    {
         // 판매 후 전체 선택 버튼 초기화
        if(selectAllButton != null)
        {
            _isUpdatingAll = true;
            selectAllButton.isOn = false;
            _isUpdatingAll = false;
        }
    }
    
    public void OnFilterButtonClicked()
    {
        isFilterMode = !isFilterMode;
        filterPanel.SetActive(isFilterMode);
        filterButtonText.text = isFilterMode ? "Confirm" : "Filter";

        // 확인을 클릭해 창 닫기
        if (!isFilterMode)
        {
            listManager.ApplyFilter(filterManager.GetSelectedRates());
            OnSlotChanged();
        }
    }

    public void OnSelectAllButtonClicked(bool isOn)
    {
        if (_isUpdatingAll) return;

        _isUpdatingAll = true;

        selectionManager.AllSelection(listManager.GetAllSlots(), isOn);
        _isUpdatingAll = false;

        OnSlotChanged();
    }


    private void UpdateSelectAllToggleState()
    {
        if(_isUpdatingAll || selectAllButton == null) return;

        // 리스트를 변수에 담고 Null 체크
        var allSlots = listManager.GetAllSlots();

        if (allSlots == null || allSlots.Count == 0)
        {
            _isUpdatingAll = true;

            if (selectAllButton.isOn)
            {
                selectAllButton.isOn = false;
            }

            _isUpdatingAll = false;
            return;
        }

        // 현재 화면에 활성화된 슬롯만 추출
        var allActiveSlots = listManager.GetAllSlots().FindAll(s => s.gameObject.activeInHierarchy);
        
        // 슬롯이 없다면 전체선택 버튼 끄고 종료
        if(allActiveSlots.Count == 0)
        {
            _isUpdatingAll = true;

            if (selectAllButton.isOn)
            {
                selectAllButton.isOn = false;
            }

            _isUpdatingAll = false;

            return;  
        } 

        // 모든 활성화된 슬롯의 토글이 켜지 있는지
        bool allSelected = allActiveSlots.TrueForAll(s => s.slotToggle.isOn);

        // 전체 선탯 버튼 상태 동기화
        if (selectAllButton.isOn != allSelected)
        {
            _isUpdatingAll = true;

            selectAllButton.isOn = allSelected;

            _isUpdatingAll = false;
        }
    }

    private void UpdateGoldUI(ulong money)
    {
        Debug.Log($"전달받은 금액 : {money}");
        // "N0"로 세자리 당 ',' 찍어주기 
        if (goldText != null) goldText.text = $"Money: {money.ToString("N0")} Gold";

        else
        Debug.Log("goldText가 인스펙터에서 연결되지 않음");
    }
}