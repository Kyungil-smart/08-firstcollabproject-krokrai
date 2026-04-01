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
    [Header("확률 데이터 리스트")]
    public List<FishRateData> fishRateList = new List<FishRateData>();
    public int fishingCount = 1; // 최대 낚시 가능한 횟수
    private int _currentCount; // 현재 낚시 가능한 횟수
    public Sprite fishingImage; // 클릭 시 변경되는 스프라이트 이미지
    private Sprite _watingImage; // 대기 상태일 때의 스프라이트 이미지
    private Image _fisherImage;
    public FishingUI uiManager; // 낚시 횟수를 갱신할 FishingUI.cs 참조
    private FishingUpgradeManager _upgradeManager; // 업그레이드 매니저 불러옴
    private FishingTimer _timer; // 낚시 횟수 자동 충전 시간을 관리하는 타이머
    public List<FishData> fishDatabase = new List<FishData>(); // 게임에 존재하는 모든 물고기 데이터 리스트
    public FishRateData fishCurrentRate; // 구글 시트에서 받아온 등급별 확률 데이터

    private void Start()
    {
        _fisherImage = GetComponent<Image>();
        _watingImage = _fisherImage.sprite;
        _currentCount = fishingCount;
        _timer = FindFirstObjectByType<FishingTimer>();
        _upgradeManager = FindFirstObjectByType<FishingUpgradeManager>();

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }

        if (_upgradeManager != null)
        {
            _upgradeManager.OnRodUpgrade += RodgradeMaxCount;
            _upgradeManager.OnBaitUpgrade += BaitgradeMaxCount;
            _upgradeManager.OnFishingUpgrade += FishRateLevel;
            _upgradeManager.OnShipUpgrade += ShipUpgradeLevel;

            RodgradeMaxCount(DataTower.instance.rodLevel);
            BaitgradeMaxCount(DataTower.instance.baitLevel);
            FishRateLevel(DataTower.instance.fishingGrade);
            ShipUpgradeLevel(DataTower.instance.fishingGrade);
        }
    }

    /// <summary>
    ///  낚시대를 강화하여 최대 낚시 횟수를 늘리고 변경된 횟수를 UI에 반영
    /// </summary>
    public void UpgradeFishingRod()
    {
        _upgradeManager.RodUpgrade();
    }

    /// <summary>
    /// 낚시대 강화 레벨에 따라 낚시 가능한 최대 횟수 값을 결정
    /// </summary>
    public void RodgradeMaxCount(int newLevel)
    {
        switch (newLevel)
        {
            case 1: fishingCount = 1; break;
            case 2: fishingCount = 2; break;
            case 3: fishingCount = 3; break;
            case 4: fishingCount = 4; break;
            case 5: fishingCount = 5; break;
        }
    }

    /// <summary>
    /// 미끼를 강화하여 낚시 횟수 충전 시간을 단축시키는 로직 실행
    /// </summary>
    public void UpgradeFishingBait()
    {
        _upgradeManager.BaitUpgrade();
    }

    /// <summary>
    /// 미끼 강화 레벨에 따라 다음 충전까지 걸리는 최대 시간을 계산하여 타이머에 전달
    /// </summary>
    public void BaitgradeMaxCount(int newLevel)
    {
        float newTimer = 3600f;

        switch (newLevel)
        {
            case 1: newTimer = 3600f; break;
            case 2: newTimer = 3300f; break;
            case 3: newTimer = 3000f; break;
            case 4: newTimer = 2400f; break;
            case 5: newTimer = 1800f; break;
            default:
                if (newLevel >= 5)
                    newTimer = 1800f; break;
        }

        if (_timer != null)
        {
            _timer.UpdateMaxTime(newTimer);
        }
    }

    /// <summary>
    /// 오브젝트 클릭 시 낚시 횟수 차감 및 낚시 연출 실행 메서드
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentCount > 0)
        {
            _currentCount--;

            if (uiManager != null)
            {
                uiManager.UpdateCountText(_currentCount, fishingCount);
            }

            StartCoroutine(FishingImageD());
        }
    }

    /// <summary>
    /// 클릭 시 짧은 시간 동안 낚시 이미지를 보여준 후 다시 대기 이미지로 복구하고, 실제 물고기를 뽑기
    /// </summary>
    /// <returns></returns>
    IEnumerator FishingImageD()
    {
        if (fishingImage != null)
        {
            _fisherImage.sprite = fishingImage;
        }

        yield return new WaitForSeconds(0.1f);

        _fisherImage.sprite = _watingImage;

        GetRandomFish();
    }

    /// <summary>
    ///  외부에서 신호를 받아 낚시 횟수를 1회 충전하고 UI를 갱신하는 메서드
    /// </summary>
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

    /// <summary>
    /// 외부에서 현재 남은 낚시 횟수를 참조할 수 있게 반환
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCount()
    {
        return _currentCount;
    }

    /// <summary>
    /// fishDatabase의 Count 범위 내에서 무작위 FishData 반환
    /// </summary>
    public void GetRandomFish()
    {
        if (fishDatabase.Count == 0)
        {
            Debug.Log("등록된 물고기가 없습니다.");
            return;
        }

        string currentRarity = GetFishRarity();
        FishData selectedFish = GetRandomRarityFish(currentRarity);

        if (selectedFish != null)
        {
            Debug.Log($"{currentRarity}, {selectedFish.korName}");

            DataTower.instance.takeFish(selectedFish);
        }
    }

    /// <summary>
    /// 확률을 바탕으로 주사위를 굴려 당첨된 등급 문자열을 반환
    /// </summary>
    /// <returns></returns>
    private string GetFishRarity()
    {
        float rarityRate = UnityEngine.Random.Range(0f, 100f);
        float baseValue = 0;

        if (fishCurrentRate == null)
        {
            Debug.Log("데이터가 연결되지 않습니다.");
            return "Normal";
        }

        if (rarityRate <= (baseValue += fishCurrentRate.trash))
        { return "Trash"; }
        if (rarityRate <= (baseValue += fishCurrentRate.normal))
        { return "Normal"; }
        if (rarityRate <= (baseValue += fishCurrentRate.fine))
        { return "Fine"; }
        if (rarityRate <= (baseValue += fishCurrentRate.superior))
        { return "Superior"; }
        if (rarityRate <= (baseValue += fishCurrentRate.rare))
        { return "Rare"; }
        if (rarityRate <= (baseValue += fishCurrentRate.elite))
        { return "Elite"; }
        if (rarityRate <= (baseValue += fishCurrentRate.fantastic))
        { return "Fantastic"; }
        return "Legendary";
    }

    /// <summary>
    /// 특정 등급을 매개변수로 받아, 해당 등급에 속하는 물고기들 중 한 마리를 무작위로 선택하여 반환
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public FishData GetRandomRarityFish(string rarity)
    {
        List<FishData> filteredFish = new List<FishData>();

        foreach (FishData randomFish in fishDatabase)
        {
            if (randomFish.fishRarity.ToString() == rarity)
            {
                filteredFish.Add(randomFish);
            }
        }

        if (filteredFish.Count == 0)
        {
            Debug.Log($"물고기가 없습니다");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, filteredFish.Count);
        return filteredFish[randomIndex];
    }

    public void FishRateLevel(int level)
    {
        int index = level - 1;

        if (index >= 0 && index < fishRateList.Count)
        {
            fishCurrentRate = fishRateList[index];
            Debug.Log("확률 레벨 증가");
        }
    }

    public void UpgradeShipLevel()
    {
        _upgradeManager.ShipUpgrade();
    }

    public void ShipUpgradeLevel(int newLevel)
    {
        Debug.Log("배가 업그레이드 되었습니다 인벤토리가 확장되었는지 확인하세요");
    }
}
