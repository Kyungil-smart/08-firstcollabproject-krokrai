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

    /// <summary>
    /// 저장을 위해서 데이터를 호출 할 수 있는 부분.
    /// </summary>
    public void PullData()
    {
        
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

    #region 기본

    /// <summary>
    /// 소지 금액 절대값 long으로 받아야함.
    /// </summary>
    public ulong money;


    #endregion

    #region 식당

    /// <summary>
    /// 손님 방문 횟수 카운터용
    /// </summary>
    public ulong customerVisitCounter;

    #endregion

    #region 음향

    /// <summary>
    /// 마스터 볼륨 조절용
    /// </summary>
    public float masterVolume; // 실제 표기는 0~100 정수 값. 실제 slider에 들어가는 값은 0~1의 실수 값.
    
    /// <summary>
    /// BGM 볼륨
    /// </summary>
    public float BGMVolume;
    
    /// <summary>
    /// SFX 볼륨
    /// </summary>
    public float SFXVolume;

    #endregion

    #region 인벤토리



    #endregion


    #region 낚시 



    /// <summary>
    /// 최대로 저장할 수 있는 미끼 수
    /// </summary>
    public int fishingMaxBait;

    /// <summary>
    /// 현재 갖고 있는 미끼 수.
    /// </summary>
    public int fishingCurrentBait;

    /// <summary>
    /// 미끼 충전 타이머.
    /// </summary>
    public float rechargeBaitTimer;

    #endregion
}
