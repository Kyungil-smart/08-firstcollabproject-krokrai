using UnityEngine;
using TMPro;
using System.Collections;

public class TextLanguage : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    private TranslationDataReader _reader;

    [Header("시트의 Id값을 똑같이 입력")]
    [SerializeField] private string translationKey;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _reader = FindFirstObjectByType<TranslationDataReader>();
    }

    private void OnEnable()
    {
        if (DataTower.instance != null)
        {
            // 이벤트 구독
            DataTower.instance.OnLanguageSettingChanged -= ApplyLanguage;
            DataTower.instance.OnLanguageSettingChanged += ApplyLanguage;
            
            // 켜질 때 현재 언어로 적용
            StartCoroutine(WaitAndApply());
        }
    }

    private void OnDisable()
    {
        if (DataTower.instance != null)
            DataTower.instance.OnLanguageSettingChanged -= ApplyLanguage;
    }

    private IEnumerator WaitAndApply()
    {
        // 둘 중 하나라도 없으면 한 프레임씩 기다림
        while (DataTower.instance == null || _reader == null)
        {
            if (_reader == null)
            {
                _reader = FindFirstObjectByType<TranslationDataReader>();
            }
            yield return null;
        }
        
        ApplyLanguage(DataTower.instance.languageSetting);
    }

    private void ApplyLanguage(Language language)
    {
        if (_tmp == null || _reader == null || string.IsNullOrEmpty(translationKey)) return;

        try 
        {
            TranslationData data = _reader.GetTranslationData(translationKey);
            if (data != null)
            {
                _tmp.text = (language == Language.KOR) ? data.Kor : data.En;
            }
        }
        catch (System.Exception)
        {
            Debug.LogWarning($"[번역에러] '{translationKey}'라는 ID를 시트 데이터에서 찾을 수 없음");
        }
    }
}
