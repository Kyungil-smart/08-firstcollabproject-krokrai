using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeUIView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] TextMeshProUGUI _toggleFishingUpgradeText;
    [SerializeField] TextMeshProUGUI _toggleDiningUpgradeText;
    [SerializeField] GameObject _fishingUpgradePanel;
    [SerializeField] GameObject _diningUpgradePanel;
    [SerializeField] Button _toggleFishingUpgradeButton;
    [SerializeField] Button _toggleDiningUpgradeButton;
    
    [Header("TranslationData / AutoSetting / For Debug")]
    [SerializeField] TranslationData _id_Money;
    [SerializeField] TranslationData _id_Fishing;
    [SerializeField] TranslationData _id_Restaurant;
    
    private TranslationDataReader _tDataReader;
    private WaitForEndOfFrame _waitForEndOfFrame = new();

    private string _moneyFrontText;
    private string _moneyBackText;
    
    private AudioManager _audioManager;

    private void Awake()
    {
        _tDataReader = FindFirstObjectByType<TranslationDataReader>();
        _audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnEnable()
    {
        StartCoroutine(LoadingOnEnableRoutine());
    }

    private void OnDisable()
    {
        EventDisable();
    }

    private void Start()
    {
        OnClickToggleFishingUpgrade();
    }

    #region 이벤트 등록/해제

    public void EventEnable()
    {
        DataTower.instance.OnChangedMoney += RenewalGoldText;
        DataTower.instance.OnLanguageSettingChanged += TranslationText;
    }

    public void EventDisable()
    {
        DataTower.instance.OnChangedMoney -= RenewalGoldText;
        DataTower.instance.OnLanguageSettingChanged -= TranslationText;
    }
    
    private IEnumerator LoadingOnEnableRoutine()
    {
        while (DataTower.instance == null)
        {
            yield return _waitForEndOfFrame;
        }
        
        EventEnable();
        SetGoldText();
        SetTranslationText();
    }

    #endregion

    #region 번역 관련

    private void SetTranslationText()
    {
        _id_Money = _tDataReader.GetTranslationData("Money");
        _id_Fishing = _tDataReader.GetTranslationData("Fishing");
        _id_Restaurant = _tDataReader.GetTranslationData("Restaurant");
        TranslationText(DataTower.instance.languageSetting);
    }
    
    private void TranslationText(Language language)
    {
        switch (language)
        {
            case Language.ENG:
                _toggleFishingUpgradeText.text = _id_Fishing.En;
                _toggleDiningUpgradeText.text = _id_Restaurant.En;
                SplitMoneyText(language, out _moneyFrontText, out _moneyBackText);
                SetGoldText();
                break;
            case Language.KOR:
                _toggleFishingUpgradeText.text = _id_Fishing.Kor;
                _toggleDiningUpgradeText.text = _id_Restaurant.Kor;
                SplitMoneyText(language, out _moneyFrontText, out _moneyBackText);
                SetGoldText();
                break;
        }
    }

    private void SplitMoneyText(Language language, out string front, out string back)
    {
        switch (language)
        {
            case Language.ENG:
                string[] temp = _id_Money.En.Split("{0:N0}");
                front = temp[0];
                back = temp[1];
                break;
            case Language.KOR:
                string[] temp1 = _id_Money.Kor.Split("{0:N0}");
                front = temp1[0];
                back = temp1[1];
                break;
            default:
                front = "";
                back = "";
                break;
        }
    }

    #endregion
    
    private void SetGoldText()
    {
        RenewalGoldText(DataTower.instance.money);
    }
    
    private void RenewalGoldText(ulong amount)
    {
        string temp = amount.TextFormatCurrency();
        _moneyText.text = $"{_moneyFrontText}{temp}{_moneyBackText}";
    }

    /// <summary>
    /// 낚시 업그레이드 목록 선택 토글
    /// </summary>
    public void OnClickToggleFishingUpgrade()
    {
        _audioManager.PlaySfxClick();
        _diningUpgradePanel.SetActive(false);
        _fishingUpgradePanel.SetActive(true);
        _toggleFishingUpgradeButton.interactable = false;
        _toggleDiningUpgradeButton.interactable = true;
    }

    /// <summary>
    /// 식당 업그레이드 목록 선택 토글
    /// </summary>
    public void OnClickToggleDiningUpgrade()
    {
        _audioManager.PlaySfxClick();
        _fishingUpgradePanel.SetActive(false);
        _diningUpgradePanel.SetActive(true);
        _toggleDiningUpgradeButton.interactable = false;
        _toggleFishingUpgradeButton.interactable = true;
    }

    

    
}
