using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using TMPro;

public class InventoryView : MonoBehaviour
{
    [Tooltip("For Inventory System")]
    [SerializeField] GameObject _inventorySlotPrefab;
    [SerializeField] GameObject _inventoryContent;

    [Tooltip("UI Elements")]
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] TextMeshProUGUI _curGoldText;
    [SerializeField] TextMeshProUGUI _countText;
    [SerializeField] TextMeshProUGUI _sortSelectText;
    [SerializeField] TextMeshProUGUI _byCaughtText;
    [SerializeField] TextMeshProUGUI _byNameText;
    [SerializeField] TextMeshProUGUI _byRairtyText;

    private InventorySystem _inventorySystem;
    private GameObject _fishBookPrefab;

    private List<InventorySlotController> _slotLists = new();
    
    private void Awake()
    {
        _inventorySystem = FindFirstObjectByType<InventorySystem>();
    }

    private void OnEnable()
    {
        _inventorySystem.OnInventorySet += SetView;
        _inventorySystem.OnInventoryChanged += ChangeView;
        _inventorySystem.OnInventoryExtended += ExtendView;
    }

    private void OnDisable()
    {
        _inventorySystem.OnInventorySet -= SetView;
        _inventorySystem.OnInventoryChanged -= ChangeView;
        _inventorySystem.OnInventoryExtended -= ExtendView;
    }
    
    private void SetView()
    {
        foreach (FishData item in DataTower.instance.Items)
        {
            GameObject newItemSlot = Instantiate(_inventorySlotPrefab, _inventoryContent.transform);
            InventorySlotController slotController = newItemSlot.GetComponent<InventorySlotController>();
            _slotLists.Add(slotController);
            slotController.SetInfo(item);
        }
    }

    private void ChangeView()
    {
        for (int i = 0; i < _slotLists.Count; i++)
        {
            _slotLists[i].SetInfo(DataTower.instance.Items[i]);
        }
    }
    
    private void ExtendView(int lagacySlotMax, int slot)
    {
        for (int i = 0; i < slot; i++)
        {
            GameObject newItemSlot = Instantiate(_inventorySlotPrefab, _inventoryContent.transform);
            InventorySlotController slotController = newItemSlot.GetComponent<InventorySlotController>();
            _slotLists.Add(slotController);
            slotController.SetInfo(DataTower.instance.Items[lagacySlotMax + i - 1]);
        }
    }
}
