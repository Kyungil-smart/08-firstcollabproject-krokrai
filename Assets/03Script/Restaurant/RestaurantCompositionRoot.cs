using UnityEngine;

public class RestaurantCompositionRoot : MonoBehaviour
{
    [SerializeField] private DataTower _dataTower;
    [SerializeField] private RestaurantManager _restarurant;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _restarurant.ConnectDataTower(_dataTower);
    }
}
