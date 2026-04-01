using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TouckLockToggleVer3 : MonoBehaviour
{
    [SerializeField] GameObject _lockCanvas;

    private void Awake()
    {
        _lockCanvas.SetActive(false);
    }

    public void OnClickTouchLock()
    {
        _lockCanvas.SetActive(true);
    }

    public void OnClickTouchUnlock()
    {
        _lockCanvas.SetActive(false);
    }

    
}
