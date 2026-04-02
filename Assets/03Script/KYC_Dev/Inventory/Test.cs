using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] InventorySystem _system;
    
    public void OnClickAddFish1()
    {
        _system.Insert("Fish_1");
    }
    
    public void OnClickAddFish2()
    {
        _system.Insert("Fish_2");
    }
    
    public void OnClickDeleteFish1()
    {
        _system.Erase("Fish_1");
    }
    
    public void OnClickDeleteFish2()
    {
        _system.Erase("Fish_2");
    }
    
    public void OnClickExtend()
    {
        _system.InventoryExtend(10);
    }
}
