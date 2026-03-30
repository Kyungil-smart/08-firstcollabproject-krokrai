using System.Collections;
using UnityEngine;

public class RestaurantSeat : MonoBehaviour
{
    [SerializeField] private Restaurant_Fixed_value _fixedValue;
    
    public bool IsOccupied { get; private set; }
    public Vector3 SitPosition => this.transform.position;

    private WaitForSeconds _washDish;

    private void Awake()
    {
        _washDish = new WaitForSeconds(_fixedValue.dishWashTime);
    }

    public void SetOccupied()
    {
        IsOccupied = true;
    }
    public void ClearSeat() 
    {
        StartCoroutine(WashingDish());
    }

    IEnumerator WashingDish()
    {
        yield return _washDish;
        IsOccupied = false;
    }
}
