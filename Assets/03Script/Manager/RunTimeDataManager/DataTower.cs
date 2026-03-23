using System.Data.SqlTypes;
using UnityEngine;

public class DataTower : MonoBehaviour
{
    public static DataTower instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // 돈
    // 인벤토리, 생선
    // 도감????
    // 손님 방문 횟수. 잠수 중 지나간 손님 수
    // 업그레이드, 어떤 걸 얼만큼 업그레이드 되어 있는 지.
    // 초밥 수
    // 언어 등 개인 설정
    // 미끼 갯수
    // 타이머

    public ulong money;

    public ulong customerVisitCounter;

    public float masterVolume; // 실제 표기는 0~100 정수 값. 실제 slider에 들어가는 값은 0~1의 실수 값.
    public float BGMVolume;
    public float SFXVolume;

    // 데이터에 따라 값 타입 바꿔 줘야함.
    public byte fishingBait;

    // 이부분도 구현 부분에 따라 타입 전환 필요
    public float fishingTimer;
}
