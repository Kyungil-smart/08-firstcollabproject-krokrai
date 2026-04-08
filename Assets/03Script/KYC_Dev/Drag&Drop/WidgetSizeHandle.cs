using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class WidgetSizeHandle : MonoBehaviour
{
    [SerializeField] GameObject _targetObj;
    
    [Header("For Debug")]
    public bool isMove;
    
    private float _minX;
    private float _maxX;
    
    private float _halfWidth;
    private float _offsetX;

    private float _maskMinScale;
    
    private WidgetSizeController _controller;
    private Collider2D _collider2D;
    private SpriteRenderer _sprite;
    private DragNDropTarget _target;
    
    private void Awake()
    {
        isMove = false;
        _collider2D = GetComponent<Collider2D>();
        _controller = GetComponentInParent<WidgetSizeController>();
        _sprite = GetComponent<SpriteRenderer>();
        _target = GetComponentInParent<DragNDropTarget>();
    }

    private void OnEnable()
    {
        _controller.OnMove += VisibleHandle;
    }

    private void OnDisable()
    {
        _controller.OnMove -= VisibleHandle;
    }

    private void Start()
    {
        InitXBounds();
        GetColliderBounds();
    }

    private void Update()
    {
        if(isMove) MovingPosition();
    }
    
    private void InitXBounds()
    {
        _minX = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        _maxX = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x;
    }

    private void GetColliderBounds()
    {
        _halfWidth = _collider2D.bounds.extents.x;
        _offsetX = _collider2D.offset.x;
        _maskMinScale = transform.parent.localScale.x;
    }

    private void MovingPosition()
    {
        Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        float leftLimit = _minX + (_halfWidth - _offsetX);
        float rightLimit = transform.parent.position.x - _maskMinScale;
        float x = Mathf.Clamp(pointerPosition.x, leftLimit, rightLimit);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        ChangeMaskScale(x);
    }

    private void ChangeMaskScale(float x)
    {
        float maskX = transform.parent.position.x - x;
        transform.parent.localScale = new Vector3(maskX, transform.parent.localScale.y, transform.parent.localScale.z);
    }
    
    private void VisibleHandle(bool state)
    {
        switch (state)
        {
            case true:
                _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1f);
                break;
            case false:
                _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0f);
                break;
        }
    }
}
