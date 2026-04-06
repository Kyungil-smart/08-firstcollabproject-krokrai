using UnityEngine;

public class CatEatMotion : MonoBehaviour
{
    [SerializeField] CatEatFish _cat;

    public void MoreFish()
    {
        _cat.CanEatFishState();
    }
}
