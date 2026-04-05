using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragNDropTarget : MonoBehaviour
{
    [Header("배경 지정")]
    [SerializeField] private GameObject _backgrounnd;

    [Header("반전할 이미지들")]
    [SerializeField] private GameObject[] _mirroringImages;

    [Header("For Debug")]
    public bool isMove;
    [SerializeField] private bool _isMirroring;
    
    private Vector2 _minBounds;
    private Vector2 _maxBounds;

    private float _halfWidth;
    private float _halfHeight;

    private Collider2D _collider2D;

    private void Awake()
    {
        isMove = false;
        _isMirroring = false;
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
            Mirroring(transform.position);
        }
    }

    private void Mirroring(Vector3 position)
    {
        Vector2 camaraCenter = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));

        if (camaraCenter.x > position.x && !_isMirroring)
        {
            foreach (GameObject image in _mirroringImages)
            {
                float x = image.transform.localScale.x;
                float y = image.transform.localScale.y;
                float z = image.transform.localScale.z;

                image.transform.localScale = new Vector3(x, -y, z);
            }
            _isMirroring = true;
        }
        else if(camaraCenter.x < position.x && _isMirroring)
        {
            foreach (GameObject image in _mirroringImages)
            {
                float x = image.transform.localScale.x;
                float y = image.transform.localScale.y;
                float z = image.transform.localScale.z;

                image.transform.localScale = new Vector3(x, -y, z);
            }
            _isMirroring = false;
        }
    }
    
    
}
