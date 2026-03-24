using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class FishingManager : MonoBehaviour, IPointerClickHandler
{
    //public int currentLevel = 1;
    public int fishingCount = 1;
    private int _currentCount;
    public Sprite fishingImage;
    private Sprite _watingImage;
    private SpriteRenderer _spriteRenderer;
    public FishingUI uiManager;
    
    private FishingUpgradeManager _upgradeManager;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _watingImage = _spriteRenderer.sprite;
        _currentCount = fishingCount;

        _upgradeManager = FindFirstObjectByType<FishingUpgradeManager>();
        
        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }
    }

    public void UpgradeFishingRod()
    {
        //업그레이드 매니저의 강화 메서드를 사용하도록 변경
        //currentLevel++;
        _upgradeManager.RodUpgrade();
        UpgradeMaxCount();

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }
    }

    private void UpgradeMaxCount()
    {
        //업그레이드 매니저의 레벨을 사용하도록 변경
        //switch (currentLevel)
        switch (_upgradeManager.RodLevel)
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
        if (_currentCount > 0)
        {
            _currentCount--;

            if (uiManager != null)
            {
                uiManager.UpdateCountText(_currentCount, fishingCount);
            }

            StartCoroutine(ChangeImage());
        }
    }

    IEnumerator ChangeImage()
    {
        if (fishingImage != null)
        {
            _spriteRenderer.sprite = fishingImage;
        }

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.sprite = _watingImage;
    }

}
