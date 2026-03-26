using UnityEngine;

[CreateAssetMenu(fileName = "Customer_Tips", menuName = "Scriptable Objects/Customer_Tips")]
public class Customer_Tips : ScriptableObject, IDataSeter
{
    public CustomerGrade customerGrade;
    public float tipsRate;
    public float tipsMulti;

    public void SetData(string[] datas)
    {
        datas[1].TryParseForSO(out tipsRate);
        datas[2].TryParseForSO(out tipsMulti);

        switch (datas[0])
        {
            case "Normal":
                customerGrade = CustomerGrade.NORMAL;
                break;
            case "Special":
                customerGrade = CustomerGrade.SPECIAL;
                break;
            case "VIP":
                customerGrade = CustomerGrade.VIP;
                break;
            default:
                customerGrade = CustomerGrade.NA;
                break;
        }
    }
}
