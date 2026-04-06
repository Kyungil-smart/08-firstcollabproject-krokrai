using UnityEngine;
using UnityEngine.UI;

public class CatEatFish : MonoBehaviour
{
    [SerializeField] InventorySystem _inven;
    [SerializeField] Button _btn;
    [SerializeField] Animation _ani;

    public void EatFish()
    {
        _btn.interactable = false;

        _ani.Play();
    }

    public void CanEatFishState()
    {
        _btn.interactable = true;
    }
}
