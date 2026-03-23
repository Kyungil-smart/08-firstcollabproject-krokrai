using UnityEngine;

[CreateAssetMenu(fileName = "CustomerDataSO - ", menuName = "Restaurant/CustomerData")]
public class CustomerDataSO : ScriptableObject, IDataSeter
{
    public int Id;
    public string CustomerName;
    public float MoveSpeed;
    public float EatDuration;
    public int PriceScaleFactor;
    public float SecondEatChance;
    public float SpawnDelay;
    public void SetData(string[] cols)
    {
        Id = int.Parse(cols[0]);
        CustomerName = cols[1];
        MoveSpeed = float.Parse(cols[2]);
        EatDuration = float.Parse(cols[3]);
        PriceScaleFactor = int.Parse(cols[4]);
        SecondEatChance = float.Parse(cols[5]);
        SpawnDelay = float.Parse(cols[6]);
    }
}
