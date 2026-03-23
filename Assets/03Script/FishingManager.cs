using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FishingManager : MonoBehaviour, IPointerClickHandler
{
    public int fishingCount = 1;
    private int currentCount;
    public TMP_Text countText;
    public Sprite fishingImage;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCount = fishingCount;
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
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
            countText.text = $"{currentCount} / {fishingCount}";
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
