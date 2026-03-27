using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DiningUpgradeData", menuName = "UpgradeDataSO/DiningUpgradeData")]
public class DiningUpgradeData : ScriptableObject, IDataSeter
{
    [field: SerializeField] public EDiningUpgradeType FishingUpgradeType { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public int Effect_Value_1 { get; private set; }
    [field: SerializeField] public float Effect_Value_2 { get; private set; }


    public void SetData(string[] datas)
    {
        Enum.TryParse(datas[0], out EDiningUpgradeType d0);
        int.TryParse(datas[1], out int d1);
        int.TryParse(datas[2], out int d2);
        int.TryParse(datas[3], out int d3);
        float.TryParse(datas[4], out float d4);
        
        FishingUpgradeType = d0;
        Level = d1;
        Cost = d2;
        Effect_Value_1 = d3;
        Effect_Value_2 = d4;
    }
}
