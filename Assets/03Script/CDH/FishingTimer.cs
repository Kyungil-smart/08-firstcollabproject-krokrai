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

            fishingTime = maxFishingTime;
            TimerUI(fishingTime);
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
                TimerUI(fishingTime);

                yield return new WaitUntil(() => !CheckingFull());
            }

            yield return new WaitForSecondsRealtime(1f);
            fishingTime--;

            TimerUI(fishingTime);
            TimeCycle();
        }
    }

    /// <summary>
    /// 남은 시간을 00:00 형태로 UI에 표시
    /// </summary>
    /// <param name="time">표시할 시간</param>
    private void TimerUI(float time)
    {
        int minutes = Mathf.FloorToInt(fishingTime / 60);
        int seconds = Mathf.FloorToInt(fishingTime % 60);
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
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
