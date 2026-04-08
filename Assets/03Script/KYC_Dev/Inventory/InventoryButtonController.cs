using System;
using UnityEngine;

public class InventoryButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _sortTab;
    [SerializeField] GameObject _encyclopediaPrefab;
    
    InventorySystem _inventorySystem;
    AudioManager _audioManager;

    private void Awake()
    {
        _inventorySystem = FindFirstObjectByType<InventorySystem>();
        _audioManager = FindFirstObjectByType<AudioManager>();
        _sortTab.SetActive(false);
    }

    public void OnButtonClickToEncyclopedia()
    {
        _audioManager.PlaySfxClick();
        _encyclopediaPrefab.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnButtonClickSortSelect()
    {
        _audioManager.PlaySfxClick();
        _sortTab.SetActive(!_sortTab.activeSelf);
    }

    public void OnButtonClickByCaught()
    {
        _audioManager.PlaySfxClick();
        _inventorySystem.SortItems(2);
    }
    
    public void OnButtonClickByName()
    {
        _audioManager.PlaySfxClick();
        _inventorySystem.SortItems(1);
    }
    
    public void OnButtonClickByRarity()
    {
        _audioManager.PlaySfxClick();
        _inventorySystem.SortItems(3);
    }
}
