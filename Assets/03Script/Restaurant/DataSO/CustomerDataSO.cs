using JetBrains.Annotations;
using UnityEngine;

public enum CustomerGrade
{
    NA = -1, NORMAL, SPECIAL, VIP
}

[CreateAssetMenu(fileName = "CustomerDataSO - ", menuName = "Restaurant/CustomerData")]
public class CustomerDataSO : ScriptableObject, IDataSeter
{
    public int customer_ID;
    public CustomerGrade grade;
    public int weight;
    public float flow_Velocity;
    public int[] orderTime; // 3, 주문시간.
    public int[] EatDuration; // 3, 먹는 시간
    public float[] orderChans; // 2, 추가 주문 확률.

    public void SetData(string[] cols)
    {
        orderTime = new int[3];
        EatDuration = new int[3];
        orderChans = new float[2];

        cols[0].TryParseForSO(out customer_ID, this.name);
        cols[2].TryParseForSO(out weight);
        cols[3].TryParseForSO(out flow_Velocity);
        
        cols[4].TryParseForSO(out orderTime[0]);
        cols[5].TryParseForSO(out EatDuration[0]);
        cols[6].TryParseForSO(out orderChans[0]);

        cols[7].TryParseForSO(out orderTime[1]);
        cols[8].TryParseForSO(out EatDuration[1]);
        cols[9].TryParseForSO(out orderChans[1]);

        cols[10].TryParseForSO(out orderTime[2]);
        cols[11].TryParseForSO(out EatDuration[2]);

        switch (cols[1])
        {
            case "Normal":
                grade = CustomerGrade.NORMAL;
                break;
            case "Special":
                grade = CustomerGrade.SPECIAL;
                break;
            case "VIP":
                grade = CustomerGrade.VIP;
                break;
            default:
                grade = CustomerGrade.NA;
                break;
        }
    }
}
