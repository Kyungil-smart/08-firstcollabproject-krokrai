using UnityEngine;

public class RestaurantSeat : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public Vector3 SitPosition => this.transform.position;

    public void SetOccupied()
    {
        IsOccupied = true;
    }
    public void ClearSeat()
    {
        IsOccupied = false;
    }
}
