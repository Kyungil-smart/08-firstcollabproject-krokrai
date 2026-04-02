using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragNDropManager : MonoBehaviour
{
    [SerializeField] LayerMask _dragNDropLayer;
    
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

        _actions.UI.Click.performed += OnPointerDown;
        _actions.UI.Click.canceled += OnPointerUp;
    }

    private void OnDisable()
    {
        _actions.UI.Click.performed -= OnPointerDown;
        _actions.UI.Click.canceled -= OnPointerUp;
        
        _actions.Disable();
    }

    private void OnPointerDown(InputAction.CallbackContext context)
    {
        Debug.Log("OnPointerDown");
        _startPos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        Ray2D ray2D = new Ray2D(_startPos, Vector2.zero);
        RaycastHit2D hit2D = Physics2D.Raycast(ray2D.origin, ray2D.direction, _dragNDropLayer);

        if (hit2D.collider != null)
        {
            _target = hit2D.collider.GetComponent<DragNDropTarget>();
            _target.isMove = true;
        }
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        Debug.Log("OnPointerUp");
        if(_target == null) return;
        _target.isMove = false;
        _target = null;
    }
}
