using UnityEngine;
using UnityEngine.UI;

public class ToggleImageSwap : MonoBehaviour
{
    public Sprite on;       // on이미지
    public Sprite off;      // off이미지
    public Image target;   // 본인의 Image 컴포넌트

    public void OnValueChange(bool isOn)
    {
        target.sprite = isOn ? on : off;
    }
}