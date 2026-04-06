using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    [Header("확률 데이터 리스트")]
    public List<FishRateData> fishRateList = new List<FishRateData>();
    [Header("스크립트 연결")]
    [SerializeField] private FishingTimer _timer;
    [SerializeField] private FishingUpgradeManager _upgradeManager; // 업그레이드 데이터 참조
    public int fishingCount = 1; // 최대 낚시 가능한 횟수
    private int _currentCount; // 현재 낚시 가능한 횟수
    public FishingUI uiManager; // 낚시 횟수를 갱신할 Ui 참조

    public List<FishData> fishDatabase = new List<FishData>(); // 게임에 존재하는 모든 물고기 데이터 리스트
    public FishRateData fishCurrentRate; // 현재 레벨에 적용된 등급별 확률 데이터

    private Animator _animator;
    private Vector2 _pressPos;
    private FishData _lastCaughtFish;
    private GameObject _currentFish;

    [Header("물고기 연출")]
    public GameObject fishPrefab;
    public Transform popFishPoint;
    public List<Sprite> fishSprites;
    public List<Sprite>raritySprites;
    
   

    private void Start()
    {
        _animator = GetComponent<Animator>();

        // FishingUpgradeManager의 이벤트에 메서드들을 연결
        if (_upgradeManager != null)
        {
            _upgradeManager.OnRodUpgrade += RodgradeMaxCount;
            _upgradeManager.OnBaitUpgrade += BaitgradeMaxCount;
            _upgradeManager.OnFishingUpgrade += FishRateLevel;
            _upgradeManager.OnShipUpgrade += ShipUpgradeLevel;

            // 현재 저장된 레벨 데이터로 초기 설정
            RodgradeMaxCount(DataTower.instance.rodLevel);
            BaitgradeMaxCount(DataTower.instance.baitLevel);
            FishRateLevel(DataTower.instance.fishingGrade);
            ShipUpgradeLevel(DataTower.instance.fishingGrade);
        }

        _currentCount = fishingCount;

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }

        StartCoroutine(IdleVariationRoutine());
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) RodgradeMaxCount(1);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) RodgradeMaxCount(2);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) RodgradeMaxCount(3); 
        if (Keyboard.current.digit4Key.wasPressedThisFrame) RodgradeMaxCount(4);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) RodgradeMaxCount(5);

        if (Keyboard.current.qKey.wasPressedThisFrame) FishRateLevel(1);
        if (Keyboard.current.wKey.wasPressedThisFrame) FishRateLevel(2);
        if (Keyboard.current.eKey.wasPressedThisFrame) FishRateLevel(3);
        if (Keyboard.current.rKey.wasPressedThisFrame) FishRateLevel(4);
        if (Keyboard.current.tKey.wasPressedThisFrame) FishRateLevel(5);
        if (Keyboard.current.yKey.wasPressedThisFrame) FishRateLevel(6);
        if (Keyboard.current.uKey.wasPressedThisFrame) FishRateLevel(7);
        if (Keyboard.current.iKey.wasPressedThisFrame) FishRateLevel(8);
        if (Keyboard.current.oKey.wasPressedThisFrame) FishRateLevel(9);
        if (Keyboard.current.pKey.wasPressedThisFrame) FishRateLevel(10);


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

        Debug.Log($"낚싯대 레벨: {fishingCount}");

        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
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

        Debug.Log($"현재 미끼 레벨: {newLevel}");

        if (_timer != null)
        {
            _timer.UpdateMaxTime(newTimer);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressPos = eventData.position;
    }

    /// <summary>
    /// 오브젝트 클릭 시 낚시 횟수 차감 및 낚시 연출 실행 메서드
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        float distance = (eventData.position - _pressPos).sqrMagnitude;

        if (distance > 100f)
        {
            Debug.Log("드래그중! 낚시 X.");
            return;
        }

        AnimatorStateInfo aniState = _animator.GetCurrentAnimatorStateInfo(0);

        if (aniState.IsName("result"))
        {
            if (_currentCount > 0)
            {
                _currentCount--;
                UpdateUI();
                GetRandomFish();

                _animator.SetTrigger("resultCatch");
            }
            else
            {
                Debug.Log("낚시 횟수 0회");
            }
            return;
        }

        if (aniState.IsName("fishing"))
        {
            if (_currentCount > 0)
            {
                _currentCount--;
                UpdateUI();
                GetRandomFish();
            }
            _animator.SetTrigger("resultCatch");
            return;
        }

        if (_currentCount > 0)
        {
            {
                _animator.SetTrigger("Fishing");
                _currentCount--;
                UpdateUI();
                GetRandomFish();
            }
        }
    }
    private void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateCountText(_currentCount, fishingCount);
        }
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

            _lastCaughtFish = selectedFish;

            if (DataTower.instance != null)
            {
                DataTower.instance.takeFish(selectedFish);
            }
            
            else
            {
                Debug.Log("DataTower 인스턴스를 찾을 수 없습니다.");
            }
        }
    }
    public void OnCatchAnimationEvent()
    {
        if (_lastCaughtFish != null)
        {
            PopFishResult(_lastCaughtFish);
            _lastCaughtFish = null;
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
            Debug.Log($"현재 플레이어 레벨: {level}");
        }
    }

    public void UpgradeShipLevel()
    {
        _upgradeManager.ShipUpgrade();
    }

    public void ShipUpgradeLevel(int newLevel)
    {
        Debug.Log($"현재 배 레벨: {newLevel}");
    }

    IEnumerator IdleVariationRoutine()
    {
        while (true)
        {
            bool isFullNow = _timer.CheckingFull();
            _animator.SetBool("IsFull", isFullNow);

            if (!isFullNow)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            float waitTime = UnityEngine.Random.Range(10f, 15f);
            float timeFlow = 0f;

            while (timeFlow < waitTime)
            {
                if (!_timer.CheckingFull())
                    break;

                timeFlow += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

            if (!_timer.CheckingFull()) continue;

            float randomVar = UnityEngine.Random.Range(0f, 100f);
            int targetIdx = 0;

            if (randomVar <= 40f)
            {
                targetIdx = 1;
            }

            else if (randomVar <= 60f)
            {
                targetIdx = 2;
            }

            else targetIdx = 3;

            _animator.SetInteger("IdleIdx", targetIdx);

            yield return new WaitForSeconds(0.1f);
            _animator.SetInteger("IdleIdx", 0);

        }
    }

    private void PopFishResult(FishData data)
    {
        if (this == null || data == null || fishPrefab == null || popFishPoint == null) 
        { 
            return; 
        }

        if (_currentFish != null)
        {
            Destroy(_currentFish);
        }

        GameObject go = Instantiate(fishPrefab, popFishPoint.position, Quaternion.identity, popFishPoint);
        _currentFish = go;

        SpriteRenderer sr = go.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {

            Sprite targetSprite = fishSprites.Find(x => x.name == data.fishSprite);
            if (targetSprite != null)
            {
                sr.sprite = targetSprite;
            }
            else
            {
                Debug.LogWarning($"{data.fishSprite} 없음");
            }

            sr.sortingOrder = 5;
        }
        Destroy(go, 3f);
    }
}
