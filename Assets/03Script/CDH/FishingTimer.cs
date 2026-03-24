using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;

public class FishingTimer : MonoBehaviour
{
    [Range(0f, 1800f)]
    [SerializeField] public float fishingTime; // sumarry : 낚시 가능 횟수 충전까지 남은 시간
    [SerializeField] public float maxFishingTime = 1800f; // sumarry : 낚시 가능 횟수 최대 쿨타임
    public TMP_Text timerText; // sumarry : 시간을 화면에 표시할 UI 텍스트 컴포넌트
    private FishingManager _manager; // sumarry : 낚시 횟수 증가를 위한 FishingManager.cs 참조

    private void Start()
    {
        _manager = Object.FindFirstObjectByType<FishingManager>();
        StartCoroutine(StratCountdown());
    }

    IEnumerator StratCountdown()
    {
        while (true)
        {
            int minutes = Mathf.FloorToInt(fishingTime / 60);
            int seconds = Mathf.FloorToInt(fishingTime % 60);
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            yield return new WaitForSeconds(1f);
            fishingTime--;
            TimeCycle();
        }
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
}
