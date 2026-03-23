using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingManager : MonoBehaviour
{
    public int fishingCount = 1;
    private int currentCount;
    public TMP_Text countText;

    public Sprite fishingImage;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentCount = fishingCount;
        UpdateUI();
    }

    private void OnMouseDown()
    {
        if (currentCount > 0)
        {
            currentCount--;
            UpdateUI();
            ChangeFisherImage();
        }
    }

    private void UpdateUI()
    {
        if (countText != null)
        {
            countText.text = currentCount + " / " + fishingCount;
        }
    }
    private void ChangeFisherImage()
    {
        if (fishingImage != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = fishingImage;
        }
    }

}
