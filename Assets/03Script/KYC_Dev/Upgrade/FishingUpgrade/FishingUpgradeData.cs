using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishingUpgradeData", menuName = "UpgradeData/FishingUpgradeData")]
public class FishingUpgradeData : ScriptableObject, IDataSeter
{
    [field: SerializeField] public EFishingUpgradeType FishingUpgradeType { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public int Req_Gold { get; private set; }
    [field: SerializeField] public int Apply_Value { get; private set; }
    [field: SerializeField] public string Rate_ID { get; private set; }
    [field: SerializeField] public string Sprite { get; private set; }


    public void SetData(string[] datas)
    {
        Enum.TryParse(datas[0], out EFishingUpgradeType e);
        int.TryParse(datas[1], out int level);
        int.TryParse(datas[2], out int req_Gold);
        int.TryParse(datas[3], out int apply_Value);
        
        FishingUpgradeType = e;
        Level = level;
        Req_Gold = req_Gold;
        Apply_Value = apply_Value;
        Rate_ID = datas[4];
        Sprite = datas[5];
    }
}
