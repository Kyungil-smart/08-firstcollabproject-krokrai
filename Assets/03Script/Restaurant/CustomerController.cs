using System.Collections;
using UnityEngine;
using static Define;

public class CustomerController : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;

    [Header("Data")]
    private CustomerDataSO _data;
    private Customer_Tips _tips;
    [SerializeField] private Restaurant_Fixed_value _fixedValue;
    [SerializeField] private GameObject _visualObject;
    [SerializeField] private OpenMenu _openMenu;

    private RestaurantManager _restaurant;
    private RestaurantSeat _seat;
    private Transform _exitPoint;

    private int _eatCounte;
    private byte _maxEatCount;
    bool _firstOrder;
    bool _isVisualContect;

    private float a;

    private CustomerState _state;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetVisual(bool b) // 여기에 낚시로 넘어가는 경우 Invoke 해줄 오브젝트 넣어 주기 및 체인 걸어주기.
    {
        if (!gameObject.activeSelf) return;
        _isVisualContect = b;
        if (_isVisualContect)
        {
            _visualObject.SetActive(_isVisualContect);
            switch (_state)
            {
                case CustomerState.Enter:
                case CustomerState.Exit:
                case CustomerState.MoveToSeat:
                    _sr.sortingOrder = -2;
                    _anim.Play("Walk");
                    break;
                case CustomerState.Eat:
                    _sr.sortingOrder = -1;
                    _anim.Play("Sit");
                    break;
            }
        }
        else
        {
            _visualObject.SetActive(_isVisualContect);
        }
    }

    private void OnDestroy()
    {
        // 낚시로 넘어간 경우 해당 Invoke 해줄 대상에게 체인 걸어둔거 풀어주기.
        _openMenu.OnChangeSceneToRestaurant -= SetVisual;
    }

    /// <summary>
    /// RestaurangtManager와 연결.
    /// </summary>
    /// <param name="restaurant"></param>
    public void ConnectRestaurant(RestaurantManager restaurant, OpenMenu opmenu)
    {
        _restaurant = restaurant;
        _openMenu = opmenu;
        _openMenu.OnChangeSceneToRestaurant += SetVisual;
    }

    /// <summary>
    /// 손님의 기본 설정을 초기화 해주는 함수
    /// </summary>
    /// <param name="restaurant"></param>
    /// <param name="seat"></param>
    /// <param name="exitPoint"></param>
    public void SetInfo(RestaurantSeat seat, Transform exitPoint, Customer_Tips tip, CustomerDataSO so, bool canVisual)
    {
        _seat = seat;
        _exitPoint = exitPoint;
        _firstOrder = true;
        _data = so;
        _eatCounte = -1;

        a = _data.flow_Velocity;

        _isVisualContect = canVisual;
        SetVisual(_isVisualContect);

        _tips = tip;
        _maxEatCount = (byte)_data.orderChans.Length;

        _state = CustomerState.MoveToSeat;
        StartCoroutine(CoStateRoutine());
    }

    private IEnumerator CoStateRoutine()
    {
        while (true)
        {
            switch (_state)
            {
                case CustomerState.MoveToSeat:
                    // 레이어 뒤로 미루기
                    _sr.sortingOrder = -2;
                    // 애니메이션 출력
                    if (_isVisualContect) _anim.Play("Walk");
                    // 자리에 이동할때까지 대기
                    yield return StartCoroutine(CoMoveTo(_seat.SitPosition));
                    //앉은 상태가 되면 먹기 실행
                    _state = CustomerState.Eat;
                    break;

                case CustomerState.Eat:
                    // 앉기로 전환
                    if(_isVisualContect) _anim.Play("Sit");
                    // 레이어 위치 변경
                    _sr.sortingOrder = -1;

                    // 식사 대기시간.
                    for (int i = 0; i < _maxEatCount; i++)
                    {
                        if (_data.orderChans[i] <= 0.001 || _data.orderChans[i] == -1)
                        {
                            break;
                        }
                        else if (_firstOrder || Random.Range(0, 1f) <= _data.orderChans[_eatCounte])
                        {
                            _firstOrder = false;
                            _eatCounte++;
                            yield return new WaitForSeconds(_data.orderTime[i]);
                            _restaurant.TryCounsumeSushiAndEarnMoney(_tips.tipsMulti);
                            yield return new WaitForSeconds(_data.eatDuration[i]); // WaitForSeconds 너무 많은 호출 후에 개선 필요 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        }
                    }

                    _state = CustomerState.Exit;
                    break;
                    // 손님 퇴장
                case CustomerState.Exit:
                    // 애니메이션 출력
                    _anim.Play("Walk");
                    // 레이어 뒤로 밀기
                    _sr.sortingOrder = -2;
                    // 현재 자리 비우기
                    _seat.ClearSeat();
                    // 탈출 포인트까지 대기
                    yield return StartCoroutine(CoMoveTo(_exitPoint.position));
                    _seat.ClearSeat();
                    _restaurant.DeSpawnCustomer(gameObject);
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator CoMoveTo(Vector3 targetPos)
    {
        // 지정 좌석까지 이동하는 것을 구현.
        while ((transform.position - targetPos).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, a * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        yield break;
    }
}
