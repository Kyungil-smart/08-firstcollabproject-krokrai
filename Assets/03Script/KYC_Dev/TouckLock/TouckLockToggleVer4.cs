using System;
using UnityEngine;
using UnityEngine.UI;
using Kirurobo;

public class TouckLockToggleVer4 : MonoBehaviour
{
    [SerializeField] UniWindowController _uniWindowController;
    [SerializeField] Toggle _touchLockToggle;
    [SerializeField] CanvasGroup[] _canvasGroups;
    
    private Color _toggleBackgroundColor;

    private void Awake()
    {
        _canvasGroups = FindObjectsByType<CanvasGroup>(FindObjectsSortMode.None);
    }

    public void TouchLockEnable()
    {
        IsTouchLock(true);
    }
    
    public void TouchLockDisable()
    {
        IsTouchLock(false);
    }
    
    private void IsTouchLock(bool isOn)
    {
        switch (isOn)
        {
            case true:
                foreach (CanvasGroup canvas in _canvasGroups)
                {
                    canvas.alpha = 0.95f;
                    canvas.interactable = false;
                }
                _uniWindowController.opacityThreshold = 0.99f;
                
                break;
            case false:
                foreach (CanvasGroup canvas in _canvasGroups)
                {
                    canvas.alpha = 1f;
                    canvas.interactable = true;
                }
                _uniWindowController.opacityThreshold = 0.3f;
                
                break;
        }
    }
    
}
