using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int _sushiCount;
    [SerializeField] private float _money = 10000f;

    [Header("Spawn")]
    [SerializeField] private CustomerController _customerPrefab;
    [SerializeField] private Transform _spwnPointRight;
    [SerializeField] private Transform _exitPointLeft;
    [SerializeField] private float _spawnInterval = 3f;

    [Header("Seats")]
    [SerializeField] private List<RestaurantSeat> _seats = new List<RestaurantSeat>();

    private Coroutine _spawnCo;

    private void Start()
    {
        _spawnCo = StartCoroutine(CoTrySpawnCustomer());
    }

    private IEnumerator CoTrySpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (_sushiCount <= 0) continue;

            RestaurantSeat emptySeat = GetEmptySeat();
            if (emptySeat == null) continue;

            SpawnCustomer(emptySeat);
        }
    }
    private void SpawnCustomer(RestaurantSeat seat)
    {
        CustomerController customer = Instantiate(_customerPrefab, _spwnPointRight.position, Quaternion.identity);
        customer.SetInfo(this, seat, _exitPointLeft);
        seat.SetOccupied(customer);
    }

    private RestaurantSeat GetEmptySeat()
    {
        for (int i = 0; i < _seats.Count; i++)
        {
            if (_seats[i].IsOccupied == false)
                return _seats[i];
        }
        return null;
    }

    public bool HasSushi() => _sushiCount > 0;

    public bool TryCounsumeSushiAndEarnMoney(int price)
    {
        if (_sushiCount <= 0) return false;

        _sushiCount--;
        _money += price;
        return true;
    }

    public void AddSushi(int amount) => _sushiCount += amount;
}
