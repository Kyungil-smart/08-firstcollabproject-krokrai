using UnityEngine;

public class RestaurantSeat : MonoBehaviour
{
    [SerializeField] private Transform _sitPoint;

    public bool IsOccupied { get; private set; }
    public CustomerController CurrentCustomer { get; private set; }

    public Vector3 SitPosition => _sitPoint.position;

    public void SetOccupied(CustomerController customer)
    {
        IsOccupied = true;
        CurrentCustomer = customer;
    }
    public void ClearSeat()
    {
        IsOccupied = false;
        CurrentCustomer = null;
    }
}
