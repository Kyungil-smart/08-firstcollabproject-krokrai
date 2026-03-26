using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FishingUpgradeDataReader : MonoBehaviour
{
    [Header("Upgrade Data SO / index 0 = level 1")]
    public FishingUpgradeData[] Grades;
    public FishingUpgradeData[] Baits;
    public FishingUpgradeData[] Rods;
    public FishingUpgradeData[] Ships;

    /// <summary>
    /// 플레이어 낚시 등급 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetFishingGradeReqGoldData(int level, out int req_Gold)
    {
        int index = level - 1;
        req_Gold = Grades[index].Req_Gold;
    }
    
    /// <summary>
    /// 미끼 레벨 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetBaitLevelReqGoldData(int level, out int req_Gold)
    {
        int index = level - 1;
        req_Gold = Baits[index].Req_Gold;
    }
    
    /// <summary>
    /// 낚시대 레벨 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="level">SO에 입력 된 레벨 데이터</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetRodLevelReqGoldData(int level, out int req_Gold)
    {
        int index = level - 1;
        req_Gold = Rods[index].Req_Gold;
    }
    
    /// <summary>
    /// 배 레벨 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="grade">현재 레벨 입력</param>
    /// <param name="level">SO에 입력 된 레벨 데이터</param>
    /// <param name="req_Gold">SO에 입력 된 필요 골드 데이터</param>
    public void GetShipLevelReqGoldData(int level, out int req_Gold)
    {
        int index = level - 1;
        req_Gold = Ships[index].Req_Gold;
    }
}
