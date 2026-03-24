using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class FishingManager : MonoBehaviour, IPointerClickHandler
{
    public int currentLevel = 1; // sumarry :
    public int fishingCount = 1; // sumarry : 최대 낚시 가능한 횟수
    private int _currentCount; // sumarry : 현재 낚시 가능한 횟수
    public Sprite fishingImage; // sumarry : 클릭 시 변경되는 스프라이트 이미지
    private Sprite _watingImage; // sumarry : 대기 상태일 때의 스프라이트 이미지
    private SpriteRenderer _spriteRenderer; // sumarry : 이미지 교체를 위해 오브젝트의 SpriteRenderer 컴포넌트를 참조
    public FishingUI uiManager; // sumarry : 낚시 횟수를 갱신할 FishingUI.cs 참조

    public int GetCurrentCount()
    {
        return _currentCount;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _watingImage = _spriteRenderer.sprite;
        _currentCount = fishingCount;

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }
    }

    public void UpgradeFishingRod()
    {
        currentLevel++;
        UpgradeMaxCount();

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }
    }

    public void UpgradeMaxCount()
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

    public void OnPointerClick(PointerEventData eventData) // sumarry : 오브젝트 클릭 시 낚시 횟수 차감 및 낚시 연출 실행 메서드
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

    public void FishingChance() // sumarry :외부에서 호출 시 낚시 횟수를 1회 충전하고 UI를 갱신하는 메서드
    {
        if (_currentCount < fishingCount)
        {
            _currentCount++;

            if (uiManager != null)
            {
                uiManager.UpdateCountText(_currentCount, fishingCount);
            }
        }
    }
}
