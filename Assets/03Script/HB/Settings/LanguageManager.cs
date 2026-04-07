using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public void SetKorean()
    {
        if (DataTower.instance != null)
        {
            DataTower.instance.ChangeLanguage(Language.KOR); 
            Debug.Log ("한국어로 변경");
        }
    }

    public void SetEnglish()
    {
        if (DataTower.instance != null)
        {
            DataTower.instance.ChangeLanguage(Language.ENG); 
            Debug.Log ("영어로 변경");
        }
    }
}
