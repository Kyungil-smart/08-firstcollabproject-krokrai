using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DiningTranslationData", menuName = "TranslationDataSO/DiningTranslationData")]
public class DiningTranslationData : ScriptableObject, IDataSeter
{
    public int Id;
    public string Kor;
    public string En;
    
    public void SetData(string[] datas)
    {
        int.TryParse(datas[0], out int d0);
        
        Id = d0;
        Kor = datas[1];
        En = datas[2];
    }
}