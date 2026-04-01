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
    [SerializeField] private DiningUpgradeDataReader _diningUpgradeDataReader;

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
    private int _maxSpecialCustomersWeight;
    private int _maxVIPCustomersWeight;

    private int _halfNormalCustomersWeight;
    private int _halfSpecialCustomersWeight;

    private byte _currentSpawnedSpecialCustomer;
    private byte _currentSpawnedVIPCustomer;

    int _currentWeightLevel;
    int _temp_Numbers;

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
                    _maxSpecialCustomersWeight += _customerData[i].weight;
                    break;
                case CustomerGrade.VIP:
                    _maxVIPCustomersWeight += _customerData[i].weight;
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

    private void SpawnCustomer(RestaurantSeat seat)
    {
        // 자리에 앉음 상태로 전환
        _currentWeightLevel = DataTower.instance.WeightLevel;
        _temp_Numbers = CustomerWeightSelecter(UnityEngine.Random.Range(0,
            (_maxNormalCustomersWeight
            + CanSpawnVIPCustomer()
            + CanSpawnSpecialCustomer())));

        seat.SetOccupied();
        _randomPrefab.SetActive(true);
        _randomPrefab.transform.position = _spawnPointRight.position;
        _randomPrefab.GetComponent<CustomerController>().SetInfo(seat,
            _exitPointLeft,
            _customerTips[(int)_customerData[_temp_Numbers].grade],
            _customerData[_temp_Numbers],
            _canVisual); // 후에 수정
    }

    private int CanSpawnSpecialCustomer()
    {
        if (_currentSpawnedSpecialCustomer < DataTower.instance.MaxSpawnLimit01Level - 1)
        {
            return _maxSpecialCustomersWeight + (DataTower.instance.MaxSpawnLimit01Level - 1) * _diningUpgradeDataReader.Weight[_currentWeightLevel].Effect_Value_1;
        }
        else
            return 0;
    }

    private int CanSpawnVIPCustomer()
    {
        if (_currentSpawnedVIPCustomer < DataTower.instance.MaxSpawnLimit02Level - 1)
        {
            return _maxVIPCustomersWeight;
        }
        else
            return 0;
    }

    private int CustomerWeightSelecter(int weight)
    {
        // weight 업글시 스페셜 손님 가중치
        _maxSpecialCustomersWeight = _maxSpecialCustomersWeight 
            + (_diningUpgradeDataReader.Weight[_currentWeightLevel].Effect_Value_1
            * _specialCustomers);

        if (weight - _maxNormalCustomersWeight < 0)
        {
            // 노말 손님들만 존재(0보다 작으니)
            Debug.Log("Normal Spawn");
            if (weight - _halfNormalCustomersWeight < 0)
            {
                return CustomerWeightFinder(weight, 0);
            }
            else
            {
                weight -= _halfNormalCustomersWeight;
                return CustomerWeightFinder(weight, (byte)(_normalCustomers / 2));
            }
        }
        else
        {
            // 그 외 스페셜 및 vip
            weight -= _maxNormalCustomersWeight;
            if (weight - _maxSpecialCustomersWeight < 0 && DataTower.instance.MaxSpawnLimit01Level > 1)
            {
                // 스페셜만
                Debug.Log("Special Spawn");
                _currentSpawnedSpecialCustomer++;
                if ( weight - _halfSpecialCustomersWeight < 0)
                    return CustomerWeightFinder(weight, _normalCustomers);
                else
                {
                    weight -= _halfSpecialCustomersWeight;
                    return CustomerWeightFinder(weight, (byte)((_normalCustomers + _specialCustomers) / 2));
                }
            }
            else
            {
                _currentSpawnedVIPCustomer++;
                Debug.Log("VIP Spawn");
                return CustomerWeightFinder(weight, (byte)(_normalCustomers + _specialCustomers) );
            }
        }
    }

    private int CustomerWeightFinder(int weight, byte startNumber)
    { // 가중치를 빼면서 작업 하기
        startNumber -= 1;
        for (byte i = startNumber; i < _customerData.Length; i++)
        {
            if (weight - _customerData[i].weight <= 0)
            {
                return i;
            }
            else
            {
                weight -= _customerData[i].weight;
            }
        }
        return -1;
    }

    /// <summary>
    /// CustomerController 기반 오브젝트를 object pool로 반환.
    /// </summary>
    /// <param name="obj"></param>
    public void DeSpawnCustomer(GameObject obj, CustomerGrade grade)
    {
        // 비용은 비싸지만 프레임마다 호출이 아니니 괜찮을 듯한 생각.
        if ( !(obj != null && obj.GetComponent<CustomerController>() is CustomerController))
        {
            Debug.Log("잘 못된 반환 형식");
            return;
        }

        switch(grade)
        {
            case CustomerGrade.SPECIAL:
                _currentSpawnedSpecialCustomer--;
                break;
            case CustomerGrade.VIP:
                _currentSpawnedVIPCustomer--;
                break;
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
        DataTower.instance.TryMoenyChanged(
            (ulong)(
            (_menuCtrl.RandomEating()
                * (1 
                    + _diningUpgradeDataReader.Bonus_Dish_Price_1[DataTower.instance.BonusDishPrice01Level].Effect_Value_1 // 호출 횟수가 많지 않아서 임시 작업, 성능 문제 발생 시 수정 필요.
                    + _diningUpgradeDataReader.Bonus_Dish_Price_2[DataTower.instance.BonusDishPrice02Level].Effect_Value_2))
                * multi
                * DataTower.instance.BonusTipsMultiLevel
            ),
            false);
    }
}
