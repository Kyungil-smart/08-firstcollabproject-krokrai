using System;
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
    
    [Header("TranslationData / 차후 수정")]
    //ToDo:ScriptableObject형식을 나중에 번역SO형식으로 교체해야됨
    [SerializeField] ScriptableObject _goldLanguageSO;

    private void OnEnable()
    {
        SetTranslationText();
        SetGoldText();
        DataTower.instance.OnChangedMoney += RenewalGoldText;
    }

    private void OnDisable()
    {
        DataTower.instance.OnChangedMoney -= RenewalGoldText;
    }

    private void Start()
    {
        OnClickToggleFishingUpgrade();
    }

    private void Update()
    {
        
    }

    private void SetTranslationText()
    {
        TranslationText(DataTower.instance.languageSetting);
    }
    
    /// <summary>
    /// ToDo:번역SO 추가되면 언어 연결 작업 필수
    /// </summary>
    private void TranslationText(Language language)
    {
        switch (language)
        {
            case Language.ENG:
                _moneyText.text = "_moneyText(EN)";
                _toggleFishingUpgradeText.text = "_fishingUpgradeText(EN)";
                _toggleDiningUpgradeText.text = "_diningUpgradeText(EN)";
                break;
            case Language.KOR:
                _moneyText.text = "_moneyText(KOR)";
                _toggleFishingUpgradeText.text = "_fishingUpgradeText(KOR)";
                _toggleDiningUpgradeText.text = "_diningUpgradeText(KOR)";
                break;
        }
    }

    private void SetGoldText()
    {
        RenewalGoldText(DataTower.instance.money);
    }
    
    /// <summary>
    /// 현재 골드가 변화 할 때, 골드 Test를 변화 시킴
    /// ToDo:DataTower 완성 되면 Tower의 Gold가 변화 할 때, 이벤트 체인으로 발동시킬 것
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
