using System;
using UnityEngine;
using TMPro;

public class UpgradeUIView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] TextMeshProUGUI _currentGoldText;
    
    [Header("TranslationData")]
    //ToDo:ETestLanguage형식을 나중에 번역 선택 enum으로 교체해야됨
    [SerializeField] ETestLanguage _testLanguage;
    //ToDo:ScriptableObject형식을 나중에 번역SO형식으로 교체해야됨
    [SerializeField] ScriptableObject _goldLanguageSO;
    
    //ToDo:DataTower에 업그레이드 변수 이관 되면 주소 수정
    private FishingUpgradeManager _fishingUpgradeManager;

    private void Awake()
    {
        _fishingUpgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
    }


    private void OnEnable()
    {
        TranslationText();
        RenewalGoldText(); // 업그레이드 창이 열릴 때 마다 갱신 필요
        //ToDo:RenewalGoldText 이벤트 등록
    }

    private void OnDisable()
    {
        //ToDo:RenewalGoldText 이벤트 해제
    }


    /// <summary>
    /// ToDo:DataTower 작업 끝나면 변수 변경할것
    /// ToDo:번역SO 추가되면 언어 연결 작업 필수
    /// </summary>
    private void TranslationText()
    {
        switch (_testLanguage)
        {
            case ETestLanguage.EN:
                _moneyText.text = "_moneyText(EN)";
                break;
            case ETestLanguage.KOR:
                _moneyText.text = "_moneyText(KOR)";
                break;
        }
    }

    /// <summary>
    /// 현재 골드가 변화 할 때, 골드 Test를 변화 시킴
    /// ToDo:DataTower 완성 되면 Tower의 Gold가 변화 할 때, 이벤트 체인으로 발동시킬 것
    /// </summary>
    private void RenewalGoldText()
    {
        _currentGoldText.text = _fishingUpgradeManager.Temp_Gold.ToString();
    }
    
    /// <summary>
    /// 닫기 버튼 구현
    /// </summary>
    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }
}
