using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class DragNDropTarget : MonoBehaviour
{
    [Header("배경 지정")] 
    [SerializeField] private GameObject _backgrounnd;

    [Header("반전할 이미지들")] 
    [SerializeField] private GameObject[] _mirroringImages;

    [Header("x축 한계 보정용")] 
    [SerializeField] private GameObject _mask;
    [SerializeField] private GameObject _handle;

    [Header("For Debug")] 
    public bool isMove;
    [SerializeField] private bool _isMirroring;

    private Vector2 _minBounds;
    private Vector2 _maxBounds;
    private Vector2 _camaraCenter;

    public float HalfWidth {get; private set; }
    private float _halfHeight;
    public float OffsetX {get; private set; }
    private float _offsetY;
    private float _handleWidth;

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
        CheckPosition(transform.position);
    }

    private void Update()
    {
        if(isMove) MovingPosition();
    }
    
    private void InitClampBounds()
    {
        _minBounds = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _maxBounds = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        _camaraCenter = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
    }

    private void GetColliderBounds()
    {
        HalfWidth = _collider2D.bounds.extents.x;
        _halfHeight = _collider2D.bounds.extents.y;
        OffsetX = _collider2D.offset.x;
        _offsetY = _collider2D.offset.y;
        _handleWidth = _handle.transform.localScale.x;
    }

    private void CheckPosition(Vector3 position)
    {
        _isMirroring = _camaraCenter.x > position.x;
    }

    private void MovingPosition()
    {
        Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        float x = Mathf.Clamp(
            pointerPosition.x, _minBounds.x + (_mask.transform.localScale.x + _handleWidth) - (HalfWidth - OffsetX), _maxBounds.x - (HalfWidth - OffsetX));
        float y = Mathf.Clamp(
            pointerPosition.y, _minBounds.y + (_halfHeight - _offsetY), _maxBounds.y - (_halfHeight + _offsetY + 1f));
        transform.position = new Vector3(x, y, transform.position.z);
        _backgrounnd.transform.position =
            new Vector3(_backgrounnd.transform.position.x, y, _backgrounnd.transform.position.z);
        Mirroring(transform.position);
        
    }

    private void Mirroring(Vector3 position)
    {
        if (_camaraCenter.x > position.x && !_isMirroring)
        {
            foreach (GameObject image in _mirroringImages)
            {
                float x = image.transform.localScale.x;
                float y = image.transform.localScale.y;
                float z = image.transform.localScale.z;

                image.transform.localScale = new Vector3(-x, y, z);
            }
            _isMirroring = true;
        }
        else if(_camaraCenter.x < position.x && _isMirroring)
        {
            foreach (GameObject image in _mirroringImages)
            {
                float x = image.transform.localScale.x;
                float y = image.transform.localScale.y;
                float z = image.transform.localScale.z;

                image.transform.localScale = new Vector3(-x, y, z);
            }
            _isMirroring = false;
        }
    }
}
