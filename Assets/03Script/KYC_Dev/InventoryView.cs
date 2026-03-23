using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class InventoryView : MonoBehaviour
{
    //ToDo:인벤토리를 싱글톤으로 하게 된다면 InventorySystem 지정과 Awake는 없어도 됨
    private InventorySystem _inventorySystem;
    
    [SerializeField] GameObject _inventorySlotPrefab;

    private List<InventorySlotController> _slotLists = new();
    private void Awake()
    {
        _inventorySystem = FindFirstObjectByType<InventorySystem>();
    }

    private void OnEnable()
    {
        Debug.Log("InventorySystem OnEnable");
        _inventorySystem.OnInventorySet += SetView;
        _inventorySystem.OnInventoryChanged += ChangeView;
        _inventorySystem.OnInventoryExtended += ExtendView;
    }

    private void OnDisable()
    {
        _inventorySystem.OnInventorySet -= SetView;
        _inventorySystem.OnInventoryChanged -= ChangeView;
        _inventorySystem.OnInventoryExtended -= ExtendView;
        Debug.Log("InventorySystem OnDisable");
    }
    
    private void SetView()
    {
        foreach (Temp_Item item in _inventorySystem.Items)
        {
            GameObject newItemSlot = Instantiate(_inventorySlotPrefab, transform);
            InventorySlotController slotController = newItemSlot.GetComponent<InventorySlotController>();
            _slotLists.Add(slotController);
            slotController.SetInfo(item);
        }
    }

    private void ChangeView()
    {
        for (int i = 0; i < _slotLists.Count; i++)
        {
            _slotLists[i].SetInfo(_inventorySystem.Items[i]);
        }
    }
    
    private void ExtendView(int lagacySlotMax, int slot)
    {
        for (int i = 0; i < slot; i++)
        {
            GameObject newItemSlot = Instantiate(_inventorySlotPrefab, transform);
            InventorySlotController slotController = newItemSlot.GetComponent<InventorySlotController>();
            _slotLists.Add(slotController);
            slotController.SetInfo(_inventorySystem.Items[lagacySlotMax + i - 1]);
        }
    }
}
