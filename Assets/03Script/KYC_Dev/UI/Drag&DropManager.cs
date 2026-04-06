using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragNDropManager : MonoBehaviour
{
    private InputSystem_Actions _actions;
    private Vector2 _startPos;
    private DragNDropTarget _target;
    
    

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
            if(_target != null)_target.isMove = true;
        }
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        if(_target == null) return;
        _target.isMove = false;
        _target = null;
    }
}
