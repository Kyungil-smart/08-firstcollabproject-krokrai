using UnityEngine;
using UnityEngine.UI;

public class TraderUI : MonoBehaviour
{
    [Header("스크립트 참조")]
    public TraderEconomy economy;               // 돈, 가격 표시, 판매 로직 담당자
    public TraderFilterUI filterUI;             // 필터 창 열고 닫는 UI 제어 담당자
    public TraderListManager listManager;       // 실제 물고기 슬롯 리스트 생성/삭제 관리자
    public SelectionManager selectionManager;   // 어떤 슬롯이 체크되었는지 계산하는 관리자
    public FilterManager filterLogic;           // 어떤 등급을 걸러낼지 계산하는 관리자
    public Toggle selectAllToggle;              // 전체선택 버튼

    private bool _isInitialized = false;       // Start가 초기설정끝냈는지

    private void Start()
    {
        SetupEvents();

        RefreshAll();

        _isInitialized = true;  
    }

    // 상점 UI가 켜질 때마다 실행되는 초기화
    private void OnEnable()
    {
        if (!_isInitialized) return;

        // 창이 켜질 때 필터와 리스트 상태 초기화
        filterUI.ForceClose();
        filterLogic.ResetFilter();

        if(DataTower.instance != null)
        {
            RefreshAll();
        }
    }

    private void OnDisable()
    {
        CleanEvents();
    }

    // 데이터 타워의 이벤트를 구독하고 초기 값 설정
    private void SetupEvents()
    {
        if(DataTower.instance == null) return;

        //중복구독 방지
        DataTower.instance.OnChangedMoney -= economy.UpdateGoldUI;
        DataTower.instance.OnChangedMoney += economy.UpdateGoldUI;

        economy.UpdateGoldUI(DataTower.instance.money);
    }

    // 데이터 타워와의 이벤트 연결을 끊음
    private void CleanEvents()
    {
        if(DataTower.instance != null)
        {
            DataTower.instance.OnChangedMoney -= economy.UpdateGoldUI;
        }
    }

    // 판매 버튼 클릭 시 선택된 물고기를 팔고 리스트 갱신
    public void OnSellButtonClicked()
    {
        var allSlots = listManager.GetAllSlots();
        
        // 선택된 슬롯만 먼저 가져옴
        var selected = selectionManager.SelectedSlots(allSlots);

        long price = selectionManager.TotalSelectedPrice(selected);

        // 경제 매니저에게 실제 판매
        if (economy.CompleteTrade(selected, price))
        {
            // 리스트에서 제거
            listManager.RemoveSlots(selected);

            // 판매 후 0으로 리셋
            OnSlotChanged();
        }
    }

    // 필터 버튼 클릭 시 호출 필터 창 토글/제어
    public void OnFilterButtonClicked()
    {
        filterUI.ToggleFilter();

        // 필터창을 닫을 때 필터링 적용
        if (!filterUI.isFilterMode)
        {
            selectionManager.AllSelection(listManager.GetAllSlots(), false);
            listManager.ApplyFilter(filterLogic.GetSelectedRates());
            OnSlotChanged();
        }
    }

    // 슬롯 상태 변경 시 호출, 하단 합계 금액 텍스트 갱신
    public void OnSlotChanged()
    {
        var allSlots = listManager.GetAllSlots();

        // 판매 합계 금액
        economy.UpdatePriceUI(selectionManager.TotalSelectedPrice(allSlots));

        // 전체 선택 토글 상태 업데이트
        if (selectAllToggle != null)
        {
            // 루프 방지
            selectAllToggle.onValueChanged.RemoveAllListeners();

            // 다 켜져있는지 확인
            selectAllToggle.isOn = selectionManager.IsAllSelected(allSlots);

            // 리스너 연결
            selectAllToggle.onValueChanged.AddListener(OnSelectAllToggleChanged);
        }
    }

    public void OnSelectAllToggleChanged(bool isOn)
    {
        selectionManager.AllSelection(listManager.GetAllSlots(), isOn);
        OnSlotChanged();
    }

    // 모든 데이터와 UI를 동기화해 새로고침
    private void RefreshAll()
    {
        if(DataTower.instance == null) return;

        // 가지고 있는 잔액
        economy.UpdateGoldUI(DataTower.instance.money);

        // 리스트 생성
        listManager.RefreshList(DataTower.instance.Items, this);
        
        // 필터 적용
        listManager.ApplyFilter(filterLogic.GetSelectedRates());

        // 초기화
        OnSlotChanged();
    }
}