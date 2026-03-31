using System;
using UnityEngine;

public class InventoryButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _sortTab;
    [SerializeField] GameObject _encyclopediaPrefab;
    
    InventorySystem _inventorySystem;

    private void Awake()
    {
        _inventorySystem = FindFirstObjectByType<InventorySystem>();
        _sortTab.SetActive(false);
    }

    public void OnButtonClickToEncyclopedia()
    {
        _encyclopediaPrefab.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnButtonClickSortSelect()
    {
        _sortTab.SetActive(!_sortTab.activeSelf);
    }

    public void OnButtonClickByCaught()
    {
        _inventorySystem.SortItems(2);
    }
    
    public void OnButtonClickByName()
    {
        _inventorySystem.SortItems(1);
    }
    
    public void OnButtonClickByRarity()
    {
        _inventorySystem.SortItems(3);
    }
}
