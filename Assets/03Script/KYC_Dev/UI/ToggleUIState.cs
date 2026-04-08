using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ToggleUIState : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    
    [SerializeField] private AudioManager _audioManager;

    private void Start()
    {
        StartCoroutine(StartRoutine());
    }

    /// <summary>
    /// 활성화 버튼 눌렀을 때
    /// </summary>
    public void OnButtenClickUIActive()
    {
        _audioManager.PlaySfxClick();
        _ui.SetActive(true);
    }

    /// <summary>
    /// 비 활성화 버튼 눌렀을 때
    /// </summary>
    public void OnButtenClickUIClose()
    {
        _audioManager.PlaySfxClick();
        _ui.SetActive(false);
    }
    
    private IEnumerator StartRoutine()
    {
        yield return new WaitForSecondsRealtime(0.001f);
        gameObject.SetActive(false);
    }
}
