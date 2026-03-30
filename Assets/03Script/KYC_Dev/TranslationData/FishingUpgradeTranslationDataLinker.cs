using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishingUpgradeTranslationDataLinker", menuName = "TranslationDataSO/FishingUpgradeTranslationDataLinker")]
public class FishingUpgradeTranslationDataLinker : ScriptableObject, IDataSeter
{
    public EFishingUpgradeType FishingUpgradeType;
    public string Upgrade_Title_ID;
    public string Upgrade_Description_ID;
    public string Tooltip_Format_ID;
    public string Tooltip_Format_Max_ID;
    
    [Header("Not Use")]
    public string Value_Type;
    public int Max_Level;
    
    public void SetData(string[] datas)
    {
        Enum.TryParse(datas[0], out EFishingUpgradeType d0);
        int.TryParse(datas[1], out int d6);

        FishingUpgradeType = d0;
        Value_Type = datas[1];
        Upgrade_Title_ID = datas[2];
        Upgrade_Description_ID = datas[3];
        Tooltip_Format_ID = datas[4];
        Tooltip_Format_Max_ID = datas[5];
        Max_Level = d6;
    }
}
