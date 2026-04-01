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
    [SerializeField] TextMeshProUGUI _currentGoldText;
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

    private void Awake()
    {
        _tDataReader = FindFirstObjectByType<TranslationDataReader>();
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
                _moneyText.text = _id_Money.En;
                _toggleFishingUpgradeText.text = _id_Fishing.En;
                _toggleDiningUpgradeText.text = _id_Restaurant.En;
                break;
            case Language.KOR:
                _moneyText.text = _id_Money.Kor;
                _toggleFishingUpgradeText.text = _id_Fishing.Kor;
                _toggleDiningUpgradeText.text = _id_Restaurant.Kor;
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
        _currentGoldText.text = amount.ToString();
    }

    /// <summary>
    /// 낚시 업그레이드 목록 선택 토글
    /// </summary>
    public void OnClickToggleFishingUpgrade()
    {
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
        _fishingUpgradePanel.SetActive(false);
        _diningUpgradePanel.SetActive(true);
        _toggleDiningUpgradeButton.interactable = false;
        _toggleFishingUpgradeButton.interactable = true;
    }

    

    
}
