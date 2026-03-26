using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeSelectPresenter : MonoBehaviour
{
    [Header("Children Upgrade Select Views")]
    [SerializeField] UpgradeSelectView[] _views;
    [SerializeField] Image[] _upgradeSprites;
    
    [Header("Upgrade Type")]
    [field: SerializeField]
    public EMainUpgradeType MainUpgradeType { get; private set; }
    
    [Header("TranslationData / 차후 수정")]
    //ToDo:ScriptableObject형식을 나중에 번역SO형식으로 교체해야됨
    [SerializeField] ScriptableObject _targetlanguage;
    [SerializeField] ScriptableObject _discriptionLanguage;
    
    private FishingUpgradeManager _fishingUpgradeManager;
    private FishingUpgradeDataReader _fishingDataReader;
    
    //ToDo:Dining업그레이드 완료되면 하단 수정
    private FishingUpgradeManager _diningUpgradeManager;
    private FishingUpgradeDataReader _diningDataReader;
    
    

    private void Awake()
    {
        _fishingUpgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
        _fishingDataReader = FindFirstObjectByType<FishingUpgradeDataReader>();
        //_diningUpgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
        //_diningDataReader = FindFirstObjectByType<FishingUpgradeDataReader>();
    }

    private void OnEnable()
    {
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

    private void OnDisable()
    {
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

    private void Start()
    {
        SetViewPlayerGrade();
        SetViewBaitLevel();
        SetViewRodLevel();
        SetViewShipLevel();
    }

    #region 낚시 부분

    private void SetViewPlayerGrade()
    {
        int level = _fishingUpgradeManager.FishingGrade;
        int reqGold;
        _fishingUpgradeManager.ChackEnoughGoldFishingGradeUpgrade(DataTower.instance.money);
        _fishingDataReader.GetFishingGradeReqGoldData(level, out reqGold);
        _fishingUpgradeManager.CheakCanBaitLevelUpgrade(_fishingUpgradeManager.FishingGrade, _fishingUpgradeManager.BaitLevel);
        _views[0].TranslationText(DataTower.instance.languageSetting);
        _views[0].RenewalLevelText(level,_fishingDataReader.Grades.Length);
        _views[0].RenewalReqGoldText(reqGold);
    }

    private void SetViewBaitLevel()
    {
        int level = _fishingUpgradeManager.FishingGrade;
        int reqGold;
        _fishingUpgradeManager.ChackEnoughGoldBaitLevelUpgrade(DataTower.instance.money);
        _fishingDataReader.GetBaitLevelReqGoldData(level, out reqGold);
        _views[1].TranslationText(DataTower.instance.languageSetting);
        _views[1].RenewalLevelText(level,_fishingDataReader.Baits.Length);
        _views[1].RenewalReqGoldText(reqGold);
    }

    private void SetViewRodLevel()
    {
        int level = _fishingUpgradeManager.FishingGrade;
        int reqGold;
        _fishingUpgradeManager.ChackEnoughGoldRodLevelUpgrade(DataTower.instance.money);
        _fishingUpgradeManager.CheakCanRodLevelUpgrade(_fishingUpgradeManager.FishingGrade, _fishingUpgradeManager.RodLevel);
        _fishingDataReader.GetRodLevelReqGoldData(level, out reqGold);
        _views[2].TranslationText(DataTower.instance.languageSetting);
        _views[2].RenewalLevelText(level,_fishingDataReader.Rods.Length);
        _views[2].RenewalReqGoldText(reqGold);
    }

    private void SetViewShipLevel()
    {
        int level = _fishingUpgradeManager.FishingGrade;
        int reqGold;
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
        _views[0].TranslationText(DataTower.instance.languageSetting);
        _views[0].RenewalLevelText(level,_fishingDataReader.Grades.Length);
        _views[0].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalBaitLevel(int level)
    {
        int reqGold;
        _fishingDataReader.GetBaitLevelReqGoldData(level, out reqGold);
        _views[1].TranslationText(DataTower.instance.languageSetting);
        _views[1].RenewalLevelText(level,_fishingDataReader.Grades.Length);
        _views[1].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalRodLevel(int level)
    {
        int reqGold;
        _fishingDataReader.GetRodLevelReqGoldData(level, out reqGold);
        _views[2].TranslationText(DataTower.instance.languageSetting);
        _views[2].RenewalLevelText(level,_fishingDataReader.Grades.Length);
        _views[2].RenewalReqGoldText(reqGold);
    }
    
    private void RenewalShipLevel(int level)
    {
        int reqGold;
        _fishingDataReader.GetShipLevelReqGoldData(level, out reqGold);
        _views[3].TranslationText(DataTower.instance.languageSetting);
        _views[3].RenewalLevelText(level,_fishingDataReader.Grades.Length);
        _views[3].RenewalReqGoldText(reqGold);
    }

    #endregion

    #region 식당 부분

    

    #endregion
    
}

