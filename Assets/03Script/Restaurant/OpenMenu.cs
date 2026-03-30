using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] Button _btn;
    [SerializeField] GameObject _menu;

    bool _active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _active = false;
    }

    public void OnChanged()
    {
        _active = !_active;
        _menu.SetActive(_active);
    }
}
