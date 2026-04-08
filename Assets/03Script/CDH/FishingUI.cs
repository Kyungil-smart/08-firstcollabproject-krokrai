using UnityEngine;
using TMPro;

public class FishingUI : MonoBehaviour
{
    public TMP_Text countText; // 낚시 가능 횟수 / 최대 횟수를 보여주는 UI 텍스트 컴포넌트
    public TMP_Text timerText; // 시간을 화면에 표시할 UI 텍스트 컴포넌트

    /// <summary>
    /// sumarry : 낚시 가능 횟수 / 최대 횟수를 UI에 갱신
    /// </summary>
    public void UpdateCountText(int current, int total) 
    {
        if (countText != null)
        {
            countText.text = $"{current}/{total}";
        }
    }

    public void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }


}
