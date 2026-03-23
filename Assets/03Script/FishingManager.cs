using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class FishingManager : MonoBehaviour, IPointerClickHandler
{
    public int currentLevel = 1;
    public int MaxLevel = 5;
    public int fishingCount = 1;
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

    public void UpgradeFishingRod()
    {
        currentLevel++;
        UpgradeMaxCount();

        if (uiManager != null)
        {
            uiManager.UpdateCountText(currentCount, fishingCount);
        }
    }

    private void UpgradeMaxCount()
    {
        switch (currentLevel)
        {
            case 1: fishingCount = 1; break;
            case 2: fishingCount = 2; break;
            case 3: fishingCount = 3; break;
            case 4: fishingCount = 4; break;
            case 5: fishingCount = 5; break;
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
