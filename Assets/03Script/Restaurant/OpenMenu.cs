using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] Button _btn;
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _Fish;

    bool _active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _active = false;
    }

    public void ChangeFishScene()
    {
        _Fish.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnChanged()
    {
        _active = !_active;
        _menu.SetActive(_active);
    }
}
