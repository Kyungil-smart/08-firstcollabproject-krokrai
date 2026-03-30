using UnityEngine;
using UnityEngine.Serialization;

public class ToggleUIState : MonoBehaviour
{
    [SerializeField] GameObject _ui;

    private void Awake()
    {
        OnButtenClickUIClose();
    }

    /// <summary>
    /// 활성화 버튼 눌렀을 때
    /// </summary>
    public void OnButtenClickUIActive()
    {
        _ui.SetActive(true);
    }

    /// <summary>
    /// 비 활성화 버튼 눌렀을 때
    /// </summary>
    public void OnButtenClickUIClose()
    {
        _ui.SetActive(false);
    }
}
