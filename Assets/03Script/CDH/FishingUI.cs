using UnityEngine;
using TMPro;
public class FishingUI : MonoBehaviour
{
    public TMP_Text countText; // sumarry : 낚시 가능 횟수 / 최대 횟수를 보여주는 UI 텍스트 컴포넌트

    public void UpdateCountText(int current, int total) // sumarry : 낚시 가능 횟수 / 최대 횟수를 UI에 갱신하는 메서드
    {
        if (countText != null)
        {
            countText.text = $"{current} / {total}";
        }
    }
}
