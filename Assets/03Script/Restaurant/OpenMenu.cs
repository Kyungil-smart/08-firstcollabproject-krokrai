using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _restaurantScreen;
    [SerializeField] GameObject _fish;
    [SerializeField] Button _masterBtn;
    [SerializeField] AudioManager _audioManager;
    [SerializeField] GameObject _cat;

    public event Action<bool> OnChangeSceneToRestaurant;

    bool _active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _active = false;
        _menu.SetActive(false);
    }

    public void OnFishScene()
    {
        _fish.SetActive(true);
        _menu.SetActive(false);
        _audioManager.PlayBgmFishhook();
        _restaurantScreen.SetActive(false);
        OnChangeSceneToRestaurant?.Invoke(false);
    }

    public void OnRestaurantScene()
    {
        if (DataTower.instance.UnlockCatObjectLevel > 1)
            _cat.SetActive(true); // 구조를 바꿀 수 없었어...
        _audioManager.PlayBgmRestaurant();
        _fish.SetActive(false);
        _restaurantScreen.SetActive(true);
        OnChangeSceneToRestaurant?.Invoke(true);
    }

    public void OpenMenuPanel()
    {
        _audioManager.PlaySfxClick();
        _active = !_active;
        _masterBtn.interactable = !_active;
        _menu.SetActive(_active);
    }
}
