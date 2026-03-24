using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FishingUpgradeDataReader : MonoBehaviour
{
    [Header("업그레이드 데이터 SO")]
    [Tooltip("업그레이드 만약 추가 된다면 각각에 추가 할 것")]
    public FishingUpgradeData[] Grades;
    public FishingUpgradeData[] Baits;
    public FishingUpgradeData[] Rods;
    public FishingUpgradeData[] Ships;

    /// <summary>
    /// 플레이어 낚시 등급 데이터 읽어오는 함수
    /// </summary>
    /// <param name="grade">현재 레벨 입력</param>
    /// <param name="level">SO에 입력 된 레벨 데이터</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetFishingGradeData(int grade, out int level, out int req_Gold)
    {
        int index = grade - 1;
        level = Grades[index].Level;
        req_Gold = Grades[index].Req_Gold;
    }
    
    /// <summary>
    /// 미끼 레벨 데이터 읽어오는 함수
    /// </summary>
    /// <param name="grade">현재 레벨 입력</param>
    /// <param name="level">SO에 입력 된 레벨 데이터</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetBaitLevelData(int grade, out int level, out int req_Gold)
    {
        int index = grade - 1;
        level = Baits[index].Level;
        req_Gold = Baits[index].Req_Gold;
    }
    
    /// <summary>
    /// 낚시대 레벨 데이터 읽어오는 함수
    /// </summary>
    /// <param name="grade">현재 레벨 입력</param>
    /// <param name="level">SO에 입력 된 레벨 데이터</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetRodLevelData(int grade, out int level, out int req_Gold)
    {
        int index = grade - 1;
        level = Rods[index].Level;
        req_Gold = Rods[index].Req_Gold;
    }
    
    /// <summary>
    /// 배 레벨 데이터 읽어오는 함수
    /// </summary>
    /// <param name="grade">현재 레벨 입력</param>
    /// <param name="level">SO에 입력 된 레벨 데이터</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetShipLevelData(int grade, out int level, out int req_Gold)
    {
        int index = grade - 1;
        level = Ships[index].Level;
        req_Gold = Ships[index].Req_Gold;
    }
}
