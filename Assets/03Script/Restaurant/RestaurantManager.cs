using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    // 후에 아래 리소스는 정리 및 DataTower로 인계
    [Header("Resources")]
    // 후에 메뉴판이랑 연동해서 관리할 것이기 때문에 메뉴판 완성 시 함수 안에 포함된 모든 _sushiCount 수정 해야함 @@@@@@@@@@@@@@@@@@@@@@@
    [SerializeField] private MenuCtrl _menuCtrl;
    [SerializeField] private CustomerDataSO[] _customerData;
    [SerializeField] private Customer_Tips[] _customerTips;
    [SerializeField] private Restaurant_Fixed_value _fixedValue;

    [Header("Spawn")]
    [SerializeField] private GameObject[] _customerPrefab;
    [SerializeField] private GameObject _customers;
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private Transform _exitPointLeft;

    [Header("Seats")]
    [SerializeField] private List<RestaurantSeat> _seats = new List<RestaurantSeat>();

    [Header("SceneChange")]
    [SerializeField] private OpenMenu _openMenu;
    private Queue<GameObject> _customerPool; // 오브젝트 풀 패턴 기반 작동 예정
    private GameObject _randomPrefab;

    private RestaurantSeat _emptySeat;
    private bool _haveDish;
    private bool _canVisual;

    private WaitForSeconds[] _seconds;

    private byte _normalCustomers;
    private byte _specialCustomers;

    private int _maxNormalCustomersWeight;
    private int _halfNormalCustomersWeight;
    private int _halfSpecialCustomersWeight;


    private void OnEnable()
    {
        _menuCtrl.OnDish += HaveDish;
        _openMenu.OnChangeSceneToRestaurant += OnVisual;
    }

    private void OnDestroy()
    {
        _menuCtrl.OnDish += HaveDish;
        _openMenu.OnChangeSceneToRestaurant -= OnVisual;
    }

    public void OnVisual(bool b)
    {
        _canVisual = b;
        Debug.Log($"시각 효과 상태 : {_canVisual}");
    }

    public void HaveDish(bool b)
    {
        _haveDish = b;
    }

    private void Start()
    {
        _haveDish = false;
        byte i = 0;
        _halfNormalCustomersWeight = 0;
        _halfSpecialCustomersWeight = 0;
        /*
        _baseDelay = new WaitForSeconds(5);
        _seconds = new WaitForSeconds[10];
        for (int i = 1; i < 11; i++)
        {
            _seconds[i] = new WaitForSeconds(i);
        }
        */

        _customerPool = new Queue<GameObject>(10);

        for (i = 0; i < 10 ; i++)
        {
            GameObject obj = Instantiate(_customerPrefab[0]); // 나중에 데이터 타워 리셋 후 받게 만들기.
            obj.GetComponent<CustomerController>().ConnectRestaurant(this, _openMenu);
            obj.transform.SetParent(_customers.transform);
            obj.name = $"customer {i}";
            obj.SetActive(false);
            _customerPool.Enqueue( obj );
        }

        for(i = 0; i < _customerData.Length; i++)
        {
            switch(_customerData[i].grade)
            {
                case CustomerGrade.NA:
                    Debug.LogWarning("값 오류");
                    break;
                case CustomerGrade.NORMAL:
                    _normalCustomers++;
                    _maxNormalCustomersWeight += _customerData[i].weight;
                    break;
                case CustomerGrade.SPECIAL:
                    _specialCustomers++;
                    break;
                case CustomerGrade.VIP:
                    break;
            }
        }

        for (i = 0; i  < _normalCustomers / 2; i++)
            _halfNormalCustomersWeight += _customerData[i].weight;
        for (i = 0; i < _specialCustomers / 2; i++)
            _halfSpecialCustomersWeight += _customerData[i + _normalCustomers].weight;

        StartCoroutine(CoTrySpawnCustomer());
    }

    private IEnumerator CoTrySpawnCustomer()
    {

        // 설거지? 시간 => 대기시간 => 스폰
        while (true)
        {
            if (!_haveDish)
            {
                yield return null;
                continue;
            }

            // 빈자리 탐색 및 반환 받음.
            _emptySeat = GetEmptySeat();
            // 자리가 없는 경우 예외처리
            if (_emptySeat != null)
            {
                // 스폰 대기 시간.
                yield return new WaitForSeconds(UnityEngine.Random.Range(_fixedValue.minSpawnDelay, _fixedValue.maxSpawnDelay + 1));

                // 손님 성향 및 이미지 랜덤 생성
                GetRandomCustomerPrefab();

                // 예외처리.
                if (_randomPrefab == null)
                    yield return null;
                // 후에 추가 딜레이 필요

                // 최종적으로 빈자리와 손님을 소환
                SpawnCustomer(_emptySeat);
            }
            else
            {
                yield return null;
            }
        }
    }

    // 받아온 정보를 기반으로 손님 생성
    /*
    private void SpawnCustomer(RestaurantSeat seat, CustomerController prefab)
    {
        // 이 방식은 object pool로 교체 필요.
        CustomerController customer = Instantiate(prefab, _spawnPointRight.position, Quaternion.identity);
        
        // 자리에 앉음 상태로 전환
        seat.SetOccupied(customer);
        // 손님 설정 초기화
        customer.SetInfo(this, seat, _exitPointLeft);
    }
    */

    private void SpawnCustomer(RestaurantSeat seat)
    {
        // 자리에 앉음 상태로 전환
        seat.SetOccupied();
        _randomPrefab.SetActive(true);
        _randomPrefab.transform.position = _spawnPointRight.position;
        _randomPrefab.GetComponent<CustomerController>().SetInfo(seat, _exitPointLeft, _customerTips[0], _customerData[0], _canVisual); // 후에 수정
    }

    private void CustomerWeightSelecter()
    {

    }

    /// <summary>
    /// CustomerController 기반 오브젝트를 object pool로 반환.
    /// </summary>
    /// <param name="obj"></param>
    public void DeSpawnCustomer(GameObject obj)
    {
        // 비용은 비싸지만 프레임마다 호출이 아니니 괜찮을 듯한 생각.
        if ( !(obj != null && obj.GetComponent<CustomerController>() is CustomerController))
        {
            Debug.Log("잘 못된 반환 형식");
            return;
        }
            
        obj.SetActive(false);
        _customerPool.Enqueue(obj);
    }

    private RestaurantSeat GetEmptySeat()
    {
        for (int i = 0; i < DataTower.instance.MaxCustomerLimitLevel; i++)
        {
            // 반복 문으로 모든 자리 탐색
            if (_seats[i].IsOccupied == false)
                return _seats[i];
        }
        return null;
    }

    // 입력된 손님 무작위  출력
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
            Debug.LogWarning("프리팹이 등록되지 않았거나, 없습니다.");
            return;
        }
        int randIndex = Random.Range(0, _customerPrefab.Length);
        if (_customerPool.Count == 0)
        {
            _customerPool.Enqueue(_customerPrefab[randIndex]);
            _randomPrefab = _customerPool.Dequeue();
        }
        else
            _randomPrefab = _customerPool.Dequeue();
    }

    /// <summary>
    /// 초밥 갯수를 체크하고 초밥이 있으면 갯수를 하나 줄이고 초밥 가격을 자산에 추가한다.
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public void TryCounsumeSushiAndEarnMoney(float multi)
    {// 기본 가격 = RandomEating // 현재 비용 높으니 후에 Action으로 전환 조치 필요.
        DataTower.instance.TryMoenyChanged((ulong)((_menuCtrl.RandomEating()
            * (1+DataTower.instance.BonusDishPrice01Level+DataTower.instance.BonusDishPrice02Level))
            * multi),
            false);
    }
}
