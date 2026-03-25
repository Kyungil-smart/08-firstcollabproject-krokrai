using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class FishingUpgradeManager : MonoBehaviour
{
    // 싱글톤 여부는 협의해서 결정
    // 업그레이드 조건 확인, 수행
    // 업그레이드 효과는 다른 컴포넌트에서 수행

    /// <summary>
    /// 업그레이드 SO를 읽는 컴포넌트
    /// 프리팹 상 자식으로 들어가 있음
    /// </summary>
    private FishingUpgradeDataReader _dataReader;

    /// <summary>
    /// 테스트를 위한 수치
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int Temp_Gold;
    
    /// <summary>
    /// 낚시 등급 : 모든 낚시 업그레이드의 기본
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int FishingGrade;

    /// <summary>
    /// 미끼 레벨
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int BaitLevel;

    /// <summary>
    /// 낚시대 레벨
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int RodLevel;

    /// <summary>
    /// 배 레벨
    /// ToDo:DataTower로 차후 이관
    /// </summary>
    public int ShipLevel;
    
    public event Action OnFishingUpgrade;
    public event Action OnBaitUpgrade;
    public event Action OnRodUpgrade;
    public event Action OnShipUpgrade;

    /// <summary>
    /// ToDo:DataTower로 변수 이관 되면 사용 안함
    /// </summary>
    private void Awake()
    {
        _dataReader = GetComponentInChildren<FishingUpgradeDataReader>();
        
        // ToDo: ToDo:DataTower로 변수 이관 되면 이 밑의 변수는 삭제
        FishingGrade = 1;
        BaitLevel = 1;
        RodLevel = 1;
        ShipLevel = 1;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    /// <summary>
    /// 레벨을 불러오는 함수
    /// ToDo:DataTower로 변수 이관 되면 사용 안함
    /// </summary>
    /// <param name="fishingGrade">낚시 등급</param>
    /// <param name="baitLevel">미끼 레벨</param>
    /// <param name="rodLevel">낚시대 레벨</param>
    /// <param name="shipLevel">배 레벨</param>
    public void LoadLevel(int fishingGrade, int baitLevel, int rodLevel, int shipLevel)
    {
        FishingGrade = fishingGrade;
        BaitLevel = baitLevel;
        RodLevel = rodLevel;
        ShipLevel = shipLevel;
    }

    /// <summary>
    /// 낚시 레벨 증가
    /// </summary>
    public void FishingUpgrade()
    {
        int gold;
        if (CanFishingGradeUp(out gold))
        {
            FishingGrade++;
            Temp_Gold -= gold;
            OnFishingUpgrade?.Invoke();
        }
        
    }

    /// <summary>
    /// 미끼 레벨 증가
    /// </summary>
    public void BaitUpgrade()
    {
        int gold;
        if (CanBaitLevelUp(out gold))
        {
            BaitLevel++;
            Temp_Gold -= gold;
            OnBaitUpgrade?.Invoke();
        }
        
    }

    /// <summary>
    /// 낚시대 레벨 증가
    /// </summary>
    public void RodUpgrade()
    {
        int gold;
        if (CanRodLevelUp(out gold))
        {
            RodLevel++;
            Temp_Gold -= gold;
            OnRodUpgrade?.Invoke();
        }
        
    }

    /// <summary>
    /// 보트 레벨 증가
    /// </summary>
    public void ShipUpgrade()
    {
        int gold;
        if (CanShipLevelUp(out gold))
        {
            ShipLevel++;
            Temp_Gold -= gold;
            OnShipUpgrade?.Invoke();
        }
        
    }
    
    private bool CanFishingGradeUp(out int gold)
    {
        int level;
        int req_Gold;
        _dataReader.GetFishingGradeData(FishingGrade, out level ,out req_Gold);
        
        bool chackLevel = _dataReader.Grades.Length > level;
        bool chackGold = Temp_Gold >= req_Gold;
        
        gold = req_Gold;
        return chackGold && chackLevel;
    }
    
    private bool CanBaitLevelUp(out int gold)
    {
        int level;
        int req_Gold;
        _dataReader.GetBaitLevelData(BaitLevel, out level ,out req_Gold);
        
        bool chackLevel = _dataReader.Grades.Length > level;
        bool chackGold = Temp_Gold >= req_Gold;
        bool chackGrade = FishingGrade/2f > BaitLevel;
        
        gold = req_Gold;
        return chackGold && chackLevel && chackGrade;
    }

    private bool CanRodLevelUp(out int gold)
    {
        int level;
        int req_Gold;
        _dataReader.GetRodLevelData(RodLevel, out level ,out req_Gold);
        
        bool chackLevel = _dataReader.Grades.Length > level;
        bool chackGold = Temp_Gold >= req_Gold;
        bool chackGrade = FishingGrade/2f > RodLevel;
        
        gold = req_Gold;
        return chackGold && chackLevel && chackGrade;
    }

    private bool CanShipLevelUp(out int gold)
    {
        int level;
        int req_Gold;
        _dataReader.GetShipLevelData(ShipLevel, out level ,out req_Gold);
        
        bool chackLevel = _dataReader.Grades.Length > level;
        bool chackGold = Temp_Gold >= req_Gold;
        bool chackGrade = FishingGrade/2f > ShipLevel;
        
        gold = req_Gold;
        return chackGold && chackLevel && chackGrade;
    }

}
