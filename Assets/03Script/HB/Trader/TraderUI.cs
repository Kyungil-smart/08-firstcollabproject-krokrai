using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Awake()
    {
        // DataTower에서 돈 관련 내용 구독
        if (DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney += UpdateGoldUI;

            // DataTower 돈 추가기능 필요
            UpdateGoldUI(DataTower.instance.money);          
        }

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

    private void OnEnable()
    {
        if (DataTower.instance != null)
        {
            // 리스트 생성
            listManager.RefreshList(DataTower.instance.Items, this);
            
            // 리스트가 생성된 직후, 현재 필터 상태를 즉시 적용
            if (filterManager != null)
            {
                listManager.ApplyFilter(filterManager.GetSelectedRates());
            }

            // 금액 및 전체 선택 버튼 상태 갱신
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
        var selected = selectionManager.SelectedSlots(listManager.GetAllSlots());
        long price = selectionManager.TotalSelectedPrice(listManager.GetAllSlots());

        // 데이터 타워에서 돈 받아옴
        DataTower.instance.TryMoenyChanged((ulong)price);

        // 판매된 물고기 삭제
        foreach (var slot in selected)
        {
            DataTower.instance.Items.Remove(slot.GetFishData());
            Destroy(slot.gameObject);
        }

        // 내부 리스트 갱신
        listManager.GetAllSlots().RemoveAll(s => s == null || selected.Contains(s));

        // UI 정리
        if(selectAllButton != null) selectAllButton.isOn = false;
        OnSlotChanged();
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

        var allActiveSlots = listManager.GetAllSlots().FindAll(s => s.gameObject.activeInHierarchy);
        
        // 슬롯이 없다면 전체선택 버튼 끔
        if(allActiveSlots.Count == 0)
        {
            _isUpdatingAll = true;
            selectAllButton.isOn = false;
            _isUpdatingAll = false;

            return;  
        } 

        bool allSelected = allActiveSlots.TrueForAll(s => s.slotToggle.isOn);

        _isUpdatingAll = true;
        selectAllButton.isOn = allSelected;
        _isUpdatingAll = false;
    }

    private void UpdateGoldUI(ulong money)
    {
        // "N0"로 세자리 당 ',' 찍어주기 
        if (goldText != null) goldText.text = $"{money.ToString("N0")} Gold";
    }
}