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
    
    [Header("TranslationData / AutoSetting")]
    [SerializeField] FishingTranslationData _id_Money;
    [SerializeField] FishingTranslationData _id_Fishhook;
    [SerializeField] FishingTranslationData _id_Restaurant;
    
    private TranslationDataReader _translationDataReader;
    public WaitForEndOfFrame LoadingDelayWait {get; private set;}

    private void Awake()
    {
        LoadingDelayWait = new WaitForEndOfFrame();
        _translationDataReader = FindFirstObjectByType<TranslationDataReader>();
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
            yield return LoadingDelayWait;
        }
        
        SetGoldText();
        SetTranslationText();
        EventEnable();
    }

    #endregion
    
    

    private void SetTranslationText()
    {
        _translationDataReader.GetFTranslationData("Money", out _id_Money);
        _translationDataReader.GetFTranslationData("Fishhook", out _id_Fishhook);
        _translationDataReader.GetFTranslationData("Restaurant", out _id_Restaurant);
        TranslationText(DataTower.instance.languageSetting);
    }
    
    private void TranslationText(Language language)
    {
        switch (language)
        {
            case Language.ENG:
                _moneyText.text = _id_Money.En;
                _toggleFishingUpgradeText.text = _id_Fishhook.En;
                _toggleDiningUpgradeText.text = _id_Restaurant.En;
                break;
            case Language.KOR:
                _moneyText.text = _id_Money.Kor;
                _toggleFishingUpgradeText.text = _id_Fishhook.Kor;
                _toggleDiningUpgradeText.text = _id_Restaurant.Kor;
                break;
        }
    }

    private void SetGoldText()
    {
        RenewalGoldText(DataTower.instance.money);
    }
    
    /// <summary>
    /// 현재 골드가 변화 할 때, 골드 Text를 변화 시킴
    /// </summary>
    private void RenewalGoldText(ulong amount)
    {
        _currentGoldText.text = amount.ToString();
    }
    
    /// <summary>
    /// 닫기 버튼 구현
    /// </summary>
    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// BackGround 클릭했을 때 창 닫기 구현
    /// </summary>
    public void OnClickBackGround()
    {
        OnClickCloseButton();
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
