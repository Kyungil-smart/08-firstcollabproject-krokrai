using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeSelectView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI _upgradeTargetText;
    [SerializeField] TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] TextMeshProUGUI _upgradeLevelText;
    [SerializeField] TextMeshProUGUI _upgradeReqGoldText;
    [SerializeField] TextMeshProUGUI _toolTipText;
    [SerializeField] Image _upgradeIconSprite;
    [SerializeField] Button _upgradeButton;
    [SerializeField] Slider _slider;
    
    [Header("TranslationData / AutoSetting / For Debug")]
    public TranslationData UpgradeTargetData;
    public TranslationData UpgradeDescriptionData;
    public TranslationData UpgradeToolTipData;
    public TranslationData UpgradeToolTipMaxData;

    private string _ttindex0;
    private string _ttindex1;
    private string _ttindex2;
    private string _ttindex3;
    private string _ttindex4;
    
    private string _ttmindex0;
    private string _ttmindex1;
    private string _ttmindex2;
    
    public event Action OnTryUpgrade;

    /// <summary>
    /// UI 텍스트 번역 설정
    /// </summary>
    /// <param name="language">영어 / 한국어</param>
    public void TranslationText(Language language)
    {
        SetTooltipText(language);
        SetTooltipMaxText(language);
        switch (language)
        {
            case Language.ENG:
                _upgradeTargetText.text = UpgradeTargetData.En;
                _upgradeDescriptionText.text = UpgradeDescriptionData.En;
                break;
            case Language.KOR:
                _upgradeTargetText.text = UpgradeTargetData.Kor;
                _upgradeDescriptionText.text = UpgradeDescriptionData.Kor;
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
        _slider.value = currentLevel / (float)maxLevel; 
    }

    /// <summary>
    /// UI의 다음 업그레이드에 필요한 골드 출력 명령
    /// </summary>
    /// <param name="requiredGold"></param>
    public void RenewalReqGoldText(int requiredGold)
    {
        _upgradeReqGoldText.text = requiredGold.TextFormatComma();
    }

    /// <summary>
    /// UI 툴팁 설명 출력
    /// </summary>
    /// <param name="curLevel">현재 레벨</param>
    /// <param name="curEffect">현재 효과</param>
    /// <param name="nextEffect">다음 효과</param>
    public void RenewalTooltipText(int curLevel, string curEffect, string nextEffect)
    {
        _toolTipText.text =
            $"{_ttindex0}{curLevel}{_ttindex1}{curEffect}{_ttindex2}{curLevel+1}{_ttindex3}{nextEffect}{_ttindex4}";
    }

    /// <summary>
    /// 낚시 툴팁 출력
    /// </summary>
    /// <param name="curLevel"></param>
    public void RenewalTooltipTextForFishingGrade(int curLevel)
    {
        _toolTipText.text = $"{_ttindex0}{curLevel}{_ttindex1}{curLevel+1}{_ttindex2}";
    }

    /// <summary>
    /// UI 툴팁 최대레벨 설명 출력
    /// </summary>
    /// <param name="curLevel">현재 레벨</param>
    /// <param name="curEffect">현재 효과</param>
    public void RenewalToolTipMaxText(int curLevel, string curEffect)
    {
        _toolTipText.text = $"{_ttmindex0}{curLevel}{_ttmindex1}{curEffect}{_ttmindex2}";
    }

    /// <summary>
    /// 낚시 최대레벨 툴팁 출력
    /// </summary>
    /// <param name="curLevel"></param>
    public void RenewalToolTipMaxTextForFishingGrade(int curLevel)
    {
        _toolTipText.text = $"{_ttmindex0}{curLevel}{_ttmindex1}";
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

    private void SetTooltipText(Language language)
    {
        if (UpgradeToolTipData.Id == "UPG_Player_ToolTip")
        {
            switch (language)
            {
                case Language.ENG:
                    string[] temp00 = UpgradeToolTipData.En.Split("{0}");
                    _ttindex0 = temp00[0];
                    string[] temp01 = temp00[1].Split("{2}");
                    _ttindex1 = temp01[0];
                    _ttindex2 = temp01[1];
                    break;
                case Language.KOR:
                    string[] temp10 = UpgradeToolTipData.Kor.Split("{0}");
                    _ttindex0 = temp10[0];
                    string[] temp11 = temp10[1].Split("{2}");
                    _ttindex1 = temp11[0];
                    _ttindex2 = temp11[1];
                    break;
            }
        }
        else
        {
            switch (language)
            {
                case Language.ENG:
                    string[] temp00 = UpgradeToolTipData.En.Split("{0}");
                    _ttindex0 = temp00[0];
                    string[] temp01 = temp00[1].Split("{1}");
                    _ttindex1 = temp01[0];
                    string[] temp02 = temp01[1].Split("{2}");
                    _ttindex2 = temp02[0];
                    string[] temp03 = temp02[1].Split("{3}");
                    _ttindex3 = temp03[0];
                    _ttindex4 = temp03[1];
                    break;
                case Language.KOR:
                    string[] temp10 = UpgradeToolTipData.Kor.Split("{0}");
                    _ttindex0 = temp10[0];
                    string[] temp11 = temp10[1].Split("{1}");
                    _ttindex1 = temp11[0];
                    string[] temp12 = temp11[1].Split("{2}");
                    _ttindex2 = temp12[0];
                    string[] temp13 = temp12[1].Split("{3}");
                    _ttindex3 = temp13[0];
                    _ttindex4 = temp13[1];
                    break;
            }
        }
        
    }
    
    private void SetTooltipMaxText(Language language)
    {
        if(UpgradeToolTipMaxData == null) return;
        if (UpgradeToolTipMaxData.Id == "UPG_Player_ToolTip_Max")
        {
            switch (language)
            {
                case Language.ENG:
                    string[] temp00 = UpgradeToolTipMaxData.En.Split("{0}");
                    _ttmindex0 = temp00[0];
                    _ttmindex1 = temp00[1];
                    break;
                case Language.KOR:
                    string[] temp10 = UpgradeToolTipMaxData.Kor.Split("{0}");
                    _ttmindex0 = temp10[0];
                    _ttmindex1 = temp10[1];
                    break;
            }
        }
        else
        {
            switch (language)
            {
                case Language.ENG:
                    string[] temp00 = UpgradeToolTipMaxData.En.Split("{0}");
                    _ttmindex0 = temp00[0];
                    string[] temp01 = temp00[1].Split("{1}");
                    _ttmindex1 = temp01[0];
                    _ttmindex2 = temp01[1];
                    break;
                case Language.KOR:
                    string[] temp10 = UpgradeToolTipMaxData.Kor.Split("{0}");
                    _ttmindex0 = temp10[0];
                    string[] temp11 = temp10[1].Split("{1}");
                    _ttmindex1 = temp11[0];
                    _ttmindex2 = temp11[1];
                    break;
            }
        }
    }
}
