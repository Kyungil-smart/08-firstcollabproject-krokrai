using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishingManager : MonoBehaviour, IPointerClickHandler
{
    public int fishingCount = 1; // sumarry : 최대 낚시 가능한 횟수
    private int _currentCount; // sumarry : 현재 낚시 가능한 횟수
    public Sprite fishingImage; // sumarry : 클릭 시 변경되는 스프라이트 이미지
    private Sprite _watingImage; // sumarry : 대기 상태일 때의 스프라이트 이미지
    private SpriteRenderer _spriteRenderer; // sumarry : 이미지 교체를 위해 오브젝트의 SpriteRenderer 컴포넌트를 참조
    public FishingUI uiManager; // sumarry : 낚시 횟수를 갱신할 FishingUI.cs 참조
    private FishingUpgradeManager _upgradeManager; // 업그레이드 매니저 불러옴
    private FishingTimer _timer;
    public List<FishData> fishDatabase = new List<FishData>();

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _watingImage = _spriteRenderer.sprite;
        _currentCount = fishingCount;
        _timer = FindFirstObjectByType<FishingTimer>();
        _upgradeManager = FindFirstObjectByType<FishingUpgradeManager>();

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }
    }

    // sumarry : 낚시대를 강화하여 최대 낚시 횟수를 늘리고 변경된 횟수를 UI에 반영
    public void UpgradeFishingRod() 
    {
        _upgradeManager.RodUpgrade();
        RodgradeMaxCount();

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }
    }

    // sumarry : 낚시대 강화 레벨에 따라 낚시 가능한 최대 횟수 값을 결정
    public void RodgradeMaxCount() 
    {
        switch (_upgradeManager.RodLevel)
        {
            case 1: fishingCount = 1; break;
            case 2: fishingCount = 2; break;
            case 3: fishingCount = 3; break;
            case 4: fishingCount = 4; break;
            case 5: fishingCount = 5; break;
        }
    }

    // sumarry : 미끼를 강화하여 낚시 횟수 충전 시간을 단축시키는 로직 실행
    public void UpgradeFishingBait() 
    {
        _upgradeManager.BaitUpgrade();
        BaitgradeMaxCount();
    }

    // sumarry : 미끼 강화 레벨에 따라 다음 충전까지 걸리는 최대 시간을 계산하여 타이머에 전달
    public void BaitgradeMaxCount()
    {
        float newTimer = 1800f;

        switch (_upgradeManager.BaitLevel)
        {
            case 1: newTimer = 1800f; break;
            case 2: newTimer = 1500f; break;
            case 3: newTimer = 1200f; break;
            case 4: newTimer = 900f; break;
            case 5: newTimer = 600f; break;
            default:
                if (_upgradeManager.BaitLevel >= 5)
                    newTimer = 600f; break;
        }

        if (_timer != null)
        {
            _timer.UpdateMaxTime(newTimer);
        }
    }

    // sumarry : 오브젝트 클릭 시 낚시 횟수 차감 및 낚시 연출 실행 메서드
    public void OnPointerClick(PointerEventData eventData) 
    {
        if (_currentCount > 0)
        {
            _currentCount--;

            if (uiManager != null)
            {
                uiManager.UpdateCountText(_currentCount, fishingCount);
            }

            StartCoroutine(FishingImage());
        }
    }

    IEnumerator FishingImage()
    {
        if (fishingImage != null)
        {
            _spriteRenderer.sprite = fishingImage;
        }

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.sprite = _watingImage;

        GetRandomFish();
    }

    // sumarry :외부에서 신호를 받아 낚시 횟수를 1회 충전하고 UI를 갱신하는 메서드
    public void FishingChance() 
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

    // sumarry : 외부에서 현재 남은 낚시 횟수를 참조할 수 있게 반환
    public int GetCurrentCount()
    {
        return _currentCount;
    }

    // sumarry : fishDatabase의 Count 범위 내에서 무작위 FishData 반환
    public void GetRandomFish()
    {
        if (fishDatabase.Count == 0)
        {
            Debug.Log("등록된 물고기가 없습니다.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, fishDatabase.Count);
        FishData selectedFish = fishDatabase[randomIndex];
        Debug.Log("낚시 성공!");
    }
}
