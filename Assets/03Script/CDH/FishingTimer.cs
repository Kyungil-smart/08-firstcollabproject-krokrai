using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;

public class FishingTimer : MonoBehaviour
{
    [Range(0f, 3600f)]
    [SerializeField] public float fishingTime; // 낚시 가능 횟수 충전까지 남은 시간
    [SerializeField] public float maxFishingTime = 3600f; // 낚시 가능 횟수 최대 쿨타임
    [SerializeField] private FishingManager _manager;
    public TMP_Text timerText; // 시간을 화면에 표시할 UI 텍스트 컴포넌트

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

        if (!CheckingFull())
        {
            fishingTime = maxFishingTime;
            TimerUI(fishingTime);
        }
    }

    IEnumerator StartCountdown()
    {
        while (true)
        {
            if (!CheckingFull())
            {
                fishingTime = maxFishingTime;
                TimerUI(fishingTime);

                yield return new WaitUntil(CheckingFull);
            }

            yield return new WaitForSecondsRealtime(1f);
            fishingTime--;

            TimerUI(fishingTime);
            TimeCycle();
        }
    }

    private void TimerUI(float time)
    {
        int minutes = Mathf.FloorToInt(fishingTime / 60);
        int seconds = Mathf.FloorToInt(fishingTime % 60);
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

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

    private bool CheckingFull()
    {
        return _manager.GetCurrentCount() < _manager.fishingCount;
    }
}
