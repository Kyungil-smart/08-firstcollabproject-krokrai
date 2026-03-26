using UnityEngine;

[CreateAssetMenu(fileName = "FishRate", menuName = "Scriptable Objects/FishRate")]
public class FishRateData : ScriptableObject, IDataSeter
{
    public string gachaGroupID; // 유저 레벨
    
    public float trash, normal, fine, superior, rare, elite, fantastic, legendary; // 물고기 레어리티

    public void SetData(string[] datas)
    { 
        gachaGroupID = datas[0];
        trash = float.Parse(datas[1]);
        normal = float.Parse(datas[2]);
        fine = float.Parse(datas[3]);
        superior = float.Parse(datas[4]);
        rare = float.Parse(datas[5]);
        elite = float.Parse(datas[6]);
        fantastic = float.Parse(datas[7]);
        legendary = float.Parse(datas[8]);
    }
}
