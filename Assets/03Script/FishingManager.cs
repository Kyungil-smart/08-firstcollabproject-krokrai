using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class FishingManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public int fishingCount;
    private int currentCount;
    public Sprite fishingImage;
    private Sprite watingImage;
    private SpriteRenderer spriteRenderer;
    public FishingUI uiManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        watingImage = spriteRenderer.sprite;
        currentCount = fishingCount;

        if (uiManager != null)
        {
            uiManager.UpdateCountText(currentCount, fishingCount);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentCount > 0)
        {
            currentCount--;

            if (uiManager != null)
            {
                uiManager.UpdateCountText(currentCount, fishingCount);
            }

            StartCoroutine(ChangeImage());
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
