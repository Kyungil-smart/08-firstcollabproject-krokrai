using System.Collections;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public enum CustomerState
    {
        Enter,
        MoveToSeat,
        Eat,
        Exit,
    }

    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _eatDuration = 3f;
    [SerializeField] private int _mealPrice = 100;

    private RestaurantManager _restaurant;
    private RestaurantSeat _seat;
    private Transform _exitPoint;

    private CustomerState _state;

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
                    yield return StartCoroutine(CoMoveTo(_seat.SitPosition));
                    _state = CustomerState.Eat;
                    break;
                case CustomerState.Eat:
                    yield return new WaitForSeconds(_eatDuration);

                    if (!_restaurant.TryCounsumeSushiAndEarnMoney(_mealPrice))
                    {
                        // 초밥이 없으면 그냥 나감
                    }
                    _state = CustomerState.Exit;
                    break;
                case CustomerState.Exit:
                    _seat.ClearSeat();
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
