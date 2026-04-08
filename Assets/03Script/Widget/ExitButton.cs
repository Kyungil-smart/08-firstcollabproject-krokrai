using TMPro;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _exitText;
    [SerializeField] TextMeshProUGUI _cancelText;
    [SerializeField] TextMeshProUGUI _confirmText;

    Language lang;

    private void OnEnable()
    {
        lang = DataTower.instance.languageSetting;
        _exitText.text = lang == Language.KOR ? "" : "Do you want to <color=red>exit</color> the game?";
        _cancelText.text = lang == Language.KOR ? "아니요" : "No";
        _confirmText.text = lang == Language.KOR ? "네" : "Yes";
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
