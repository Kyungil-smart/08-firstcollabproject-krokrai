using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] Button _btn;
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _restaurantScreen;
    [SerializeField] GameObject _fish;

    public event Action<bool> OnChangeSceneToRestaurant;

    bool _active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _active = false;
    }

    public void OnFishScene()
    {
        _fish.SetActive(true);
        _menu.SetActive(false);
        _restaurantScreen.SetActive(false);
        OnChangeSceneToRestaurant?.Invoke(false);
    }

    public void OnRestaurantScene()
    {
        _fish.SetActive(false);
        _restaurantScreen.SetActive(true);
        OnChangeSceneToRestaurant?.Invoke(true);
    }

    public void OpenMenuPanel()
    {
        _active = !_active;
        _menu.SetActive(_active);
    }
}
