using System;
using UnityEngine;
using UnityEngine.UI;
using Kirurobo;

public class TouckLockToggleVer4 : MonoBehaviour
{
    [SerializeField] UniWindowController _uniWindowController;
    [SerializeField] Toggle _touchLockToggle;
    [SerializeField] CanvasGroup[] _canvasGroups;
    [SerializeField] GameObject _touchLockCanvas; 
    
    private Color _toggleBackgroundColor;

    private void Awake()
    {
        _canvasGroups = FindObjectsByType<CanvasGroup>(FindObjectsSortMode.None);
        _touchLockCanvas.SetActive(false);
    }

    public void TouchLockEnable()
    {
        IsTouchLock(true);
    }
    
    public void TouchLockDisable()
    {
        IsTouchLock(false);
    }
    
    private void IsTouchLock(bool state)
    {
        switch (state)
        {
            case true:
                foreach (CanvasGroup canvas in _canvasGroups)
                {
                    canvas.alpha = 0.95f;
                    canvas.interactable = false;
                }
                _uniWindowController.opacityThreshold = 0.99f;
                _touchLockCanvas.SetActive(true);
                
                break;
            case false:
                foreach (CanvasGroup canvas in _canvasGroups)
                {
                    canvas.alpha = 1f;
                    canvas.interactable = true;
                }
                _uniWindowController.opacityThreshold = 0.3f;
                _touchLockCanvas.SetActive(false);
                break;
        }
    }
    
}
