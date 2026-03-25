using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeSelectPresenter : MonoBehaviour
{
    [Header("Children Upgrade Select Views")]
    [SerializeField] UpgradeSelectView[] _views;
    
    [Header("Upgrade Type")]
    [field: SerializeField]
    public EMainUpgradeType MainUpgradeType { get; private set; }

    [Header("Language")]
    public ETestLanguage Language;
    
    //ToDo:DataTower에 업그레이드 변수 이관 되면 주소 수정
    private FishingUpgradeManager _fishingUpgradeManager;
    private FishingUpgradeDataReader _fishingDataReader;
    
    //ToDo:DiningDataReader 완성 되면 주소 수정
    private FishingUpgradeDataReader _diningDataReader;

    private void Awake()
    {
        _fishingUpgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
        _fishingDataReader = FindFirstObjectByType<FishingUpgradeDataReader>();
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
                break;
            case EMainUpgradeType.Dining:
                
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
                break;
            case EMainUpgradeType.Dining:
                
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        SetViews();
    }

    private void SetViews()
    {
        switch (MainUpgradeType)
        {
            case EMainUpgradeType.Fishing:
                for (int i = 0; i < _views.Length; i++)
                {
                    _views[i].FishingUpgradeType = (EFishingUpgradeType)i;
                }
                break;
            case EMainUpgradeType.Dining:
                for (int i = 0; i < _views.Length; i++)
                {
                    _views[i].DiningUpgradeType = (EDiningUpgradeType)i;
                }
                break;
        }
    }

    private void RenewalPlayerGrade()
    {
        
    }
    
    private void RenewalBaitLevel()
    {
        
    }
    
    private void RenewalRodLevel()
    {
        
    }
    
    private void RenewalShipLevel()
    {
        
    }
    
    
    
    
    
    /// <summary>
    /// View에서 업그레이드 버튼 누를 때 전달 해줄 명령어
    /// </summary>
    public void UpgradeSelectAndRun()
    {
        switch (MainUpgradeType)
        {
            case EMainUpgradeType.Fishing:
                
                break;
            case EMainUpgradeType.Dining:
                break;
            default:
                break;
        }
    }
}

