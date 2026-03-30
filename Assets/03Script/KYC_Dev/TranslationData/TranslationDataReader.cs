using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TranslationDataReader : MonoBehaviour
{
    [SerializeField] TranslationData[] _translationDatas;
    [SerializeField] private FishingUpgradeTranslationDataLinker[] _fTDataLinkers;
    [SerializeField] private DiningUpgradeTranslationDataLinker[] _dTDataLinkers;
    
    private Dictionary<string, TranslationData> _translationDictionary = new();
    private Dictionary<EFishingUpgradeType, FishingUpgradeTranslationDataLinker> _fTDataLinkerDictionary = new ();
    private Dictionary<EDiningUpgradeType, DiningUpgradeTranslationDataLinker> _dTDataLinkerDictionary = new ();
    

    private void Awake()
    {
        foreach (TranslationData data in _translationDatas)
        {
            _translationDictionary.TryAdd(data.Id, data);
        }

        foreach (FishingUpgradeTranslationDataLinker linker in _fTDataLinkers)
        {
            _fTDataLinkerDictionary.TryAdd(linker.FishingUpgradeType, linker);
        }

        foreach (DiningUpgradeTranslationDataLinker linker in _dTDataLinkers)
        {
            _dTDataLinkerDictionary.TryAdd(linker.DiningUpgradeType, linker);
        }
    }

    /// <summary>
    /// 키값을 넣으면 번역 데이터 SO를 찾아줌
    /// </summary>
    /// <param name="key">키</param>
    /// <returns>번역 데이터 SO</returns>
    public TranslationData GetTranslationData(string key)
    {
        return _translationDictionary[key];
    }

    /// <summary>
    /// 낚시 업그레이드의 번역 데이터 ID를 어떤것을 불러올지 찾아줌
    /// </summary>
    /// <param name="key">키</param>
    /// <param name="index">1 = Upgrade_Title / 2 = Simple_Description / 3 = Tooltip_Format_ID / 4 = Tooltip_Format_Max_ID</param>
    /// <returns>번역 데이터 ID</returns>
    public string GetFishingTranslationID(EFishingUpgradeType key, int index)
    {
        FishingUpgradeTranslationDataLinker temp = _fTDataLinkerDictionary[key];
        switch (index)
        {
            case 1:
                return temp.Upgrade_Title_ID;
            case 2:
                return temp.Upgrade_Description_ID;
            case 3:
                return temp.Tooltip_Format_ID;
            case 4:
                return temp.Tooltip_Format_Max_ID;
            default:
                Debug.LogWarning("<color=red>잘못된 인덱스 접근</color>");
                return "";
        }
    }

    /// <summary>
    /// 식당 업그레이드의 번역 데이터 ID를 어떤것을 불러올지 찾아줌
    /// </summary>
    /// <param name="key">키</param>
    /// <param name="index">1 = Upgrade_Title / 2 = Simple_Description / 3 = Tooltip_Format_ID / 4 = Tooltip_Format_Max_ID</param>
    /// <returns>번역 데이터 ID</returns>
    public string GetDiningTranslationID(EDiningUpgradeType key, int index)
    {
        DiningUpgradeTranslationDataLinker temp = _dTDataLinkerDictionary[key];
        switch (index)
        {
            case 1:
                return temp.Upgrade_Title_ID;
            case 2:
                return temp.Upgrade_Description_ID;
            case 3:
                return temp.Tooltip_Format_ID;
            case 4:
                return temp.Tooltip_Format_Max_ID;
            default:
                Debug.LogWarning("<color=red>잘못된 인덱스 접근</color>");
                return "";
        }
    }
}
