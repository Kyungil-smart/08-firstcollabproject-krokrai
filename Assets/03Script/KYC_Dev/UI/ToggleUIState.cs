using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ToggleUIState : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindFirstObjectByType<AudioManager>();
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
}
