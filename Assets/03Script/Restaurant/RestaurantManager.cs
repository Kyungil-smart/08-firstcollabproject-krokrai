using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int _sushiCount;
    [SerializeField] private float _money = 10000f;

    [Header("Spawn")]
    [SerializeField] private CustomerController[] _customerPrefab;
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private Transform _exitPointLeft;

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
            CustomerController randomPrefab = GetRandomCustomerPrefab();
            if (randomPrefab == null)
                yield break;

            yield return new WaitForSeconds(randomPrefab.SpawnDelay());

            if (_sushiCount <= 0) continue;

            RestaurantSeat emptySeat = GetEmptySeat();
            if (emptySeat == null) continue;

            SpawnCustomer(emptySeat, randomPrefab);
        }
    }
    private void SpawnCustomer(RestaurantSeat seat, CustomerController prefab)
    {
        CustomerController customer = Instantiate(prefab, _spawnPointRight.position, Quaternion.identity);
        seat.SetOccupied(customer);
        customer.SetInfo(this, seat, _exitPointLeft);
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
    private CustomerController GetRandomCustomerPrefab()
    {
        if (_customerPrefab == null || _customerPrefab.Length == 0)
            return null;

        int randIndex = Random.Range(0, _customerPrefab.Length);
        return _customerPrefab[randIndex];
    }

    public bool HasSushi() => _sushiCount > 0;
    /// <summary>
    /// 초밥 갯수를 체크하고 초밥이 있으면 갯수를 하나 줄이고 초밥 가격을 자산에 추가한다.
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool TryCounsumeSushiAndEarnMoney(int price)
    {
        if (_sushiCount <= 0) return false;

        _sushiCount--;
        _money += price;
        return true;
    }
    /// <summary>
    /// 초밥의 갯수를 추가하는 함수
    /// 요리가 끝났을 때 추가하면 된다.
    /// </summary>
    /// <param name="amount"></param>
    public void AddSushi(int amount) => _sushiCount += amount;
}
