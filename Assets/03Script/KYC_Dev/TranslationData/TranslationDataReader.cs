using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TranslationDataReader : MonoBehaviour
{
    [SerializeField] FishingTranslationData[] _fishingTranslationDatas;
    [SerializeField] DiningTranslationData[] _diningTranslationDatas;
    
    private Dictionary<string, FishingTranslationData> _fishingTranslationDictionary = new();
    private Dictionary<int, DiningTranslationData> _diningTranslationDictionary = new();

    private void Awake()
    {
        foreach (FishingTranslationData data in _fishingTranslationDatas)
        {
            _fishingTranslationDictionary.Add(data.Id, data);
        }
        foreach (DiningTranslationData data in _diningTranslationDatas)
        {
            _diningTranslationDictionary.Add(data.Id, data);
        }
    }

    /// <summary>
    /// 키 값을 넣으면 번역 데이터를 찾아줌(낚시 번역)
    /// </summary>
    /// <param name="key">키</param>
    /// <param name="translationData">번역 데이터 SO</param>
    public void GetFTranslationData(string key, out FishingTranslationData translationData)
    {
        translationData = _fishingTranslationDictionary[key];
    }
    
    /// <summary>
    /// 키 값을 넣으면 번역 데이터를 찾아줌(식당 번역)
    /// </summary>
    /// <param name="key">키</param>
    /// <param name="translationData">번역 데이터 SO</param>
    public void GetDTranslationData(int key,out DiningTranslationData translationData)
    {
        translationData = _diningTranslationDictionary[key];
    }
    
}
