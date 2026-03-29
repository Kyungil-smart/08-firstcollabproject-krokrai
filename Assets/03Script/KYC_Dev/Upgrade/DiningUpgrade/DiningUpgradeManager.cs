using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class DiningUpgradeManager : MonoBehaviour
{
    // 싱글톤 여부는 협의해서 결정
    // 업그레이드 조건 확인, 수행
    // 업그레이드 효과는 다른 컴포넌트에서 수행
    
    private DiningUpgradeDataReader _dataReader;
    private WaitForEndOfFrame _waitForEndOfFrame = new();

    /// <summary>
    /// 식당 마스터 레벨
    /// </summary>
    public int MasterLevel;
    
    /// <summary>
    /// 좌석 업그레이드 레벨
    /// </summary>
    public int MaxCustomerLimitLevel;
    
    /// <summary>
    /// 특별 손님 업그레이드 레벨
    /// </summary>
    public int MaxSpawnLimit01Level;
    
    /// <summary>
    /// VIP 업그레이드 레벨
    /// </summary>
    public int MaxSpawnLimit02Level;
    
    /// <summary>
    /// 팁주는 손님 가중치 업그레이드 레벨
    /// </summary>
    public int WeightLevel;
    
    /// <summary>
    /// 모금함(팁 액수 증가) 업그레이드 레벨
    /// </summary>
    public int BonusTipsMultiLevel;
    
    /// <summary>
    /// 계산대(요리 가격 증가) 업그레이드 레벨
    /// </summary>
    public int BonusDishPrice01Level;
    
    /// <summary>
    /// 밥솥(요리 가격 증가) 업그레이드 레벨
    /// </summary>
    public int BonusDishPrice02Level;
    
    /// <summary>
    /// 식칼(요리 개수 증가) 업그레이드 레벨
    /// </summary>
    public int BonusFood01Level;
    
    /// <summary>
    /// 도마(요리 개수 증가) 업그레이드 레벨
    /// </summary>
    public int BonusFood02Level;
    
    /// <summary>
    /// 고양이 업그레이드 레벨
    /// </summary>
    public int UnlockCatObjectLevel;
    
    private int _masterLevelCost;
    private int _maxCustomerLimitLevelCost;
    private int _maxSpawnLimit01LevelCost;
    private int _maxSpawnLimit02LevelCost;
    private int _weightLevelCost;
    private int _bonusTipsMultiLevelCost;
    private int _bonusDishPrice01LevelCost;
    private int _bonusDishPrice02LevelCost;
    private int _bonusFood01LevelCost;
    private int _bonusFood02LevelCost;
    private int _unlockCatObjectLevelCost;
    
    #region Events
    
    /// <summary>
    /// 마스터 레벨 상승 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnMasterLevelUpgrade;
    
    /// <summary>
    /// 좌석 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnMaxCustomerLimitLevelUpgrade;
    
    /// <summary>
    /// 특별 손님 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnMaxSpawnLimit01LevelUpgrade;
    
    /// <summary>
    /// VIP 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnMaxSpawnLimit02LevelUpgrade;
   
    /// <summary>
    /// 간판(팁 주는 손님) 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnWeightLevelUpgrade;
    
    /// <summary>
    /// 모금함(팁 액수 증가) 업그레이드시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnBonusTipsMultiLevelUpgrade;
    
    /// <summary>
    /// 계산대(요리 가격 증가) 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnBonusDishPrice01LevelUpgrade;
    
    /// <summary>
    /// 밥솥(요리 가격 증가) 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnBonusDishPrice02LevelUpgrade;
    
    /// <summary>
    /// 식칼(요리 개수 증가) 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnBonusFood01LevelUpgrade;
    
    /// <summary>
    /// 도마(요리 개수 증가) 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnBonusFood02LevelUpgrade;
  
    /// <summary>
    /// 고양이 업그레이드 시 효과 발동을 위한 이벤트
    /// </summary>
    public event Action<int> OnUnlockCatObjectLevelUpgrade;
    
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanMaxCustomerLimitLevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanMaxSpawnLimit01LevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanMaxSpawnLimit02LevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanWeightLevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanBonusTipsMultiLevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanBonusDishPrice01LevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanBonusDishPrice02LevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanBonusFood01LevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanBonusFood02LevelUpgrade;
    
    /// <summary>
    /// 업그레이드가 불가능하면 버튼을 사용 불가로 만드는 이벤트
    /// </summary>
    public event Action<bool> CanUnlockCatObjectLevelUpgrade;
    
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldMasterLevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldMaxCustomerLevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldMaxSpawnLimit01LevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldMaxSpawnLimit02LevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldWeightLevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldBonusTipsMultiLevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldBonusDishPrice01LevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldBonusDishPrice02LevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldBonusFood01LevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldBonusFood02LevelUpgrade;
    
    /// <summary>
    /// 골드가 부족하면 버튼의 색을 변화하는 이벤트
    /// </summary>
    public event Action<bool> EnoughGoldUnlockCatObjectLevelUpgrade;

    #endregion
    
    private void Awake()
    {
        _dataReader = GetComponentInChildren<DiningUpgradeDataReader>();
        
        // ToDo: ToDo:DataTower로 변수 이관 되면 이 밑의 변수는 삭제
        MasterLevel = 1;
        MaxCustomerLimitLevel = 1;
        MaxSpawnLimit01Level = 1;
        MaxSpawnLimit02Level = 1;
        WeightLevel = 1;
        BonusTipsMultiLevel = 1;
        BonusDishPrice01Level = 1;
        BonusDishPrice02Level = 1;
        BonusFood01Level = 1;
        BonusFood02Level = 1;
        UnlockCatObjectLevel = 1;
    }

    private void OnEnable()
    {
        CheckCosts();
        StartCoroutine(LoadingOnEnableRoutine());
    }

    private void OnDisable()
    {
        EventDisable();
    }

    #region 이벤트 구독/해제

    private void EventEnable()
    {
        DataTower.instance.OnChangedMoney += CheckEnoughGoldMasterLevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldMaxCustomerLimitLevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldMaxSpawnLimit01LevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldMaxSpawnLimit02LevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldWeightLevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldBonusTipsMultiLevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldBonusDishPrice01LevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldBonusDishPrice02LevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldBonusFood01LevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldBonusFood02LevelUpgrade;
        DataTower.instance.OnChangedMoney += CheckEnoughGoldUnlockCatObjectLevelUpgrade;
    }

    private void EventDisable()
    {
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldMasterLevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldMaxCustomerLimitLevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldMaxSpawnLimit01LevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldMaxSpawnLimit02LevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldWeightLevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldBonusTipsMultiLevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldBonusDishPrice01LevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldBonusDishPrice02LevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldBonusFood01LevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldBonusFood02LevelUpgrade;
        DataTower.instance.OnChangedMoney -= CheckEnoughGoldUnlockCatObjectLevelUpgrade;
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
    /// 마스터 레벨 증가
    /// </summary>
    public void MasterLevelUpgrade()
    {
        if (CanMasterLevelUp(DataTower.instance.money))
        {
            MasterLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_masterLevelCost);
            OnMasterLevelUpgrade?.Invoke(MasterLevel);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }
    
    /// <summary>
    /// 좌석 레벨 증가
    /// </summary>
    public void MaxCustomerLimitLevelUpgrade()
    {
        if (CanMaxCustomerLimitLevelUp(DataTower.instance.money))
        {
            MaxCustomerLimitLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_maxCustomerLimitLevelCost);
            OnMaxCustomerLimitLevelUpgrade?.Invoke(MaxCustomerLimitLevel);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }
    
    /// <summary>
    /// 특별 손님 레벨 증가
    /// </summary>
    public void MaxSpawnLimit01LevelUpgrade()
    {
        if (CanMaxSpawnLimit01LevelUp(DataTower.instance.money))
        {
            MaxSpawnLimit01Level++;
            DataTower.instance.TryMoenyChanged((ulong)_maxSpawnLimit01LevelCost);
            OnMaxSpawnLimit01LevelUpgrade?.Invoke(MaxSpawnLimit01Level);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }
    
    /// <summary>
    /// VIP 레벨 증가
    /// </summary>
    public void MaxSpawnLimit02LevelUpgrade()
    {
        if (CanMaxSpawnLimit02LevelUp(DataTower.instance.money))
        {
            MaxSpawnLimit02Level++;
            DataTower.instance.TryMoenyChanged((ulong)_maxSpawnLimit02LevelCost);
            OnMaxSpawnLimit02LevelUpgrade?.Invoke(MaxSpawnLimit02Level);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }
    
    /// <summary>
    /// 팁 주는 손님 가중치 레벨 증가
    /// </summary>
    public void WeightLevelUpgrade()
    {
        if (CanWeightLevelUp(DataTower.instance.money))
        {
            WeightLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_weightLevelCost);
            OnWeightLevelUpgrade?.Invoke(WeightLevel);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }
    
    /// <summary>
    /// 모금함(팁 액수 증가) 레벨 증가
    /// </summary>
    public void BonusTipsMultiLevelUpgrade()
    {
        if (CanBonusTipsMultiLevelUp(DataTower.instance.money))
        {
            BonusTipsMultiLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_bonusTipsMultiLevelCost);
            OnBonusTipsMultiLevelUpgrade?.Invoke(BonusTipsMultiLevel);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }

    /// <summary>
    /// 계산대(요리 가격 증가) 레벨 증가
    /// </summary>
    public void BonusDishPrice01LevelUpgrade()
    {
        if (CanBonusDishPrice01LevelUp(DataTower.instance.money))
        {
            BonusDishPrice01Level++;
            DataTower.instance.TryMoenyChanged((ulong)_bonusDishPrice01LevelCost);
            OnBonusDishPrice01LevelUpgrade?.Invoke(BonusDishPrice01Level);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }

    /// <summary>
    /// 밥솥(요리 가격 증가) 레벨 증가
    /// </summary>
    public void BonusDishPrice02LevelUpgrade()
    {
        if (CanBonusDishPrice02LevelUp(DataTower.instance.money))
        {
            BonusDishPrice02Level++;
            DataTower.instance.TryMoenyChanged((ulong)_bonusDishPrice02LevelCost);
            OnBonusDishPrice02LevelUpgrade?.Invoke(BonusDishPrice02Level);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }

    /// <summary>
    /// 식칼(요리 개수 증가) 레벨 증가
    /// </summary>
    public void BonusFood01LevelUpgrade()
    {
        if (CanBonusFood01LevelUp(DataTower.instance.money))
        {
            BonusFood01Level++;
            DataTower.instance.TryMoenyChanged((ulong)_bonusFood01LevelCost);
            OnBonusFood01LevelUpgrade?.Invoke(BonusFood01Level);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }
    
    /// <summary>
    /// 도마(요리 개수 증가) 레벨 증가
    /// </summary>
    public void BonusFood02LevelUpgrade()
    {
        if (CanBonusFood02LevelUp(DataTower.instance.money))
        {
            BonusFood02Level++;
            DataTower.instance.TryMoenyChanged((ulong)_bonusFood02LevelCost);
            OnBonusFood02LevelUpgrade?.Invoke(BonusFood02Level);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }
    
    /// <summary>
    /// 고양이 레벨 증가
    /// </summary>
    public void UnlockCatObjectLevelUpgrade()
    {
        if (CanUnlockCatObjectLevelUp(DataTower.instance.money))
        {
            UnlockCatObjectLevel++;
            DataTower.instance.TryMoenyChanged((ulong)_unlockCatObjectLevelCost);
            OnUnlockCatObjectLevelUpgrade?.Invoke(UnlockCatObjectLevel);
            CheckCanUpgrades();
            CheckCosts();
        }
        
    }

    #endregion

    #region 업그레이드 관련 메서드

    private bool CanMasterLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Master_Lv.Length > MasterLevel;
        bool chackGold = curGold >= (ulong)_masterLevelCost;
        
        return chackGold && chackLevel;
    }

    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldMasterLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_masterLevelCost;
        EnoughGoldMasterLevelUpgrade?.Invoke(result);
    }
    
    private bool CanMaxCustomerLimitLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Max_Customer_Limit.Length > MaxCustomerLimitLevel;
        bool chackGold = curGold >= (ulong)_maxCustomerLimitLevelCost;
        bool chackGrade = CheckCanMaxCustomerLimitLevelUpgrade(MasterLevel,MaxCustomerLimitLevel);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldMaxCustomerLimitLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_maxCustomerLimitLevelCost;
        EnoughGoldMaxCustomerLevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="maxCustomerLimitLevel"></param>
    /// <returns></returns>
    public bool CheckCanMaxCustomerLimitLevelUpgrade(int masterLevel, int maxCustomerLimitLevel)
    {
        bool result = masterLevel > maxCustomerLimitLevel;
        CanMaxCustomerLimitLevelUpgrade?.Invoke(result);

        return result;
    }

    private bool CanMaxSpawnLimit01LevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Max_Spawn_Limit_1.Length > MaxSpawnLimit01Level;
        bool chackGold = curGold >= (ulong)_maxCustomerLimitLevelCost;
        bool chackGrade = CheckCanMaxSpawnLimit01LevelUpgrade(MasterLevel, MaxSpawnLimit01Level);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldMaxSpawnLimit01LevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_maxSpawnLimit01LevelCost;
        EnoughGoldMaxSpawnLimit01LevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="maxSpawnLimit01Level"></param>
    /// <returns></returns>
    public bool CheckCanMaxSpawnLimit01LevelUpgrade(int masterLevel, int maxSpawnLimit01Level)
    {
        bool result = masterLevel > maxSpawnLimit01Level;
        CanMaxSpawnLimit01LevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanMaxSpawnLimit02LevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Max_Spawn_Limit_2.Length > MaxSpawnLimit02Level;
        bool chackGold = curGold >= (ulong)_maxCustomerLimitLevelCost;
        bool chackGrade = CheckCanMaxSpawnLimit02LevelUpgrade(MasterLevel, MaxSpawnLimit02Level);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldMaxSpawnLimit02LevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_maxSpawnLimit02LevelCost;
        EnoughGoldMaxSpawnLimit02LevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="maxSpawnLimit02Level"></param>
    /// <returns></returns>
    public bool CheckCanMaxSpawnLimit02LevelUpgrade(int masterLevel, int maxSpawnLimit02Level)
    {
        bool result = masterLevel > maxSpawnLimit02Level;
        CanMaxSpawnLimit02LevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanWeightLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Weight.Length > WeightLevel;
        bool chackGold = curGold >= (ulong)_weightLevelCost;
        bool chackGrade = CheckCanWeightLevelUpgrade(MasterLevel, WeightLevel);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldWeightLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_weightLevelCost;
        EnoughGoldWeightLevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="weightLevel"></param>
    /// <returns></returns>
    public bool CheckCanWeightLevelUpgrade(int masterLevel, int weightLevel)
    {
        bool result = masterLevel > weightLevel;
        CanWeightLevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanBonusTipsMultiLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Bonus_Tips_Multi.Length > BonusTipsMultiLevel;
        bool chackGold = curGold >= (ulong)_bonusTipsMultiLevelCost;
        bool chackGrade = CheckCanBonusTipsMultiLevelUpgrade(MasterLevel, BonusTipsMultiLevel);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldBonusTipsMultiLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_bonusTipsMultiLevelCost;
        EnoughGoldBonusTipsMultiLevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="bonusTipsMultiLevel"></param>
    /// <returns></returns>
    public bool CheckCanBonusTipsMultiLevelUpgrade(int masterLevel, int bonusTipsMultiLevel)
    {
        bool result = masterLevel > bonusTipsMultiLevel;
        CanBonusTipsMultiLevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanBonusDishPrice01LevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Bonus_Dish_Price_1.Length > BonusDishPrice01Level;
        bool chackGold = curGold >= (ulong)_bonusDishPrice01LevelCost;
        bool chackGrade = CheckCanBonusDishPrice01LevelUpgrade(MasterLevel, BonusDishPrice01Level);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldBonusDishPrice01LevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_bonusDishPrice01LevelCost;
        EnoughGoldBonusDishPrice01LevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="bonusDishPrice01Level"></param>
    /// <returns></returns>
    public bool CheckCanBonusDishPrice01LevelUpgrade(int masterLevel, int bonusDishPrice01Level)
    {
        bool result = masterLevel > bonusDishPrice01Level;
        CanBonusDishPrice01LevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanBonusDishPrice02LevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Bonus_Dish_Price_2.Length > BonusDishPrice02Level;
        bool chackGold = curGold >= (ulong)_bonusDishPrice02LevelCost;
        bool chackGrade = CheckCanBonusDishPrice02LevelUpgrade(MasterLevel, BonusDishPrice02Level);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldBonusDishPrice02LevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_bonusDishPrice02LevelCost;
        EnoughGoldBonusDishPrice02LevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="bonusDishPrice02Level"></param>
    /// <returns></returns>
    public bool CheckCanBonusDishPrice02LevelUpgrade(int masterLevel, int bonusDishPrice02Level)
    {
        bool result = masterLevel > bonusDishPrice02Level;
        CanBonusDishPrice02LevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanBonusFood01LevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Bonus_Food_1.Length > BonusFood01Level;
        bool chackGold = curGold >= (ulong)_bonusFood01LevelCost;
        bool chackGrade = CheckCanBonusFood01LevelUpgrade(MasterLevel, BonusFood01Level);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldBonusFood01LevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_bonusFood01LevelCost;
        EnoughGoldBonusFood01LevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="bonusFood01Level"></param>
    /// <returns></returns>
    public bool CheckCanBonusFood01LevelUpgrade(int masterLevel, int bonusFood01Level)
    {
        bool result = masterLevel > bonusFood01Level;
        CanBonusFood01LevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanBonusFood02LevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Bonus_Food_2.Length > BonusFood02Level;
        bool chackGold = curGold >= (ulong)_bonusFood02LevelCost;
        bool chackGrade = CheckCanBonusFood02LevelUpgrade(MasterLevel, BonusFood02Level);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldBonusFood02LevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_bonusFood02LevelCost;
        EnoughGoldBonusFood02LevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="bonusFood02Level"></param>
    /// <returns></returns>
    public bool CheckCanBonusFood02LevelUpgrade(int masterLevel, int bonusFood02Level)
    {
        bool result = masterLevel > bonusFood02Level;
        CanBonusFood02LevelUpgrade?.Invoke(result);

        return result;
    }
    
    private bool CanUnlockCatObjectLevelUp(ulong curGold)
    {
        bool chackLevel = _dataReader.Unlock_Cat_Object.Length > UnlockCatObjectLevel;
        bool chackGold = curGold >= (ulong)_unlockCatObjectLevelCost;
        bool chackGrade = CheckCanUnlockCatObjectLevelUpgrade(MasterLevel, UnlockCatObjectLevel);
        
        return chackGold && chackLevel && chackGrade;
    }
    
    /// <summary>
    /// 골드 업그레이드 가능한지 체크해서 UI 필요 골드의 색상 변화
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="curGold"></param>
    public void CheckEnoughGoldUnlockCatObjectLevelUpgrade(ulong curGold)
    {
        bool result = curGold >= (ulong)_unlockCatObjectLevelCost;
        EnoughGoldUnlockCatObjectLevelUpgrade?.Invoke(result);
    }

    /// <summary>
    /// 버튼 활성화 가능한지 체크해서 버튼 활성화 세팅
    /// 이벤트도 연결 되어있지만 UI Enable시 마다 실행 해줘야되서 public임
    /// </summary>
    /// <param name="masterLevel"></param>
    /// <param name="unlockCatObjectLevel"></param>
    /// <returns></returns>
    public bool CheckCanUnlockCatObjectLevelUpgrade(int masterLevel, int unlockCatObjectLevel)
    {
        bool result = masterLevel > unlockCatObjectLevel;
        CanUnlockCatObjectLevelUpgrade?.Invoke(result);

        return result;
    }
    
    /// <summary>
    /// 업그레이드 가능한지 체크
    /// </summary>
    public void CheckCanUpgrades()
    {
        CheckCanMaxCustomerLimitLevelUpgrade(MasterLevel,MaxCustomerLimitLevel);
        CheckCanMaxSpawnLimit01LevelUpgrade(MasterLevel, MaxSpawnLimit01Level);
        CheckCanMaxSpawnLimit02LevelUpgrade(MasterLevel, MaxSpawnLimit02Level);
        CheckCanWeightLevelUpgrade(MasterLevel, WeightLevel);
        CheckCanBonusTipsMultiLevelUpgrade(MasterLevel, BonusTipsMultiLevel);
        CheckCanBonusDishPrice01LevelUpgrade(MasterLevel, BonusDishPrice01Level);
        CheckCanBonusDishPrice02LevelUpgrade(MasterLevel, BonusDishPrice02Level);
        CheckCanBonusFood01LevelUpgrade(MasterLevel, BonusFood01Level);
        CheckCanBonusFood02LevelUpgrade(MasterLevel, BonusFood02Level);
        CheckCanUnlockCatObjectLevelUpgrade(MasterLevel, UnlockCatObjectLevel);
    }
    
    private void CheckCosts()
    {
        _dataReader.GetMasterLevelCostData(MasterLevel,out _masterLevelCost);
        _dataReader.GetMaxCustomerLimitCostData(MaxCustomerLimitLevel,out _maxCustomerLimitLevelCost);
        _dataReader.GetMaxSpawnLimit01CostData(MaxSpawnLimit01Level,out _maxSpawnLimit01LevelCost);
        _dataReader.GetMaxSpawnLimit02CostData(MaxSpawnLimit02Level,out _maxSpawnLimit02LevelCost);
        _dataReader.GetWeightCostData(WeightLevel,out _weightLevelCost);
        _dataReader.GetBonusTipsMultiCostData(BonusTipsMultiLevel,out _bonusTipsMultiLevelCost);
        _dataReader.GetBonusDishPrice01CostData(BonusDishPrice01Level, out _bonusDishPrice01LevelCost);
        _dataReader.GetBonusDishPrice02CostData(BonusDishPrice02Level, out _bonusDishPrice02LevelCost);
        _dataReader.GetBonusFood01CostData(BonusFood01Level, out _bonusFood01LevelCost);
        _dataReader.GetBonusFood02CostData(BonusFood02Level, out _bonusFood02LevelCost);
        _dataReader.GetUnlockCatObjectCostData(UnlockCatObjectLevel,out _unlockCatObjectLevelCost);
    }

    #endregion
}
