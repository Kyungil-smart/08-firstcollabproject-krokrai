using System.Collections;
using UnityEngine;
using static Define;

public class CustomerController : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;

    [Header("Data")]
    [SerializeField] private CustomerDataSO _data;

    [Header("Runtime Data")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _eatDuration = 3f;
    [SerializeField] private int _priceFactor = 1;
    [SerializeField] private float _spawnDelay = 3f;

    private RestaurantManager _restaurant;
    private RestaurantSeat _seat;
    private Transform _exitPoint;

    private CustomerState _state;

    private void Awake()
    {
        _moveSpeed = _data.MoveSpeed;
        _eatDuration = _data.EatDuration;
        _priceFactor = _data.PriceScaleFactor;
        _spawnDelay = _data.SpawnDelay;

        _anim = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
    }
    /// <summary>
    /// 손님 스폰 시간 값
    /// </summary>
    /// <returns></returns>
    public float SpawnDelay() => _data.SpawnDelay;
    /// <summary>
    /// 손님의 기본 설정을 초기화 해주는 함수
    /// </summary>
    /// <param name="restaurant"></param>
    /// <param name="seat"></param>
    /// <param name="exitPoint"></param>
    public void SetInfo(RestaurantManager restaurant, RestaurantSeat seat, Transform exitPoint)
    {
        _restaurant = restaurant;
        _seat = seat;
        _exitPoint = exitPoint;

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
                    _sr.sortingOrder = -2;
                    _anim.Play("Walk");
                    yield return StartCoroutine(CoMoveTo(_seat.SitPosition));
                    _state = CustomerState.Eat;
                    break;
                case CustomerState.Eat:
                    _anim.Play("Sit");
                    _sr.sortingOrder = -1;
                    yield return new WaitForSeconds(_eatDuration);

                    if (_restaurant.TryCounsumeSushiAndEarnMoney(_priceFactor * 200))
                    {
                        bool canSecondEat = Random.Range(0, 101) < _data.SecondEatChance;
                        if (canSecondEat)
                        {
                            yield return new WaitForSeconds(_eatDuration);
                        }
                    }
                    _state = CustomerState.Exit;
                    break;
                case CustomerState.Exit:
                    _seat.ClearSeat(this);
                    _sr.sortingOrder = -2;
                    _anim.Play("Walk");
                    yield return StartCoroutine(CoMoveTo(_exitPoint.position));
                    Destroy(gameObject);
                    break;

            }
            yield return null;
        }
    }
    private IEnumerator CoMoveTo(Vector3 targetPos)
    {
        while ((transform.position - targetPos).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }
}
