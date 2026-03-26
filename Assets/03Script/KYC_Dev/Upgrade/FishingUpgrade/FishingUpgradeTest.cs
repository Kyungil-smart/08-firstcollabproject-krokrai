using System;
using TMPro;
using UnityEngine;

public class FishingUpgradeTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _fishingGradeText;
    [SerializeField] private TextMeshProUGUI _baitLevelText;
    [SerializeField] private TextMeshProUGUI _rodLevelText;
    [SerializeField] private TextMeshProUGUI _shipLevelText;

    DataTower _dataTower;
    FishingUpgradeManager _upgradeManager;
    
    private void Awake()
    {
        _dataTower = FindFirstObjectByType<DataTower>();
        _upgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
    }

    private void Update()
    {
        _goldText.text = _dataTower.money.ToString();
        _fishingGradeText.text = _upgradeManager.FishingGrade.ToString();
        _baitLevelText.text = _upgradeManager.BaitLevel.ToString();
        _rodLevelText.text = _upgradeManager.RodLevel.ToString();
        _shipLevelText.text = _upgradeManager.ShipLevel.ToString();
    }

    public void OnGoldPlusClick()
    {
        
    }
    
    public void OnFishingGradePlusClick()
    {
        _upgradeManager.FishingUpgrade();
    }

    public void OnBaitLevelPlusClick()
    {
        _upgradeManager.BaitUpgrade();
    }
    
    public void OnRodLevelPlusClick()
    {
        _upgradeManager.RodUpgrade();
    }
    
    public void OnShipLevelPlusClick()
    {
        _upgradeManager.ShipUpgrade();
    }
}
