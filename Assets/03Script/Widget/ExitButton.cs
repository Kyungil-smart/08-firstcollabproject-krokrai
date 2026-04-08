using TMPro;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _exitText;
    [SerializeField] TextMeshProUGUI _cancelText;
    [SerializeField] TextMeshProUGUI _confirmText;
    [SerializeField] TranslationDataReader _reader;

    Language lang;

    TranslationData _exitData;
    TranslationData _cancelData;
    TranslationData _confirmData;

    private void OnEnable()
    {
        lang = DataTower.instance.languageSetting;

        _exitData = _reader.GetTranslationData("Game Exit");
        _cancelData = _reader.GetTranslationData("No");
        _confirmData = _reader.GetTranslationData("Yes");

        _exitText.text = lang == Language.KOR ? _exitData.Kor : _exitData.En;
        _cancelText.text = lang == Language.KOR ? _cancelData.Kor : _cancelData.En;
        _confirmText.text = lang == Language.KOR ? _confirmData.Kor : _confirmData.En;
    }

    public void OnExitPopup()
    {
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
