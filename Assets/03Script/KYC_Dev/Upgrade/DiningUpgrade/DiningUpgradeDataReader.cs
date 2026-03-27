using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DiningUpgradeDataReader : MonoBehaviour
{
    [Header("Upgrade Data SO / index 0 = level 1")]
    public DiningUpgradeData[] Master_Lv;
    public DiningUpgradeData[] Max_Customer_Limit;
    public DiningUpgradeData[] Max_Spawn_Limit_1;
    public DiningUpgradeData[] Max_Spawn_Limit_2;
    public DiningUpgradeData[] Weight;
    public DiningUpgradeData[] Bonus_Tips_Multi;
    public DiningUpgradeData[] Bonus_Dish_Price_1;
    public DiningUpgradeData[] Bonus_Dish_Price_2;
    public DiningUpgradeData[] Bonus_Food_1;
    public DiningUpgradeData[] Bonus_Food_2;
    public DiningUpgradeData[] Unlock_Cat_Object;
    
    /// <summary>
    /// 마스터 레벨 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetMasterLevelCostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Master_Lv[index].Cost;
    }
    
    /// <summary>
    /// 좌석 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetMaxCustomerLimitCostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Max_Customer_Limit[index].Cost;
    }
    
    /// <summary>
    /// 특별 손님 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetMaxSpawnLimit01CostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Max_Spawn_Limit_1[index].Cost;
    }
    
    /// <summary>
    /// VIP 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetMaxSpawnLimit02CostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Max_Spawn_Limit_2[index].Cost;
    }
    
    /// <summary>
    /// 팁주는 손님 가중치 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetWeightCostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Weight[index].Cost;
    }
    
    /// <summary>
    /// 모금함(팁 액수 증가) 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetBonusTipsMultiCostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Bonus_Tips_Multi[index].Cost;
    }
    
    /// <summary>
    /// 계산대(요리 가격 증가) 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost"></param>
    public void GetBonusDishPrice01CostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Bonus_Dish_Price_1[index].Cost;
    }
    
    /// <summary>
    /// 밥솥(요리 가격 증가) 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetBonusDishPrice02CostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Bonus_Dish_Price_2[index].Cost;
    }
    
    /// <summary>
    /// 식칼(요리 개수 증가) 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetBonusFood01CostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Bonus_Food_1[index].Cost;
    }
    
    /// <summary>
    /// 도마(요리 개수 증가) 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetBonusFood02CostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Bonus_Food_2[index].Cost;
    }
    
    /// <summary>
    /// 고양이 업그레이드 필요 골드 읽어오는 함수
    /// </summary>
    /// <param name="level">현재 레벨 입력</param>
    /// <param name="cost">SO에 입력된 필요 골드 데이터</param>
    public void GetUnlockCatObjectCostData(int level, out int cost)
    {
        int index = level - 1;
        cost = Unlock_Cat_Object[index].Cost;
    }
}
