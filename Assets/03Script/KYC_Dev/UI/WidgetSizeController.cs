using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WidgetSizeController : MonoBehaviour
{
    private InputSystem_Actions _actions;
    private Vector2 _startPos;
    private WidgetSizeHandle _target;

    public event Action<bool> OnMove;

    private void Awake()
    {
        _actions = new InputSystem_Actions();
        _target = null;
    }

    private void OnEnable()
    {
        _actions.Enable();

        _actions.DragNDrop.Drag.performed += OnPointerDown;
        _actions.DragNDrop.Drag.canceled += OnPointerUp;
    }

    private void OnDisable()
    {
        _actions.DragNDrop.Drag.performed -= OnPointerDown;
        _actions.DragNDrop.Drag.canceled -= OnPointerUp;

        _actions.Disable();
    }

    private void OnPointerDown(InputAction.CallbackContext context)
    {
        _startPos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        RaycastHit2D[] hit2Ds = Physics2D.RaycastAll(_startPos, Vector2.zero);

        foreach (RaycastHit2D hit2D in hit2Ds)
        {
            hit2D.collider.gameObject.TryGetComponent(out _target);
            if (_target != null)
            {
                _target.isMove = true;
                OnMove?.Invoke(true);
            }
        }
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        if (_target == null) return;
        _target.isMove = false;
        OnMove?.Invoke(false);
        _target = null;
    }
}