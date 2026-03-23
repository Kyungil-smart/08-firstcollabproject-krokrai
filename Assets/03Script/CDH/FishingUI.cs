using UnityEngine;
using TMPro;
public class FishingUI : MonoBehaviour
{
    public TMP_Text countText;

    public void UpdateCountText(int current, int total)
    {
        if (countText != null)
        {
            countText.text = $"{current} / {total}";
        }
    }
}
