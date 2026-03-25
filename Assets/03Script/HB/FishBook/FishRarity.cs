using UnityEngine;

public enum FishRate
{
    Trash,
    Normal,
    Fine,
    Superior,
    Rare,
    Elite,
    Fantastic,
    Legendary
}
public class FishRarity : MonoBehaviour
{
    public FishRate fishRate;
    public int price;
}
