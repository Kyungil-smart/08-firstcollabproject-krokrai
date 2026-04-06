using UnityEngine;
using TMPro;

public class TextLanguage : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    [Header("언어별 텍스트 설정")]
    [SerializeField] private string korText;
    [SerializeField] private string engText;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged += ApplyLanguage;

            ApplyLanguage(DataTower.instance.languageSetting);
        }
    }

    private void OnDisable()
    {
        if(DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged -= ApplyLanguage;
        }
    }

    private void ApplyLanguage(Language language)
    {
        if (_tmp == null) return;

        _tmp.text = (language == Language.KOR) ? korText : engText;
    }
}
