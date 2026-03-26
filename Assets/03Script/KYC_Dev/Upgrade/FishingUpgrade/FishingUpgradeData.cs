using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishingUpgradeData", menuName = "UpgradeDataSO/FishingUpgradeData")]
public class FishingUpgradeData : ScriptableObject, IDataSeter
{
    [field: SerializeField] public EFishingUpgradeType FishingUpgradeType { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public int Req_Gold { get; private set; }
    [field: SerializeField] public int Apply_Value { get; private set; }
    [field: SerializeField] public string Rate_ID { get; private set; }


    public void SetData(string[] datas)
    {
        Enum.TryParse(datas[0], out EFishingUpgradeType d0);
        int.TryParse(datas[1], out int d1);
        int.TryParse(datas[2], out int d2);
        int.TryParse(datas[3], out int d3);
        
        FishingUpgradeType = d0;
        Level = d1;
        Req_Gold = d2;
        Apply_Value = d3;
        Rate_ID = datas[4];
    }
}
