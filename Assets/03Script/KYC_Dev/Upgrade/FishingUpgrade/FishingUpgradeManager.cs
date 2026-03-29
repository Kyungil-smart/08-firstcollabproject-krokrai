using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class FishingUpgradeManager : MonoBehaviour
{
    // 싱글톤 여부는 협의해서 결정
    // 업그레이드 조건 확인, 수행
    // 업그레이드 효과는 다른 컴포넌트에서 수행
    
    private FishingUpgradeDataReader _dataReader;
    private WaitForEndOfFrame _waitForEndOfFrame = new();
    
    /// <summary>
    /// 낚시 등급 : 모든 낚시 업그레이드의 기본
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int FishingGrade;

    /// <summary>
    /// 미끼 레벨
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int BaitLevel;

    /// <summary>
    /// 낚시대 레벨
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int RodLevel;

    /// <summary>
    /// 배 레벨
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int ShipLevel;
    
    private int _fishingGradeReqGold;
    private int _baitLevelReqGold;
    private int _rodLevelReqGold;
    private int _shipLevelReqGold;
    
    #region Events
    
    /// <summary>
    /// 낚시 등급 상승 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnFishingUpgrade;
    
    /// <summary>
    /// 미끼 레벨 상승 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnBaitUpgrade;
    
    /// <summary>
    /// 낚시대 레벨 상승 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnRodUpgrade;
    
    /// <summary>
    /// 배 레벨 상승 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnShipUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanBaitLevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanRodLevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanShipLevelUpgrade;
    
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldFishingGradeUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldBaitLevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldRodLevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldShipLevelUpgrade;

    #endregion

    /// <summary>
    /// ToDo:DataTower로 변수 이관 되면 사용 안함
    /// </summary>
    private void Awake()
    {
        _dataReader = GetComponentInChildren<FishingUpgradeDataReader>();
        
        // ToDo: ToDo:DataTower로 변수 이관 되면 이 밑의 변수는 삭제
        FishingGrade = 1;
        BaitLevel = 1;
        RodLevel = 1;
        ShipLevel = 1;
    }

    private void OnEnable()
    {
        CheckReqGolds();
        StartCoroutine(LoadingOnEnableRoutine());
    }

    private void OnDisable()
    {
        EventDisable();
    }

    #region 이벤트 구독/해제

    private void EventEnable()
    {
        DataTower.instance.OnChangedMoney += CheckEnoughGoldFishingGradeUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldBaitLevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldRodLevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldShipLevelUpgrade;
    }

    private void EventDisable()
    {
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldFishingGradeUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldBaitLevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldRodLevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldShipLevelUpgrade;
    }

    private IEnumerator LoadingOnEnableRoutine()
    {
        while (DataTower.instance == null)
        {
            yield return _waitForEndOfFrame;
        }
        
        EventEnable();
    }

    #endregion

    #region 업그레이드 실행

    /// <summary>
    /// 낚시 레벨 증가
    /// </summary>
    public void FishingUpgrade()
    {
        if (CanFishingGradeUp(DataTower.instance.money))
        {
            FishingGrade++;
            DataTower.instance.TryMoenyChanged((ulong)_fishingGradeReqGold);
            OnFishingUpgrade?.Invoke(FishingGrade);
            CheckCanUpgrades();
            CheckReqGolds();
        }
        
    }

    /// <summary>
    /// 미끼 레벨 증가
    /// </summary>
    public void BaitUpgrade()
    {
        if (CanBaitLevelUp(DataTower.instance.money))
        {
            BaitLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_baitLevelReqGold);
            OnBaitUpgrade?.Invoke(BaitLevel);
            CheckCanUpgrades();
            CheckReqGolds();
        }
        
    }

    /// <summary>
    /// 낚시대 레벨 증가
    /// </summary>
    public void RodUpgrade()
    {
        if (CanRodLevelUp(DataTower.instance.money))
        {
            RodLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_rodLevelReqGold);
            OnRodUpgrade?.Invoke(RodLevel);
            CheckCanUpgrades();
            CheckReqGolds();
        }
        
    }

    /// <summary>
    /// 보트 레벨 증가
    /// </summary>
    public void ShipUpgrade()
    {
        if (CanShipLevelUp(DataTower.instance.money))
        {
            ShipLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_shipLevelReqGold);
            OnShipUpgrade?.Invoke(ShipLevel);
            CheckCanUpgrades();
            CheckReqGolds();
        }
        
    }

    #endregion

    #region 업그레이드 관련 메서드

    private bool CanFishingGradeUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Grades.Length > FishingGrade;
        bool chackGold = curGold >= (ulong)_fishingGradeReqGold;
        
        return chackGold && chackLevel;
    }

    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldFishingGradeUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_fishingGradeReqGold;
        EnoughGoldFishingGradeUpgrade?.Invoke(result);
    }
    
    private bool CanBaitLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Grades.Length > BaitLevel;
        bool chackGold = curGold >= (ulong)_baitLevelReqGold;
        bool chackGrade = CheckCanBaitLevelUpgrade(FishingGrade,BaitLevel);
        
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldBaitLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_baitLevelReqGold;
        EnoughGoldBaitLevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="fishingGrade"></param>
    /// <param name="baitLevel"></param>
    /// <returns></returns>
    public bool CheckCanBaitLevelUpgrade(int fishingGrade, int baitLevel)
    {
        bool result = fishingGrade/2f > baitLevel;
        CanBaitLevelUpgrade?.Invoke(result);

        return result;
    }

    private bool CanRodLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Grades.Length > RodLevel;
        bool chackGold = curGold >= (ulong)_rodLevelReqGold;
        bool chackGrade = CheckCanRodLevelUpgrade(FishingGrade,RodLevel);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldRodLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_rodLevelReqGold;
        EnoughGoldRodLevelUpgrade?.Invoke(result);
    }
    
    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="fishingGrade"></param>
    /// <param name="rodLevel"></param>
    /// <returns></returns>
    public bool CheckCanRodLevelUpgrade(int fishingGrade, int rodLevel)
    {
        bool result = fishingGrade/2f > rodLevel;
        CanRodLevelUpgrade?.Invoke(result);

        return result;
    }

    private bool CanShipLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Grades.Length > ShipLevel;
        bool chackGold = curGold >= (ulong)_shipLevelReqGold;
        bool chackGrade = CheckCanShipLevelUpgrade(FishingGrade,ShipLevel);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldShipLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_shipLevelReqGold;
        EnoughGoldShipLevelUpgrade?.Invoke(result);
    }
    
    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="fishingGrade"></param>
    /// <param name="shipLevel"></param>
    /// <returns></returns>
    public bool CheckCanShipLevelUpgrade(int fishingGrade, int shipLevel)
    {
        bool result = fishingGrade/2f > shipLevel;
        CanShipLevelUpgrade?.Invoke(result);

        return result;
    }
    
    /// <summary>
    /// 업그레이드 가능한지 체크
    /// </summary>
    public void CheckCanUpgrades()
    {
        CheckCanBaitLevelUpgrade(FishingGrade,BaitLevel);
        CheckCanRodLevelUpgrade(FishingGrade, RodLevel);
        CheckCanShipLevelUpgrade(FishingGrade, ShipLevel);
    }
    
    private void CheckReqGolds()
    {
        _dataReader.GetFishingGradeReqGoldData(FishingGrade ,out _fishingGradeReqGold);
        _dataReader.GetBaitLevelReqGoldData(BaitLevel ,out _baitLevelReqGold);
        _dataReader.GetRodLevelReqGoldData(RodLevel ,out _rodLevelReqGold);
        _dataReader.GetShipLevelReqGoldData(ShipLevel ,out _shipLevelReqGold);
    }

    #endregion
}
