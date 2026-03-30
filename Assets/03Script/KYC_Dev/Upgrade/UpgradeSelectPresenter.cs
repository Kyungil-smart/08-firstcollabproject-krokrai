using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeSelectPresenter : MonoBehaviour
{
    [SerializeField] private UpgradeUIView _upgradeUIView;
    
    [Header("Children Upgrade Select Views")]
    [SerializeField] UpgradeSelectView[] _views;
    [SerializeField] Image[] _upgradeSprites;
    
    [Header("Upgrade Type")]
    [field: SerializeField]
    public EMainUpgradeType MainUpgradeType { get; private set; }
    
    private FishingUpgradeManager _fUpgrade;
    private FishingUpgradeDataReader _fDataReader;
    
    //ToDo:Dining업그레이드 완료되면 하단 수정
    private DiningUpgradeManager _dUpgrade;
    private DiningUpgradeDataReader _dDataReader;
    
    private TranslationDataReader _tDataReader;
    private WaitForEndOfFrame _waitForEndOfFrame = new ();

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        StartCoroutine(LoadingOnEnableRoutine());
    }

    private void OnDisable()
    {
        DisableEvents();
    }

    private void Init()
    {
        _fUpgrade = FindFirstObjectByType<FishingUpgradeManager>();
        _fDataReader = FindFirstObjectByType<FishingUpgradeDataReader>();
        _dUpgrade = FindFirstObjectByType<DiningUpgradeManager>();
        _dDataReader = FindFirstObjectByType<DiningUpgradeDataReader>();
        _tDataReader = FindFirstObjectByType<TranslationDataReader>();
    }

    private void RunSettings()
    {
        switch (MainUpgradeType)
        {
            case EMainUpgradeType.Fishing:
                SetViewPlayerGrade();
                SetViewBaitLevel();
                SetViewRodLevel();
                SetViewShipLevel();
                SetFishingTranslationTextSetting();
                _fUpgrade.CheckCanUpgrades();
                break;
            
            case EMainUpgradeType.Dining:
                SetViewMasterLevel();
                SetViewMaxCustomerLimitLevel();
                SetViewMaxSpawnLimit01Level();
                SetViewMaxSpawnLimit02Level();
                SetViewWeightLevel();
                SetViewBonusTipsMultiLevel();
                SetViewBonusDishPrice01Level();
                SetViewBonusDishPrice02Level();
                SetViewBonusFood01Level();
                SetViewBonusFood02Level();
                SetViewUnlockCatObjectLevel();
                SetDiningTranslationTextSetting();
                _dUpgrade.CheckCanUpgrades();
                break;
        }
    }

    #region 이벤트 구독/해제

    private void EnableEvents()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            DataTower.instance.OnLanguageSettingChanged += _views[i].TranslationText;
        }
        switch (MainUpgradeType)
        {
            case EMainUpgradeType.Fishing:
                
                _fUpgrade.OnFishingUpgrade += RenewalPlayerGrade;
                _fUpgrade.OnBaitUpgrade += RenewalBaitLevel;
                _fUpgrade.OnRodUpgrade += RenewalRodLevel;
                _fUpgrade.OnShipUpgrade += RenewalShipLevel;
                
                // EFishingUpgradeType 인덱스 번호로 정렬
                for (int i = 0; i < _views.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _views[i].OnTryUpgrade += _fUpgrade.FishingUpgrade;
                            _fUpgrade.EnoughGoldFishingGradeUpgrade += _views[i].ToggleReqGoldTextColor;
                            break;
                        case 1:
                            _views[i].OnTryUpgrade += _fUpgrade.BaitUpgrade;
                            _fUpgrade.EnoughGoldBaitLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _fUpgrade.CanBaitLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 2:
                            _views[i].OnTryUpgrade += _fUpgrade.RodUpgrade;
                            _fUpgrade.EnoughGoldRodLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _fUpgrade.CanRodLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 3:
                            _views[i].OnTryUpgrade += _fUpgrade.ShipUpgrade;
                            _fUpgrade.EnoughGoldShipLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _fUpgrade.CanShipLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        default:
                            break;
                    }
                }
                break;
            
            // EDiningUpgradeType 인덱스 번호로 정렬
            case EMainUpgradeType.Dining:

                _dUpgrade.OnMasterLevelUpgrade += RenewalMasterLevel;
                _dUpgrade.OnMaxCustomerLimitLevelUpgrade += RenewalMaxCustomerLimitLevel;
                _dUpgrade.OnMaxSpawnLimit01LevelUpgrade += RenewalMaxSpawnLimit01Level;
                _dUpgrade.OnMaxSpawnLimit02LevelUpgrade += RenewalMaxSpawnLimit02Level;
                _dUpgrade.OnWeightLevelUpgrade += RenewalWeightLevel;
                _dUpgrade.OnBonusTipsMultiLevelUpgrade += RenewalBonusTipsMultiLevel;
                _dUpgrade.OnBonusDishPrice01LevelUpgrade += RenewalBonusDishPrice01Level;
                _dUpgrade.OnBonusDishPrice02LevelUpgrade += RenewalBonusDishPrice02Level;
                _dUpgrade.OnBonusFood01LevelUpgrade += RenewalBonusFood01Level;
                _dUpgrade.OnBonusFood02LevelUpgrade += RenewalBonusFood02Level;
                _dUpgrade.OnUnlockCatObjectLevelUpgrade += RenewalUnlockCatObjectLevel;
                
                for (int i = 0; i < _views.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _views[i].OnTryUpgrade += _dUpgrade.MasterLevelUpgrade;
                            _dUpgrade.EnoughGoldMasterLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            break;
                        case 1:
                            _views[i].OnTryUpgrade += _dUpgrade.MaxCustomerLimitLevelUpgrade;
                            _dUpgrade.EnoughGoldMaxCustomerLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanMaxCustomerLimitLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 2:
                            _views[i].OnTryUpgrade += _dUpgrade.MaxSpawnLimit01LevelUpgrade;
                            _dUpgrade.EnoughGoldMaxSpawnLimit01LevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanMaxSpawnLimit01LevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 3:
                            _views[i].OnTryUpgrade += _dUpgrade.MaxSpawnLimit02LevelUpgrade;
                            _dUpgrade.EnoughGoldMaxSpawnLimit02LevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanMaxSpawnLimit02LevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 4:
                            _views[i].OnTryUpgrade += _dUpgrade.WeightLevelUpgrade;
                            _dUpgrade.EnoughGoldWeightLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanWeightLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 5:
                            _views[i].OnTryUpgrade += _dUpgrade.BonusTipsMultiLevelUpgrade;
                            _dUpgrade.EnoughGoldBonusTipsMultiLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusTipsMultiLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 6:
                            _views[i].OnTryUpgrade += _dUpgrade.BonusDishPrice01LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusDishPrice01LevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusDishPrice01LevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 7:
                            _views[i].OnTryUpgrade += _dUpgrade.BonusDishPrice02LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusDishPrice02LevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusDishPrice02LevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 8:
                            _views[i].OnTryUpgrade += _dUpgrade.BonusFood01LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusFood01LevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusFood01LevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 9:
                            _views[i].OnTryUpgrade += _dUpgrade.BonusFood02LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusFood02LevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusFood02LevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 10:
                            _views[i].OnTryUpgrade += _dUpgrade.UnlockCatObjectLevelUpgrade;
                            _dUpgrade.EnoughGoldUnlockCatObjectLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanUnlockCatObjectLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }

    private void DisableEvents()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            DataTower.instance.OnLanguageSettingChanged -= _views[i].TranslationText;
        }
        switch (MainUpgradeType)
        {
            case EMainUpgradeType.Fishing:
                
                _fUpgrade.OnFishingUpgrade -= RenewalPlayerGrade;
                _fUpgrade.OnBaitUpgrade -= RenewalBaitLevel;
                _fUpgrade.OnRodUpgrade -= RenewalRodLevel;
                _fUpgrade.OnShipUpgrade -= RenewalShipLevel;
                
                for (int i = 0; i < _views.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _views[i].OnTryUpgrade -= _fUpgrade.FishingUpgrade;
                            _fUpgrade.EnoughGoldFishingGradeUpgrade -= _views[i].ToggleReqGoldTextColor;
                            break;
                        case 1:
                            _views[i].OnTryUpgrade -= _fUpgrade.BaitUpgrade;
                            _fUpgrade.EnoughGoldBaitLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _fUpgrade.CanBaitLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 2:
                            _views[i].OnTryUpgrade -= _fUpgrade.RodUpgrade;
                            _fUpgrade.EnoughGoldRodLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _fUpgrade.CanRodLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 3:
                            _views[i].OnTryUpgrade -= _fUpgrade.ShipUpgrade;
                            _fUpgrade.EnoughGoldShipLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _fUpgrade.CanShipLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        default:
                            break;
                    }
                }
                break;
            case EMainUpgradeType.Dining:

                _dUpgrade.OnMasterLevelUpgrade -= RenewalMasterLevel;
                _dUpgrade.OnMaxCustomerLimitLevelUpgrade -= RenewalMaxCustomerLimitLevel;
                _dUpgrade.OnMaxSpawnLimit01LevelUpgrade -= RenewalMaxSpawnLimit01Level;
                _dUpgrade.OnMaxSpawnLimit02LevelUpgrade -= RenewalMaxSpawnLimit02Level;
                _dUpgrade.OnWeightLevelUpgrade -= RenewalWeightLevel;
                _dUpgrade.OnBonusTipsMultiLevelUpgrade -= RenewalBonusTipsMultiLevel;
                _dUpgrade.OnBonusDishPrice01LevelUpgrade -= RenewalBonusDishPrice01Level;
                _dUpgrade.OnBonusDishPrice02LevelUpgrade -= RenewalBonusDishPrice02Level;
                _dUpgrade.OnBonusFood01LevelUpgrade -= RenewalBonusFood01Level;
                _dUpgrade.OnBonusFood02LevelUpgrade -= RenewalBonusFood02Level;
                _dUpgrade.OnUnlockCatObjectLevelUpgrade -= RenewalUnlockCatObjectLevel;
                
                for (int i = 0; i < _views.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _views[i].OnTryUpgrade -= _dUpgrade.MasterLevelUpgrade;
                            _dUpgrade.EnoughGoldMasterLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            break;
                        case 1:
                            _views[i].OnTryUpgrade -= _dUpgrade.MaxCustomerLimitLevelUpgrade;
                            _dUpgrade.EnoughGoldMaxCustomerLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanMaxCustomerLimitLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 2:
                            _views[i].OnTryUpgrade -= _dUpgrade.MaxCustomerLimitLevelUpgrade;
                            _dUpgrade.EnoughGoldMaxCustomerLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanMaxCustomerLimitLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 3:
                            _views[i].OnTryUpgrade -= _dUpgrade.MaxSpawnLimit01LevelUpgrade;
                            _dUpgrade.EnoughGoldMaxSpawnLimit01LevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanMaxSpawnLimit01LevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 4:
                            _views[i].OnTryUpgrade -= _dUpgrade.MaxSpawnLimit02LevelUpgrade;
                            _dUpgrade.EnoughGoldMaxSpawnLimit02LevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanMaxSpawnLimit02LevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 5:
                            _views[i].OnTryUpgrade -= _dUpgrade.WeightLevelUpgrade;
                            _dUpgrade.EnoughGoldWeightLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanWeightLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 6:
                            _views[i].OnTryUpgrade -= _dUpgrade.BonusTipsMultiLevelUpgrade;
                            _dUpgrade.EnoughGoldBonusTipsMultiLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusTipsMultiLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 7:
                            _views[i].OnTryUpgrade -= _dUpgrade.BonusDishPrice01LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusDishPrice01LevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusDishPrice01LevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 8:
                            _views[i].OnTryUpgrade -= _dUpgrade.BonusDishPrice02LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusDishPrice02LevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusDishPrice02LevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 9:
                            _views[i].OnTryUpgrade -= _dUpgrade.BonusFood01LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusFood01LevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusFood01LevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 10:
                            _views[i].OnTryUpgrade -= _dUpgrade.BonusFood02LevelUpgrade;
                            _dUpgrade.EnoughGoldBonusFood02LevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanBonusFood02LevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 11:
                            _views[i].OnTryUpgrade -= _dUpgrade.UnlockCatObjectLevelUpgrade;
                            _dUpgrade.EnoughGoldUnlockCatObjectLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _dUpgrade.CanUnlockCatObjectLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator LoadingOnEnableRoutine()
    {
        while (DataTower.instance == null)
        {
            yield return _waitForEndOfFrame;
        }
        
        EnableEvents();
        RunSettings();
    }

    #endregion

    #region 낚시 부분

    // 낚시 등급
    private void SetViewPlayerGrade()
    {
        int level = DataTower.instance.fishingGrade;
        _fUpgrade.CheckEnoughGoldFishingGradeUpgrade(DataTower.instance.money);
        _fDataReader.GetFishingGradeReqGoldData(level, out int reqGold);
        _views[0].RenewalLevelText(level,_fDataReader.Grades.Length);
        _views[0].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalPlayerGrade(int level)
    {
        _fDataReader.GetFishingGradeReqGoldData(level, out int reqGold);
        _views[0].RenewalLevelText(level,_fDataReader.Grades.Length);
        _views[0].RenewalReqGoldText(reqGold);
    }

    // 미끼 레벨
    private void SetViewBaitLevel()
    {
        int level = DataTower.instance.baitLevel;
        
        _fUpgrade.CheckEnoughGoldBaitLevelUpgrade(DataTower.instance.money);
        _fUpgrade.CheckCanBaitLevelUpgrade(DataTower.instance.fishingGrade, DataTower.instance.baitLevel);
        _fDataReader.GetBaitLevelReqGoldData(level, out int reqGold);
        _views[1].RenewalLevelText(level,_fDataReader.Baits.Length);
        _views[1].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalBaitLevel(int level)
    {
        _fDataReader.GetBaitLevelReqGoldData(level, out int reqGold);
        _views[1].RenewalLevelText(level,_fDataReader.Baits.Length);
        _views[1].RenewalReqGoldText(reqGold);
    }
    
    // 낚시대 레벨
    private void SetViewRodLevel()
    {
        int level = DataTower.instance.rodLevel;
        
        _fUpgrade.CheckEnoughGoldRodLevelUpgrade(DataTower.instance.money);
        _fUpgrade.CheckCanRodLevelUpgrade(DataTower.instance.fishingGrade, DataTower.instance.rodLevel);
        _fDataReader.GetRodLevelReqGoldData(level, out int reqGold);
        _views[2].RenewalLevelText(level,_fDataReader.Rods.Length);
        _views[2].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalRodLevel(int level)
    {
        _fDataReader.GetRodLevelReqGoldData(level, out int reqGold);
        _views[2].RenewalLevelText(level,_fDataReader.Rods.Length);
        _views[2].RenewalReqGoldText(reqGold);
    }

    // 배 레벨
    private void SetViewShipLevel()
    {
        int level = DataTower.instance.shipLevel;
        
        _fUpgrade.CheckEnoughGoldShipLevelUpgrade(DataTower.instance.money);
        _fUpgrade.CheckCanShipLevelUpgrade(DataTower.instance.fishingGrade, DataTower.instance.shipLevel);
        _fDataReader.GetShipLevelReqGoldData(level, out int reqGold);
        _views[3].RenewalLevelText(level,_fDataReader.Ships.Length);
        _views[3].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalShipLevel(int level)
    {
        _fDataReader.GetShipLevelReqGoldData(level, out int reqGold);
        _views[3].RenewalLevelText(level,_fDataReader.Ships.Length);
        _views[3].RenewalReqGoldText(reqGold);
    }

    // 텍스트 세팅
    private void SetFishingTranslationTextSetting()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].UpgradeTargetData = _tDataReader.GetTranslationData(_tDataReader.GetFishingTranslationID((EFishingUpgradeType)i, 1));
            _views[i].UpgradeDescriptionData = _tDataReader.GetTranslationData(_tDataReader.GetFishingTranslationID((EFishingUpgradeType)i, 2));
            _views[i].UpgradeToolTipData = _tDataReader.GetTranslationData(_tDataReader.GetFishingTranslationID((EFishingUpgradeType)i, 3));
            _views[i].UpgradeToolTipMaxData = _tDataReader.GetTranslationData(_tDataReader.GetFishingTranslationID((EFishingUpgradeType)i, 4));
            _views[i].TranslationText(DataTower.instance.languageSetting);
        }
    }

    #endregion

    #region 식당 부분

    // 마스터 레벨
    private void SetViewMasterLevel()
    {
        int level = DataTower.instance.MasterLevel;
        _dUpgrade.CheckEnoughGoldMasterLevelUpgrade(DataTower.instance.money);
        _dDataReader.GetMasterLevelCostData(level, out int cost);
        _views[0].RenewalLevelText(level,_dDataReader.Master_Lv.Length);
        _views[0].RenewalReqGoldText(cost);
    }
    
    private void RenewalMasterLevel(int level)
    {
        _dDataReader.GetMasterLevelCostData(level, out int cost);
        _views[0].RenewalLevelText(level,_dDataReader.Master_Lv.Length);
        _views[0].RenewalReqGoldText(cost);
    }
    
    // 좌석 업그레이드 레벨
    private void SetViewMaxCustomerLimitLevel()
    {
        int level = DataTower.instance.MaxCustomerLimitLevel;
        _dUpgrade.CheckEnoughGoldMaxCustomerLimitLevelUpgrade(DataTower.instance.money);
        _dDataReader.GetMaxCustomerLimitCostData(level, out int cost);
        _views[1].RenewalLevelText(level,_dDataReader.Max_Customer_Limit.Length);
        _views[1].RenewalReqGoldText(cost);
    }
    
    private void RenewalMaxCustomerLimitLevel(int level)
    {
        _dDataReader.GetMaxCustomerLimitCostData(level, out int cost);
        _views[1].RenewalLevelText(level,_dDataReader.Max_Customer_Limit.Length);
        _views[1].RenewalReqGoldText(cost);
    }
    
    // 특별 손님 업그레이드 레벨
    private void SetViewMaxSpawnLimit01Level()
    {
        int level = DataTower.instance.MaxSpawnLimit01Level;
        _dUpgrade.CheckEnoughGoldMaxSpawnLimit01LevelUpgrade(DataTower.instance.money);
        _dDataReader.GetMaxSpawnLimit01CostData(level, out int cost);
        _views[2].RenewalLevelText(level,_dDataReader.Max_Spawn_Limit_1.Length);
        _views[2].RenewalReqGoldText(cost);
    }
    
    private void RenewalMaxSpawnLimit01Level(int level)
    {
        _dDataReader.GetMaxSpawnLimit01CostData(level, out int cost);
        _views[2].RenewalLevelText(level,_dDataReader.Max_Spawn_Limit_1.Length);
        _views[2].RenewalReqGoldText(cost);
    }
    
    // VIP 업그레이드 레벨
    private void SetViewMaxSpawnLimit02Level()
    {
        int level = DataTower.instance.MaxSpawnLimit02Level;
        _dUpgrade.CheckEnoughGoldMaxSpawnLimit02LevelUpgrade(DataTower.instance.money);
        _dDataReader.GetMaxSpawnLimit02CostData(level, out int cost);
        _views[3].RenewalLevelText(level,_dDataReader.Max_Spawn_Limit_2.Length);
        _views[3].RenewalReqGoldText(cost);
    }
    
    private void RenewalMaxSpawnLimit02Level(int level)
    {
        _dDataReader.GetMaxSpawnLimit02CostData(level, out int cost);
        _views[3].RenewalLevelText(level,_dDataReader.Max_Spawn_Limit_2.Length);
        _views[3].RenewalReqGoldText(cost);
    }
    
    // 팁주는 손님 가중치 업그레이드 레벨
    private void SetViewWeightLevel()
    {
        int level = DataTower.instance.WeightLevel;
        _dUpgrade.CheckEnoughGoldWeightLevelUpgrade(DataTower.instance.money);
        _dDataReader.GetWeightCostData(level, out int cost);
        _views[4].RenewalLevelText(level,_dDataReader.Weight.Length);
        _views[4].RenewalReqGoldText(cost);
    }
    
    private void RenewalWeightLevel(int level)
    {
        _dDataReader.GetWeightCostData(level, out int cost);
        _views[4].RenewalLevelText(level,_dDataReader.Weight.Length);
        _views[4].RenewalReqGoldText(cost);
    }
    
    // 모금함(팁 액수 증가) 업그레이드 레벨
    private void SetViewBonusTipsMultiLevel()
    {
        int level = DataTower.instance.BonusTipsMultiLevel;
        _dUpgrade.CheckEnoughGoldBonusTipsMultiLevelUpgrade(DataTower.instance.money);
        _dDataReader.GetBonusTipsMultiCostData(level, out int cost);
        _views[5].RenewalLevelText(level,_dDataReader.Bonus_Tips_Multi.Length);
        _views[5].RenewalReqGoldText(cost);
    }
    
    private void RenewalBonusTipsMultiLevel(int level)
    {
        _dDataReader.GetBonusTipsMultiCostData(level, out int cost);
        _views[5].RenewalLevelText(level,_dDataReader.Bonus_Tips_Multi.Length);
        _views[5].RenewalReqGoldText(cost);
    }

    // 계산대(요리 가격 증가) 업그레이드 레벨
    private void SetViewBonusDishPrice01Level()
    {
        int level = DataTower.instance.BonusDishPrice01Level;
        _dUpgrade.CheckEnoughGoldBonusDishPrice01LevelUpgrade(DataTower.instance.money);
        _dDataReader.GetBonusDishPrice01CostData(level, out int cost);
        _views[6].RenewalLevelText(level,_dDataReader.Bonus_Dish_Price_1.Length);
        _views[6].RenewalReqGoldText(cost);
    }
    
    private void RenewalBonusDishPrice01Level(int level)
    {
        _dDataReader.GetBonusDishPrice01CostData(level, out int cost);
        _views[6].RenewalLevelText(level,_dDataReader.Bonus_Dish_Price_1.Length);
        _views[6].RenewalReqGoldText(cost);
    }
    
    // 밥솥(요리 가격 증가) 업그레이드 레벨
    private void SetViewBonusDishPrice02Level()
    {
        int level = DataTower.instance.BonusDishPrice02Level;
        _dUpgrade.CheckEnoughGoldBonusDishPrice02LevelUpgrade(DataTower.instance.money);
        _dDataReader.GetBonusDishPrice02CostData(level, out int cost);
        _views[7].RenewalLevelText(level,_dDataReader.Bonus_Dish_Price_2.Length);
        _views[7].RenewalReqGoldText(cost);
    }
    
    private void RenewalBonusDishPrice02Level(int level)
    {
        _dDataReader.GetBonusDishPrice02CostData(level, out int cost);
        _views[7].RenewalLevelText(level,_dDataReader.Bonus_Dish_Price_2.Length);
        _views[7].RenewalReqGoldText(cost);
    }
    
    // 식칼(요리 개수 증가) 업그레이드 레벨
    private void SetViewBonusFood01Level()
    {
        int level = DataTower.instance.BonusFood01Level;
        _dUpgrade.CheckEnoughGoldBonusFood01LevelUpgrade(DataTower.instance.money);
        _dDataReader.GetBonusFood01CostData(level, out int cost);
        _views[8].RenewalLevelText(level,_dDataReader.Bonus_Food_1.Length);
        _views[8].RenewalReqGoldText(cost);
    }
    
    private void RenewalBonusFood01Level(int level)
    {
        _dDataReader.GetBonusFood01CostData(level, out int cost);
        _views[8].RenewalLevelText(level,_dDataReader.Bonus_Food_1.Length);
        _views[8].RenewalReqGoldText(cost);
    }
    
    // 도마(요리 개수 증가) 업그레이드 레벨
    private void SetViewBonusFood02Level()
    {
        int level = DataTower.instance.BonusFood02Level;
        _dUpgrade.CheckEnoughGoldBonusFood02LevelUpgrade(DataTower.instance.money);
        _dDataReader.GetBonusFood02CostData(level, out int cost);
        _views[9].RenewalLevelText(level,_dDataReader.Bonus_Food_2.Length);
        _views[9].RenewalReqGoldText(cost);
    }
    
    private void RenewalBonusFood02Level(int level)
    {
        _dDataReader.GetBonusFood02CostData(level, out int cost);
        _views[9].RenewalLevelText(level,_dDataReader.Bonus_Food_2.Length);
        _views[9].RenewalReqGoldText(cost);
    }
    
    // 고양이 업그레이드 레벨
    private void SetViewUnlockCatObjectLevel()
    {
        int level = DataTower.instance.UnlockCatObjectLevel;
        _dUpgrade.CheckEnoughGoldUnlockCatObjectLevelUpgrade(DataTower.instance.money);
        _dDataReader.GetUnlockCatObjectCostData(level, out int cost);
        _views[10].RenewalLevelText(level,_dDataReader.Unlock_Cat_Object.Length);
        _views[10].RenewalReqGoldText(cost);
    }
    
    private void RenewalUnlockCatObjectLevel(int level)
    {
        _dDataReader.GetUnlockCatObjectCostData(level, out int cost);
        _views[10].RenewalLevelText(level,_dDataReader.Unlock_Cat_Object.Length);
        _views[10].RenewalReqGoldText(cost);
    }
    
    // 텍스트 세팅
    private void SetDiningTranslationTextSetting()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].UpgradeTargetData = _tDataReader.GetTranslationData(_tDataReader.GetDiningTranslationID((EDiningUpgradeType)i, 1));
            _views[i].UpgradeDescriptionData = _tDataReader.GetTranslationData(_tDataReader.GetDiningTranslationID((EDiningUpgradeType)i, 2));
            _views[i].UpgradeToolTipData = _tDataReader.GetTranslationData(_tDataReader.GetDiningTranslationID((EDiningUpgradeType)i, 3));
            // _views[i].UpgradeToolTipMaxData = _tDataReader.GetTranslationData(_tDataReader.GetDiningTranslationID((EDiningUpgradeType)i, 4));
            _views[i].TranslationText(DataTower.instance.languageSetting);
        }
    }

    #endregion
    
}

