using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private Button _targetButton;
    [SerializeField] private AudioManager _audioManager;

    private void Start()
    {
        if (_targetButton != null)
        {
            _targetButton.onClick.AddListener(_audioManager.PlaySfxClick);
        }
    }
}
