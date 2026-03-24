using UnityEngine;

public class RestaurantCompositionRoot : MonoBehaviour
{
    [SerializeField] private DataTower _dataTower;
    [SerializeField] private RestaurantManager _restarurant;
    [SerializeField] private CustomerController _customer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _restarurant.ConnectDataTower(_dataTower);
        _customer.ConnectRestaurant(_restarurant);
    }
}
