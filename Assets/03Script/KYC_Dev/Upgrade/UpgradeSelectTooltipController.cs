using System;
using System.Collections;
using UnityEngine;

public class UpgradeSelectTooltipController : MonoBehaviour
{
    [SerializeField] GameObject _tooltip;
    [SerializeField] float _hoverDelay;
    
    Coroutine _hoverCoroutine;
    WaitForSecondsRealtime _waitForSeconds;

    private void Awake()
    {
        _tooltip.SetActive(false);
        _waitForSeconds = new WaitForSecondsRealtime(_hoverDelay);
    }
    
    /// <summary>
    /// 업그레이드 UI창에 일정시간 머물면 툴팁 표기
    /// </summary>
    public void Hover()
    {
        if(_hoverCoroutine != null) return;
        _hoverCoroutine = StartCoroutine(HoverCoroutine());
    }

    /// <summary>
    /// 툴팁 OFF
    /// </summary>
    public void UnHover()
    {
        if(_hoverCoroutine != null) StopCoroutine(_hoverCoroutine);
        _hoverCoroutine = null;
        _tooltip.SetActive(false);
    }
    
    private IEnumerator HoverCoroutine()
    {
        yield return _waitForSeconds;
        _tooltip.SetActive(true);
        _hoverCoroutine = null;
    }
}
