using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using Kirurobo;
using UnityEngine.UI;

public class TouchLockTest : MonoBehaviour
{
    [SerializeField] private GameObject _testObject;
    [SerializeField] private GameObject _testObject2;
    [SerializeField] private TextMeshProUGUI _hit;
    [SerializeField] private TextMeshProUGUI _through;
    [SerializeField] private TextMeshProUGUI _pos;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _alpha;
    [SerializeField] private TextMeshProUGUI _buttonAlpha;
    [SerializeField] private TextMeshProUGUI _opacity;
    [SerializeField] private UniWindowController _uniWindowController;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] private Image _image;
    
    Vector2 _pointerPos;
    private float _timer;

    private void Awake()
    {
        _timer = 0f;
    }

    private void Update()
    {
        
        _pointerPos = Pointer.current.position.ReadValue();
        _hit.text = _uniWindowController.isHitTestEnabled.ToString();
        _through.text = _uniWindowController.isClickThrough.ToString();
        _alpha.text = _canvasGroup.alpha.ToString();
        _buttonAlpha.text = _image.color.ToString();
        _opacity.text = _uniWindowController.opacityThreshold.ToString();
        _pos.text = $"({_pointerPos.x} ,{_pointerPos.y})";
        TimerUI();
    }
    
    private void TimerUI()
    {
        _timer += Time.deltaTime;
        
        int minutes = (int)(_timer / 60);
        int seconds = (int)(_timer % 60);
        int microsecond = (int)((_timer * 100) % 100);

        _time.text = string.Format("{0:D2} : {1:D2} : {2:D2}", minutes, seconds, microsecond);
    }
    
    public void Onclick()
    {
        _testObject.SetActive(!_testObject.activeSelf);
    }

    // public void OnPointerEnter()
    // {
    //     _testObject2.SetActive(true);
    // }
    //
    // public void OnPointerExit()
    // {
    //     _testObject2.SetActive(false);
    // }
}
