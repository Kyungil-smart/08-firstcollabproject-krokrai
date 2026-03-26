using System.Collections;
using UnityEngine;
using static Define;

public class CustomerController : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;

    [Header("Data")]
    [SerializeField] private CustomerDataSO _data;


    // 데이터 나중에 DataTower에서 받아오기.
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
        /*
        _moveSpeed = _data.MoveSpeed;
        _eatDuration = _data.EatDuration;
        _priceFactor = _data.PriceScaleFactor; 
        _spawnDelay = _data.SpawnDelay;
        */
        _anim = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// 손님 스폰 시간 값
    /// </summary>
    /// <returns></returns>
    //public float SpawnDelay() => _data.SpawnDelay;

    /// <summary>
    /// RestaurangtManager와 연결.
    /// </summary>
    /// <param name="restaurant"></param>
    public void ConnectRestaurant(RestaurantManager restaurant)
    {
        _restaurant = restaurant;
    }

    /// <summary>
    /// 손님의 기본 설정을 초기화 해주는 함수
    /// </summary>
    /// <param name="restaurant"></param>
    /// <param name="seat"></param>
    /// <param name="exitPoint"></param>
    public void SetInfo(RestaurantSeat seat, Transform exitPoint)
    {
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
                    // 레이어 뒤로 미루기
                    _sr.sortingOrder = -2;
                    // 애니메이션 출력
                    _anim.Play("Walk");
                    // 자리에 이동할때까지 대기
                    yield return StartCoroutine(CoMoveTo(_seat.SitPosition));
                    //앉은 상태가 되면 먹기 실행
                    _state = CustomerState.Eat;
                    break;

                case CustomerState.Eat:
                    // 앉기로 전환
                    _anim.Play("Sit");
                    // 레이어 위치 변경
                    _sr.sortingOrder = -1;
                    // 식사 대기시간.
                    yield return new WaitForSeconds(_eatDuration);

                    // 이부분 추가 수정 필요 TryCounsumSushiAndEarnMoney(price) 손님 행동 테이블 업데이트에 맞게 수정해야 하니 후순위로 작업 @@@@@@@@@@@@@@@@@@
                    if (_restaurant.TryCounsumeSushiAndEarnMoney(_priceFactor * 200))
                    {
                        /*
                        // 추가로 먹을 확률 계산
                        if (Random.Range(0, 101) < _data.SecondEatChance)
                        {
                            yield return new WaitForSeconds(_eatDuration); // WaitForSeconds 너무 많은 호출 후에 개선 필요 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        }
                        */
                    }
                    _state = CustomerState.Exit;
                    break;
                    // 손님 퇴장
                case CustomerState.Exit:
                    // 현재 자리 비우기
                    _seat.ClearSeat();
                    // 레이어 뒤로 밀기
                    _sr.sortingOrder = -2;
                    // 애니메이션 출력
                    _anim.Play("Walk");
                    // 탈출 포인트까지 대기
                    yield return StartCoroutine(CoMoveTo(_exitPoint.position));
                    // 파괴 Instantiate를 교체하면서 반드시 교체 필수  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    //Destroy(gameObject);
                    Debug.Log($"{gameObject.name} 도착");
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
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }
}
