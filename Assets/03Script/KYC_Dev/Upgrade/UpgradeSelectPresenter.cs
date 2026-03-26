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
    
    private FishingUpgradeManager _fishingUpgradeManager;
    private FishingUpgradeDataReader _fishingDataReader;
    
    //ToDo:Dining업그레이드 완료되면 하단 수정
    private FishingUpgradeManager _diningUpgradeManager;
    private FishingUpgradeDataReader _diningDataReader;
    
    private TranslationDataReader _translationDataReader;

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
        _fishingUpgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
        _fishingDataReader = FindFirstObjectByType<FishingUpgradeDataReader>();
        //_diningUpgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
        //_diningDataReader = FindFirstObjectByType<FishingUpgradeDataReader>();
        _translationDataReader = FindFirstObjectByType<TranslationDataReader>();
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
                break;
            case EMainUpgradeType.Dining:
                
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
                _fishingUpgradeManager.OnFishingUpgrade += RenewalPlayerGrade;
                _fishingUpgradeManager.OnBaitUpgrade += RenewalBaitLevel;
                _fishingUpgradeManager.OnRodUpgrade += RenewalRodLevel;
                _fishingUpgradeManager.OnShipUpgrade += RenewalShipLevel;
                for (int i = 0; i < _views.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _views[i].OnTryUpgrade += _fishingUpgradeManager.FishingUpgrade;
                            _fishingUpgradeManager.EnoughGoldFishingGradeUpgrade += _views[i].ToggleReqGoldTextColor;
                            //_views[i].UpgradeTargetLanguage = _targetlanguage;
                            //_views[i].UpgradeDescriptionLanguage = _discriptionLanguage;
                            break;
                        case 1:
                            _views[i].OnTryUpgrade += _fishingUpgradeManager.BaitUpgrade;
                            _fishingUpgradeManager.EnoughGoldBaitLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _fishingUpgradeManager.CanBaitLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 2:
                            _views[i].OnTryUpgrade += _fishingUpgradeManager.RodUpgrade;
                            _fishingUpgradeManager.EnoughGoldRodLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _fishingUpgradeManager.CanRodLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        case 3:
                            _views[i].OnTryUpgrade += _fishingUpgradeManager.ShipUpgrade;
                            _fishingUpgradeManager.EnoughGoldShipLevelUpgrade += _views[i].ToggleReqGoldTextColor;
                            _fishingUpgradeManager.CanShipLevelUpgrade += _views[i].ToggleButtenState;
                            break;
                        default:
                            break;
                    }
                }
                break;
            case EMainUpgradeType.Dining:
                
                for (int i = 0; i < _views.Length; i++)
                {
                    
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
                _fishingUpgradeManager.OnFishingUpgrade -= RenewalPlayerGrade;
                _fishingUpgradeManager.OnBaitUpgrade -= RenewalBaitLevel;
                _fishingUpgradeManager.OnRodUpgrade -= RenewalRodLevel;
                _fishingUpgradeManager.OnShipUpgrade -= RenewalShipLevel;
                for (int i = 0; i < _views.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _views[i].OnTryUpgrade -= _fishingUpgradeManager.FishingUpgrade;
                            _fishingUpgradeManager.EnoughGoldFishingGradeUpgrade -= _views[i].ToggleReqGoldTextColor;
                            break;
                        case 1:
                            _views[i].OnTryUpgrade -= _fishingUpgradeManager.BaitUpgrade;
                            _fishingUpgradeManager.EnoughGoldBaitLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _fishingUpgradeManager.CanBaitLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 2:
                            _views[i].OnTryUpgrade -= _fishingUpgradeManager.RodUpgrade;
                            _fishingUpgradeManager.EnoughGoldRodLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _fishingUpgradeManager.CanRodLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        case 3:
                            _views[i].OnTryUpgrade -= _fishingUpgradeManager.ShipUpgrade;
                            _fishingUpgradeManager.EnoughGoldShipLevelUpgrade -= _views[i].ToggleReqGoldTextColor;
                            _fishingUpgradeManager.CanShipLevelUpgrade -= _views[i].ToggleButtenState;
                            break;
                        default:
                            break;
                    }
                }
                break;
            case EMainUpgradeType.Dining:
                
                break;
            default:
                break;
        }
    }

    private IEnumerator LoadingOnEnableRoutine()
    {
        while (DataTower.instance == null)
        {
            yield return _upgradeUIView.LoadingDelayWait;
        }
        
        RunSettings();
        EnableEvents();
    }

    #endregion

    #region 낚시 부분

    private void SetViewPlayerGrade()
    {
        int level = _fishingUpgradeManager.FishingGrade;
        int reqGold;
        _translationDataReader.GetFTranslationData("UPG_Player", out _views[0].UpgradeTargetData);
        _translationDataReader.GetFTranslationData("UPG_Player_Desc", out _views[0].UpgradeDescriptionData);
        _translationDataReader.GetFTranslationData("UPG_Player_ToolTip", out _views[0].UpgradeToolTipData);
        _translationDataReader.GetFTranslationData("UPG_Player_ToolTip_Max", out _views[0].UpgradeToolTipMaxData);
        _fishingUpgradeManager.ChackEnoughGoldFishingGradeUpgrade(DataTower.instance.money);
        _fishingDataReader.GetFishingGradeReqGoldData(level, out reqGold);
        _fishingUpgradeManager.CheakCanBaitLevelUpgrade(_fishingUpgradeManager.FishingGrade, _fishingUpgradeManager.BaitLevel);
        _views[0].TranslationText(DataTower.instance.languageSetting);
        _views[0].RenewalLevelText(level,_fishingDataReader.Grades.Length);
        _views[0].RenewalReqGoldText(reqGold);
    }

    private void SetViewBaitLevel()
    {
        int level = _fishingUpgradeManager.BaitLevel;
        int reqGold;
        _translationDataReader.GetFTranslationData("UPG_Bait", out _views[1].UpgradeTargetData);
        _translationDataReader.GetFTranslationData("UPG_Bait_Desc", out _views[1].UpgradeDescriptionData);
        _translationDataReader.GetFTranslationData("UPG_Bait_ToolTip", out _views[1].UpgradeToolTipData);
        _translationDataReader.GetFTranslationData("UPG_Bait_ToolTip_Max", out _views[1].UpgradeToolTipMaxData);
        _fishingUpgradeManager.ChackEnoughGoldBaitLevelUpgrade(DataTower.instance.money);
        _fishingDataReader.GetBaitLevelReqGoldData(level, out reqGold);
        _views[1].TranslationText(DataTower.instance.languageSetting);
        _views[1].RenewalLevelText(level,_fishingDataReader.Baits.Length);
        _views[1].RenewalReqGoldText(reqGold);
    }

    private void SetViewRodLevel()
    {
        int level = _fishingUpgradeManager.RodLevel;
        int reqGold;
        _translationDataReader.GetFTranslationData("UPG_Fishing", out _views[2].UpgradeTargetData);
        _translationDataReader.GetFTranslationData("UPG_Fishing_Desc", out _views[2].UpgradeDescriptionData);
        _translationDataReader.GetFTranslationData("UPG_FishingRod_ToolTip", out _views[2].UpgradeToolTipData);
        _translationDataReader.GetFTranslationData("UPG_FishingRod_ToolTip_Max", out _views[2].UpgradeToolTipMaxData);
        _fishingUpgradeManager.ChackEnoughGoldRodLevelUpgrade(DataTower.instance.money);
        _fishingUpgradeManager.CheakCanRodLevelUpgrade(_fishingUpgradeManager.FishingGrade, _fishingUpgradeManager.RodLevel);
        _fishingDataReader.GetRodLevelReqGoldData(level, out reqGold);
        _views[2].TranslationText(DataTower.instance.languageSetting);
        _views[2].RenewalLevelText(level,_fishingDataReader.Rods.Length);
        _views[2].RenewalReqGoldText(reqGold);
    }

    private void SetViewShipLevel()
    {
        int level = _fishingUpgradeManager.ShipLevel;
        int reqGold;
        _translationDataReader.GetFTranslationData("UPG_Ship", out _views[3].UpgradeTargetData);
        _translationDataReader.GetFTranslationData("UPG_Ship_Desc", out _views[3].UpgradeDescriptionData);
        _translationDataReader.GetFTranslationData("UPG_Ship_ToolTip", out _views[3].UpgradeToolTipData);
        _translationDataReader.GetFTranslationData("UPG_Ship_ToolTip_Max", out _views[3].UpgradeToolTipMaxData);
        _fishingUpgradeManager.ChackEnoughGoldShipLevelUpgrade(DataTower.instance.money);
        _fishingUpgradeManager.CheakCanShipLevelUpgrade(_fishingUpgradeManager.FishingGrade, _fishingUpgradeManager.ShipLevel);
        _fishingDataReader.GetShipLevelReqGoldData(level, out reqGold);
        _views[3].TranslationText(DataTower.instance.languageSetting);
        _views[3].RenewalLevelText(level,_fishingDataReader.Ships.Length);
        _views[3].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalPlayerGrade(int level)
    {
        int reqGold;
        _fishingDataReader.GetFishingGradeReqGoldData(level, out reqGold);
        _views[0].RenewalLevelText(level,_fishingDataReader.Grades.Length);
        _views[0].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalBaitLevel(int level)
    {
        int reqGold;
        _fishingDataReader.GetBaitLevelReqGoldData(level, out reqGold);
        _views[1].RenewalLevelText(level,_fishingDataReader.Baits.Length);
        _views[1].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalRodLevel(int level)
    {
        int reqGold;
        _fishingDataReader.GetRodLevelReqGoldData(level, out reqGold);
        _views[2].RenewalLevelText(level,_fishingDataReader.Rods.Length);
        _views[2].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalShipLevel(int level)
    {
        int reqGold;
        _fishingDataReader.GetShipLevelReqGoldData(level, out reqGold);
        _views[3].RenewalLevelText(level,_fishingDataReader.Ships.Length);
        _views[3].RenewalReqGoldText(reqGold);
    }

    #endregion

    #region 식당 부분

    

    #endregion
    
}

