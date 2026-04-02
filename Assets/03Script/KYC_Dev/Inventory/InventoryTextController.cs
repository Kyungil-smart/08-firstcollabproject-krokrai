using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryTextController : MonoBehaviour
{
    [Tooltip("UI Texts")]
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] TextMeshProUGUI _curGoldText;
    [SerializeField] TextMeshProUGUI _countText;
    [SerializeField] TextMeshProUGUI _sortSelectText;
    [SerializeField] TextMeshProUGUI _byCaughtText;
    [SerializeField] TextMeshProUGUI _byNameText;
    [SerializeField] TextMeshProUGUI _byRairtyText;

    [Header("TranslationData / AutoSetting / For Debug")]
    [SerializeField] TranslationData _id_Money;
    [SerializeField] TranslationData _id_Fish_Count;
    [SerializeField] TranslationData _id_Order;
    [SerializeField] TranslationData _id_Acquisition;
    [SerializeField] TranslationData _id_Name;
    [SerializeField] TranslationData _id_Rarity;

    private InventorySystem _inventorySystem;
    private TranslationDataReader _dataReader;
    private WaitForEndOfFrame _waitForEndOfFrame = new();

    private string _fishCountFrontText;
    private string _fishCountBackText;

    private void Awake()
    {
        _inventorySystem = FindFirstObjectByType<InventorySystem>();
        _dataReader = FindFirstObjectByType<TranslationDataReader>();
    }

    private void OnEnable()
    {
        StartCoroutine(LoadingOnEnableRoutine());
    }

    private void OnDisable()
    {
        EventDisable();
    }

    #region 이벤트 등록/해제

    private void EventEnable()
    {
        DataTower.instance.OnChangedMoney += RenewalGoldText;
        DataTower.instance.OnLanguageSettingChanged += TranslationText;
        _inventorySystem.OnInventoryCountChanged += RenewalCountText;
    }

    private void EventDisable()
    {
        DataTower.instance.OnChangedMoney -= RenewalGoldText;
        DataTower.instance.OnLanguageSettingChanged -= TranslationText;
        _inventorySystem.OnInventoryCountChanged -= RenewalCountText;
    }
    
    private IEnumerator LoadingOnEnableRoutine()
    {
        while (DataTower.instance == null)
        {
            yield return _waitForEndOfFrame;
        }
        
        EventEnable();
        SetGoldText();
        SetCountText();
        SetTranslationText();
    }

    #endregion
    
    #region 번역 관련
    
    private void SetTranslationText()
    {
        _id_Money = _dataReader.GetTranslationData("Money");
        _id_Fish_Count = _dataReader.GetTranslationData("Fish_Count");
        _id_Order = _dataReader.GetTranslationData("Order");
        _id_Acquisition = _dataReader.GetTranslationData("Acquisition");
        _id_Name = _dataReader.GetTranslationData("Name");
        _id_Rarity = _dataReader.GetTranslationData("Rarity");
        
        TranslationText(DataTower.instance.languageSetting);
    }

    private void TranslationText(Language language)
    {
        switch (language)
        {
            case Language.ENG:
                _moneyText.text = _id_Money.En;
                _sortSelectText.text = _id_Order.En;
                _byCaughtText.text = _id_Acquisition.En;
                _byNameText.text = _id_Name.En;
                _byRairtyText.text = _id_Rarity.En;
                SetFishCountTranslationText(language, out _fishCountFrontText, out _fishCountBackText);
                SetCountText();
                break;
            case Language.KOR:
                _moneyText.text = _id_Money.Kor;
                _sortSelectText.text = _id_Order.Kor;
                _byCaughtText.text = _id_Acquisition.Kor;
                _byNameText.text = _id_Name.Kor;
                _byRairtyText.text = _id_Rarity.Kor;
                SetFishCountTranslationText(language, out _fishCountFrontText, out _fishCountBackText);
                SetCountText();
                break;
        }
    }

    private void SetFishCountTranslationText(Language language, out string front, out string back)
    {
        string[] temp;
        switch (language)
        {
            case Language.ENG:
                temp = _id_Fish_Count.En.Split("{0}");
                front = temp[0];
                back = temp[1];
                break;
            case Language.KOR:
                temp = _id_Order.Kor.Split("{0}");
                front = temp[0];
                back = temp[1];
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
        _curGoldText.text = amount.TextFormatCurrency();
    }
    
    private void SetCountText()
    {
        RenewalCountText(_inventorySystem.InventoryCount);
    }
    
    private void RenewalCountText(int count)
    {
        _countText.text = $"{_fishCountFrontText}{count}{_fishCountBackText}";
    }
}
