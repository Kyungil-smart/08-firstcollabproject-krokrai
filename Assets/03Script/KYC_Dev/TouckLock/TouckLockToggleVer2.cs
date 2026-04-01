using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kirurobo;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TouckLockToggleVer2 : MonoBehaviour
{
    [Tooltip("UniWindowController in Scene")]
    [SerializeField] UniWindowController _uniWindowController;
    
    [Tooltip("TouchLockToggle in Scene")]
    [SerializeField] Toggle _touchLockToggle;
    [SerializeField] RectTransform _touchLockTransform;
    
    [Tooltip("Others")]
    [SerializeField] float _touchLockDelay;

    private Vector2 _pointerPos;

    private void Update()
    {
        if(_touchLockToggle.isOn) WhileTouchLock();
    }

    public void ToggleTouchLock()
    {
        _uniWindowController.isHitTestEnabled = !_touchLockToggle.isOn;
        Debug.Log($"Toggle : {_uniWindowController.isHitTestEnabled}");
    }

    private void WhileTouchLock()
    {
        _pointerPos = Pointer.current.position.ReadValue();
        if (RectTransformUtility.RectangleContainsScreenPoint(_touchLockTransform, _pointerPos))
        {
            _uniWindowController.isClickThrough = false;
        }
        else
        {
            _uniWindowController.isClickThrough = true;
        }
        Debug.Log(_uniWindowController.isClickThrough);
    }
}
