using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragNDropTarget : MonoBehaviour
{
    [SerializeField] private GameObject _backgrounnd;
    
    public bool isMove;
    
    private Vector2 _minBounds;
    private Vector2 _maxBounds;

    private float _halfWidth;
    private float _halfHeight;

    private Collider2D _collider2D;

    private void Awake()
    {
        isMove = false;
        _collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        InitClampBounds();
        GetColliderBounds();
    }

    private void Update()
    {
        MovingPosition();
    }
    
    private void InitClampBounds()
    {
        _minBounds = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _maxBounds = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    private void GetColliderBounds()
    {
        _halfWidth = _collider2D.bounds.extents.x;
        _halfHeight = _collider2D.bounds.extents.y;
    }

    private void MovingPosition()
    {
        if(isMove)
        {
            Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            float x = Mathf.Clamp(pointerPosition.x , _minBounds.x + _halfWidth, _maxBounds.x - _halfHeight);
            float y = Mathf.Clamp(pointerPosition.y , _minBounds.y + _halfHeight, _maxBounds.y - _halfHeight);
            transform.position = new Vector3(x, y, transform.position.z);
            _backgrounnd.transform.position = new Vector3(_backgrounnd.transform.position.x, y, _backgrounnd.transform.position.z);
        }
    }
    
    
}
