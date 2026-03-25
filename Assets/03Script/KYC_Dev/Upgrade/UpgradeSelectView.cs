using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeSelectView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI _upgradeTargetText;
    [SerializeField] TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] TextMeshProUGUI _upgradeLevelText;
    [SerializeField] TextMeshProUGUI _upgradeReqGoldText;
    [SerializeField] Image _upgradeIconSprite;
    [SerializeField] Button _upgradeButton;
    
    [Header("Localization Data")]
    [SerializeField] ScriptableObject _upgradeTargetLanguage;
    [SerializeField] ScriptableObject _upgradeDescriptionLanguage;
    
    [Header("Don`t Touch")]
    public EFishingUpgradeType FishingUpgradeType;
    public EDiningUpgradeType DiningUpgradeType;

    private UpgradeSelectPresenter _presenter;

    private void Awake()
    {
        _presenter = GetComponentInParent<UpgradeSelectPresenter>();
    }

    private void OnEnable()
    {
        // 이벤트 구독
    }

    private void OnDisable()
    {
        // 구독 해제
    }
    
    /// <summary>
    /// 번역
    /// ToDo:DataTower 작업 끝나면 변수 변경할것
    /// ToDo:번역SO 추가되면 언어 연결 작업 필수
    /// </summary>
    public void TranslationText()
    {
        switch (_presenter.Language)
        {
            case ETestLanguage.EN:
                _upgradeTargetText.text = "";
                _upgradeDescriptionText.text = "";
                break;
            case ETestLanguage.KOR:
                _upgradeTargetText.text = "";
                _upgradeDescriptionText.text = "";
                break;
            default:
                break;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentLevel"></param>
    /// <param name="maxLevel"></param>
    public void RenewalLevelText(int currentLevel, int maxLevel)
    {
        if (currentLevel != maxLevel)
        {
            _upgradeLevelText.text = $"{currentLevel}/{maxLevel}";
        }
        else
        {
            _upgradeLevelText.text = $"MAX({currentLevel}/{maxLevel})";
        }
    }

    public void RenewalReqGoldText(int requiredGold)
    {
        _upgradeReqGoldText.text = requiredGold.ToString();
    }

    public void SetUpgradeIconSprite()
    {
        
    }

    public void ToggleButtenState(bool state)
    {
        switch (state)
        {
            case true:
                _upgradeButton.interactable = true;
                break;
            case false:
                _upgradeButton.interactable = false;
                break;
        }
    }

    public void ToggleReqGoldTextColor(bool state)
    {
        switch (state)
        {
            case true:
                _upgradeReqGoldText.color = Color.black;
                break;
            case false:
                _upgradeReqGoldText.color = Color.red;
                break;
        }
    }

    /// <summary>
    /// 업그레이드 버튼 누를 때 행동
    /// </summary>
    public void OnClickUpgradeButton()
    {
        _presenter.UpgradeSelectAndRun();
    }
}
