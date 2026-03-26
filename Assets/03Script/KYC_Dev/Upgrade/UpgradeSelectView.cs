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
    
    [Header("TranslationData / Auto Setting / 차후 수정")]
    //ToDo:ScriptableObject형식을 나중에 번역SO형식으로 교체해야됨
    public ScriptableObject UpgradeTargetLanguage;
    public ScriptableObject UpgradeDescriptionLanguage;
    
    public event Action OnTryUpgrade;

    /// <summary>
    /// UI 텍스트 번역 설정
    /// ToDo:번역SO 추가되면 언어 연결 작업 필수
    /// </summary>
    /// <param name="language">영어 / 한국어</param>
    public void TranslationText(Language language)
    {
        switch (language)
        {
            case Language.ENG:
                _upgradeTargetText.text = "";
                _upgradeDescriptionText.text = "";
                break;
            case Language.KOR:
                _upgradeTargetText.text = "";
                _upgradeDescriptionText.text = "";
                break;
        }
    }
    
    /// <summary>
    /// UI의 업그레이드 레벨 현황 출력 명령
    /// </summary>
    /// <param name="currentLevel">현재 레벨</param>
    /// <param name="maxLevel">최대 레벨</param>
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

    /// <summary>
    /// UI의 다음 업그레이드에 필요한 골드 출력 명령
    /// </summary>
    /// <param name="requiredGold"></param>
    public void RenewalReqGoldText(int requiredGold)
    {
        _upgradeReqGoldText.text = requiredGold.ToString();
    }

    /// <summary>
    /// UI의 아이콘 스프라이트 출력 명령
    /// ToDo:스프라이트 설정 넘어오면 작성
    /// </summary>
    public void SetUpgradeIconSprite(Image sprite)
    {
        _upgradeIconSprite = sprite;
    }

    /// <summary>
    /// 업그레이드 버튼의 활성화 상태 조절
    /// </summary>
    /// <param name="state">활성화 / 비활성화</param>
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

    /// <summary>
    /// 업그레이드 버튼의 텍스트 색 수정
    /// ToDo:에셋 적용 후 배경색에 따라 수정 필요
    /// </summary>
    /// <param name="state">normal / red</param>
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
        OnTryUpgrade?.Invoke();
    }
}
