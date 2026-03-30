using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TranslationData", menuName = "TranslationDataSO/TranslationData")]
public class TranslationData : ScriptableObject, IDataSeter
{
    public string Id;
    public string Kor;
    public string En;
    
    public void SetData(string[] datas)
    {
        Id = datas[0];
        Kor = datas[1];
        En = datas[2];
    }
}
