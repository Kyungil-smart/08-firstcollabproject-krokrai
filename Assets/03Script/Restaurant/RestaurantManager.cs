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
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private Transform _exitPointLeft;

    [Header("Seats")]
    [SerializeField] private List<RestaurantSeat> _seats = new List<RestaurantSeat>();

    private DataTower _dataTower;

    private Queue<GameObject> _customerPool; // 오브젝트 풀 패턴 기반 작동 예정
    private GameObject _randomPrefab;

    private RestaurantSeat _emptySeat;
    private bool haveDish;

    private Coroutine _spawnCo;

    private WaitForSeconds[] _seconds;
    private WaitForSeconds _baseDelay;

    private void OnEnable()
    {
        _menuCtrl.OnDish += HaveDish;
    }

    public void HaveDish(bool b)
    {
        haveDish = b;
    }

    private void Start()
    {
        haveDish = false;
        /*
        _baseDelay = new WaitForSeconds(5);
        _seconds = new WaitForSeconds[10];
        for (int i = 1; i < 11; i++)
        {
            _seconds[i] = new WaitForSeconds(i);
        }
        */
        _baseDelay = new WaitForSeconds(_fixedValue.dishWashTime);

        _customerPool = new Queue<GameObject>(8);
        for (int i = 0; i < 8 ; i++)
        {
            GameObject obj = Instantiate(_customerPrefab[0]);
            obj.GetComponent<CustomerController>().ConnectRestaurant(this);
            obj.name = $"customer {i}";
            obj.SetActive(false);
            _customerPool.Enqueue( obj );
        }

        _spawnCo = StartCoroutine(CoTrySpawnCustomer());
    }

    /// <summary>
    /// 데이터 관리자에게 연결하기 위한 함수
    /// RestaurantCompositionRoot.cs 를 제외하고 호출하는 스크립트가 없어야합니다.
    /// </summary>
    /// <param name="tower"></param>
    public void ConnectDataTower(DataTower tower)
    {
        if (_dataTower != null)
        {
            Debug.Log($"초기화된 변수에 접근 시도가 있습니다. {gameObject.name}");
            return;
        }
        _dataTower = tower;
    }

    private IEnumerator CoTrySpawnCustomer()
    {

        // 설거지? 시간 => 대기시간 => 스폰
        while (true)
        {
            if (!haveDish)
            {
                yield return null;
                continue;
            }
            // 손님 성향 및 이미지 랜덤 생성
            GetRandomCustomerPrefab();
            // 예외처리.
            if (_randomPrefab == null)
                yield break;

            // 스폰 대기 시간.
            //yield return _baseDelay ;
            yield return _baseDelay;
            yield return new WaitForSeconds(UnityEngine.Random.Range(_fixedValue.minSpawnDelay, _fixedValue.maxSpawnDelay +1));
            // 후에 추가 딜레이 필요

            // 빈자리 탐색 및 반환 받음.
            _emptySeat = GetEmptySeat();

            // 자리가 없는 경우 예외처리
            if (_emptySeat == null) continue;

            // 최종적으로 빈자리와 손님을 소환
            SpawnCustomer(_emptySeat);
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
        _randomPrefab.GetComponent<CustomerController>().SetInfo(seat, _exitPointLeft, _customerTips[0], _customerData[0]); // 후에 수정
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
        for (int i = 0; i < _seats.Count; i++)
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
    {
        DataTower.instance.TryMoenyChanged((ulong)(_menuCtrl.RandomEating()*multi),false);
    }
}
