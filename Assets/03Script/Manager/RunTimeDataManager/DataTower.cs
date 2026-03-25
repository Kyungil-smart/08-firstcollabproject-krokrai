using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class DataTower : MonoBehaviour
{
    public bool isDataInitiated = false;
    public static DataTower instance;

    private InventorySystem _inventorySystem; // 참조 걸어줄 방식 지정 필요.
    private Dictionary<string, FishData> FishDatas;  // 물고기 고유번호, 물고기 저장방식(SO) 기입 후 사용 예정. 목적 : 데이터 검사용 예시 : 해당 물고기가 도감에 등록 되어 있는지

    public Language languageSetting { get; private set; } /// <summary> 현재 설정된 언어, 기본 값 : 영어 </summary>

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
        languageSetting = Language.ENG; // 기본 값 영어로 출력 되게 설정 되었습니다. 
    }

    public void InitializedData(bool ForcedInitialized = false)
    {
        if (!isDataInitiated || !ForcedInitialized)
        {
            if (ForcedInitialized)
                Debug.Log("강제 초기화 실행");
            FishDatas = new Dictionary<string, FishData>(50); // 하드 코딩 되어 있으니 후에 데이터 테이블 완성 후 수정 필요 @@@@@@@@@@@@@@@@@@
            money = 0;
            customerVisitCounter = 0;
            masterVolume = 0;
            BGMVolume = 0;
            SFXVolume = 0;
            fishingCount = 0;
            currentCount = 0;
            rechargeBaitTimer = 0;
        }
        else
        {
            Debug.LogWarning("강제 되지 않은 초기화 선언 감지됌");
        }
    }

    /// <summary>
    /// 저장을 위해서 데이터를 호출 할 수 있는 부분.
    /// </summary>
    public void PullData()
    {
        
    }

    /// <summary>
    /// 언어 설정을 위한 함수. 설정에서만 호출 할 것.
    /// </summary>
    /// <param name="lan">KOR : 한글 , ENG : 영어.</param>
    public void ChangeLanguage(Language lan)
    {
        languageSetting = lan;
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

    #region 기본 변수

    /// <summary>
    /// 소지 금액 ulong으로 받아야함.
    /// </summary>
    public ulong money { get; private set; }

    /// <summary>
    /// 차감 후 금액 반환.
    /// </summary>
    public event Action<ulong> OnChangedMoney;

    /// <summary>
    /// 입력된 값으로 차감 시도
    /// 성공한 경우 : OnChangedMoney 호출 및 ture 반환
    /// 실패한 경우 : false 반환 (가급적이면 false 반환하지 않게 예외처리 할 것.)
    /// </summary>
    /// <param name="mny">차감할 금액</param>
    /// <returns></returns>
    public bool TryMoenyChanged(ulong mny)
    {
        if (money - mny < 0)
            return false;
        money -= mny;
        OnChangedMoney?.Invoke(money);
        return true;
    }
    


    //List<Temp_Item> Items; // Item SO 작업 후 추가 작업 예정 @@@@@@@@@@@@@@@@@@@@


    #endregion

    #region 식당

    /// <summary>
    /// 손님 방문 횟수 카운터용
    /// </summary>
    public ulong customerVisitCounter;

    #endregion

    #region 개인 설정 변수

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

    /// <summary>
    /// 투명도 조절 변수 0~100 값만 사용 예정
    /// 투명도가 높을 수록 게임이 투명해지며, 최대치 일때 게임은 알파 값 30 유지.
    /// </summary>
    public byte transparentLevel;

    #endregion

    #region 낚시 변수



    /// <summary>
    /// 최대로 저장할 수 있는 미끼 수
    /// </summary>
    public int fishingCount;

    /// <summary>
    /// 현재 갖고 있는 미끼 수.
    /// </summary>
    public int currentFishingCount;

    /// <summary>
    /// 현재 미끼 충전 타이머.
    /// </summary>
    public float fishingTime;

    /// <summary>
    /// 충전 타이머 시작점
    /// </summary>
    public float maxFishingTime;

    #endregion

    #region 식당 관련 함수들

    #endregion

    #region 낚시 관련 함수들
    // 아마 So로 넘어올거 같다.
    /// <summary>
    /// 물고기를 잡은 경우 호출.
    /// </summary>
    public void takeFish() // takeFish(FishData fish)
    {
        //_inventorySystem.Insert(); // Item SO 변경 후 작업 @@@@@@@@@@@@@@@@@@@@@@@@@@
        // if ( _fishDatas.[fish.fishID].isfirst )
        // 
    }
    #endregion


}
