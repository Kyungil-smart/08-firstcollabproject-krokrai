using UnityEngine;

[CreateAssetMenu(fileName = "FishRate", menuName = "Scriptable Objects/FishRate")]
public class FishRateData : ScriptableObject
{
    public string gachaGroupID; // 유저 레벨
    // 아래는 레어리티
    public float trash;
    public float normal;
    public float fine;
    public float superior;
    public float rare;
    public float elite;
    public float fantastic;
    public float legendary;
}
