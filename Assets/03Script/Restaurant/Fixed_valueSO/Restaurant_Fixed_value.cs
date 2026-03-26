using UnityEngine;

[CreateAssetMenu(fileName = "Restaurant_Fixed_valueSO", menuName = "Scriptable Objects/Restaurant_Fixed_valueSO")]
public class Restaurant_Fixed_value : ScriptableObject, IDataSeter
{
    public int dishWashTime;
    public int minSpawnDelay;
    public int maxSpawnDelay;

    public void SetData(string[] datas)
    {
        datas[0].TryParseForSO(out dishWashTime);
        datas[1].TryParseForSO(out minSpawnDelay);
        datas[2].TryParseForSO(out maxSpawnDelay);
    }
}
