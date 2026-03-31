using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryScrollView : MonoBehaviour
{
    [Tooltip("For Inventory System")]
    [SerializeField] GameObject _inventorySlotPrefab;

    private InventorySystem _inventorySystem;
    
    private List<InventorySlotController> _slotLists = new();
    
    
    private void Awake()
    {
        _inventorySystem = FindFirstObjectByType<InventorySystem>();
    }

    private void OnEnable()
    {
        EventEnable();
    }

    private void OnDisable()
    {
        EventDisable();
    }

    #region 이벤트 구독/해제

    private void EventEnable()
    {
        
        _inventorySystem.OnInventorySet += SetView;
        _inventorySystem.OnInventoryChanged += ChangeView;
        _inventorySystem.OnInventoryExtended += ExtendView;
    }

    private void EventDisable()
    {
        _inventorySystem.OnInventorySet -= SetView;
        _inventorySystem.OnInventoryChanged -= ChangeView;
        _inventorySystem.OnInventoryExtended -= ExtendView;
    }

    #endregion

    #region 인벤토리 창 컨트롤러

    private void SetView()
    {
        foreach (FishData item in DataTower.instance.Items)
        {
            GameObject newItemSlot = Instantiate(_inventorySlotPrefab, transform);
            InventorySlotController slotController = newItemSlot.GetComponent<InventorySlotController>();
            _slotLists.Add(slotController);
            slotController.SetInfo(item);
        }
    }

    private void ChangeView()
    {
        int a = DataTower.instance.Items.Count;
        int b = _slotLists.Count;

        if (a < b)
        {
            DiscardView(b - a);
            b = _slotLists.Count;
        }

        for (int i = 0; i < b; i++)
        {
            _slotLists[i].SetInfo(DataTower.instance.Items[i]);
        }
    }
    
    private void ExtendView(int lagacySlotMax, int slot)
    {
        for (int i = 0; i < slot; i++)
        {
            GameObject newItemSlot = Instantiate(_inventorySlotPrefab, transform);
            InventorySlotController slotController = newItemSlot.GetComponent<InventorySlotController>();
            _slotLists.Add(slotController);
            slotController.SetInfo(DataTower.instance.Items[lagacySlotMax + i - 1]);
        }
    }

    private void DiscardView(int slot)
    {
        for (int i = 0; i < slot; i++)
        {
            _slotLists.RemoveAt(_slotLists.Count - 1);
            Destroy(_inventorySlotPrefab);
        }
    }

    #endregion
    
}
