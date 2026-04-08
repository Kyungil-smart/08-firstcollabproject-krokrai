using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;

public class FishingTimer : MonoBehaviour
{
    [Range(0f, 3600f)]
    [SerializeField] public float fishingTime; // 현재 남은 최대 쿨타임
    [SerializeField] public float maxFishingTime = 3600f; // 충전 완료까지의 최대 시간
    [SerializeField] private FishingManager _manager; // 낚시 카운트를 관리하는 매니저
   

    private void Start()
    {
        if (_manager == null)
    {
        Debug.Log("인스펙터창을 확인하세요.");
    }
        StartCoroutine(StartCountdown());
    }

    /// <sumarry> 
    /// 미끼 레벨업 시 최대 쿨타임을 갱신하고, 횟수가 가득 찬 경우 UI를 즉시 업데이트
    /// </sumarry>
    public void UpdateMaxTime(float newTime)
    {
        maxFishingTime = newTime;

        fishingTime = maxFishingTime;
        UpdateTimerUI();
    }

    /// <summary>
    /// 1초마다 쿨타임을 감소시키는 루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCountdown()
    {
        while (true)
        {
            if (CheckingFull())
            {
                fishingTime = maxFishingTime;
                UpdateTimerUI();

                yield return new WaitUntil(() => !CheckingFull());
            }

            yield return new WaitForSeconds(1f);
            fishingTime--;

            UpdateTimerUI();
            TimeCycle();
        }
    }

    /// <summary>
    ///
    /// </summary>
    private void UpdateTimerUI()
    {
        if (_manager != null && _manager.uiManager != null)
        {
            _manager.uiManager.UpdateTimerText(fishingTime);
        }        
    }

    /// <summary>
    /// 시간이 0이 되면 낚시 기회를 1 추가하고 타이머를 재시작
    /// </summary>
    private void TimeCycle() 
    {
        if (fishingTime <= 0)
        {
            fishingTime = maxFishingTime;

            if (_manager != null)
            {
                _manager.FishingChance();
            }
        }
    }

    /// <summary>
    /// 현재 유저의 낚시 가능 횟수가 최대치인지 확인
    /// </summary>
    /// <returns>최대치일때 true 반환</returns>
    public bool CheckingFull()
    {
        return _manager.GetCurrentCount() >= _manager.fishingCount;
    }
}
