using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class FishingManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public int fishingCount;
    private int currentCount;
    public TMP_Text countText;
    public Sprite fishingImage;
    private Sprite watingImage;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        watingImage = spriteRenderer.sprite;
        currentCount = fishingCount;
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentCount > 0)
        {
            currentCount--;
            UpdateUI();
            StartCoroutine(ChangeImage());
        }
    }

    private void UpdateUI()
    {
        if (countText != null)
        {
            countText.text = $"{currentCount} / {fishingCount}";
        }
    }
    IEnumerator ChangeImage()
    {
        if (fishingImage != null)
        {
            spriteRenderer.sprite = fishingImage;
        }

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = watingImage;
    }

}
