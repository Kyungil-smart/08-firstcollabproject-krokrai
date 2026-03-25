using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int _sushiCount;
    [SerializeField] private ulong _money = 10000;

    [Header("Spawn")]
    [SerializeField] private GameObject[] _customerPrefab;
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private Transform _exitPointLeft;

    [Header("Seats")]
    [SerializeField] private List<RestaurantSeat> _seats = new List<RestaurantSeat>();

    private DataTower _dataTower;

    private Queue<GameObject> _customerPool; // ������Ʈ Ǯ ���� ��� �۵� ����
    private GameObject _randomPrefab;
    RestaurantSeat emptySeat;

    private Coroutine _spawnCo;

    private WaitForSeconds[] _seconds;
    private WaitForSeconds _baseDelay;

    private void Start()
    {
        _baseDelay = new WaitForSeconds(5);
        _seconds = new WaitForSeconds[10];
        for (int i = 1; i < 11; i++)
        {
            _seconds[i] = new WaitForSeconds(i);
        }
        _customerPool = new Queue<GameObject>(8);
        _spawnCo = StartCoroutine(CoTrySpawnCustomer());
    }

    /// <summary>
    /// ������ �����ڿ��� �����ϱ� ���� �Լ�
    /// RestaurantCompositionRoot.cs �� �����ϰ� ȣ���ϴ� ��ũ��Ʈ�� ������մϴ�.
    /// </summary>
    /// <param name="tower"></param>
    public void ConnectDataTower(DataTower tower)
    {
        _dataTower = tower;
    }

    private IEnumerator CoTrySpawnCustomer()
    {

        // ������? �ð� => ���ð� => ����
        while (true)
        {
            // �մ� ���� �� �̹��� ���� ����
            GetRandomCustomerPrefab();
            // ����ó��.
            if (_randomPrefab == null)
                yield break;

            // ���� ��� �ð�.
            yield return _baseDelay ;

            // �ʹ��� ���� �ܿ� ����ó��
            if (_sushiCount <= 0) continue;

            // ���ڸ� Ž�� �� ��ȯ ����.
            emptySeat = GetEmptySeat();

            // �ڸ��� ���� ��� ����ó��
            if (emptySeat == null) continue;

            // ���������� ���ڸ��� �մ��� ��ȯ
            SpawnCustomer(emptySeat);
        }
    }

    // �޾ƿ� ������ ������� �մ� ����
    /*
    private void SpawnCustomer(RestaurantSeat seat, CustomerController prefab)
    {
        // �� ����� object pool�� ��ü �ʿ�.
        CustomerController customer = Instantiate(prefab, _spawnPointRight.position, Quaternion.identity);
        
        // �ڸ��� ���� ���·� ��ȯ
        seat.SetOccupied(customer);
        // �մ� ���� �ʱ�ȭ
        customer.SetInfo(this, seat, _exitPointLeft);
    }
    */
    private void SpawnCustomer(RestaurantSeat seat)
    {
        // �ڸ��� ���� ���·� ��ȯ
        seat.SetOccupied();
        
        // �մ� ���� �ʱ�ȭ
        //customer.SetInfo(this, seat, _exitPointLeft);
    }

    private RestaurantSeat GetEmptySeat()
    {
        for (int i = 0; i < _seats.Count; i++)
        {
            // �ݺ� ������ ��� �ڸ� Ž��
            if (_seats[i].IsOccupied == false)
                return _seats[i];
        }
        return null;
    }

    // �Էµ� �մ� ������  ���
    /*
    private CustomerController GetRandomCustomerPrefab()
    {
        if (_customerPrefab == null || _customerPrefab.Length == 0)
            return null;

        int randIndex = Random.Range(0, _customerPrefab.Length);
        return _customerPrefab[randIndex];
    }*/

    private void GetRandomCustomerPrefab()
    {
        if (_customerPrefab == null || _customerPrefab.Length == 0)
        {
            Debug.LogWarning("�������� ��ϵ��� �ʾҰų�, �����ϴ�.");
            return;
        }
        int randIndex = Random.Range(0, _customerPrefab.Length);
        if (_customerPool.Count == 0)
            _customerPool.Enqueue(_customerPrefab[randIndex]);
        else
            _randomPrefab = _customerPool.Dequeue();
    }

    public bool HasSushi() => _sushiCount > 0;

    /// <summary>
    /// �ʹ� ������ üũ�ϰ� �ʹ��� ������ ������ �ϳ� ���̰� �ʹ� ������ �ڻ꿡 �߰��Ѵ�.
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool TryCounsumeSushiAndEarnMoney(int price)
    {
        if (_sushiCount <= 0) return false;

        _sushiCount--;
        _money += (ulong)price;
        return true;
    }

    /// <summary>
    /// �ʹ��� ������ �߰��ϴ� �Լ�
    /// �丮�� ������ �� �߰��ϸ� �ȴ�.
    /// </summary>
    /// <param name="amount"></param>
    public void AddSushi(int amount) => _sushiCount += amount;
}
