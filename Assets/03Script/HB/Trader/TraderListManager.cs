using System.Collections.Generic;
using UnityEngine;

public class TraderListManager : MonoBehaviour
{
    public Transform content;           // FishSlot의 부모
    public GameObject fishSlotPrefab;   // FishSlot Variant 프리팹

    // 생성된 슬롯 리스트 관리
    private List<FishSlot> _allSlots = new List<FishSlot>();

    // 리스트 초기화, 생성
    public void RefreshList(List<FishData> items, TraderUI trader)
    {
        ClearList();

        foreach (FishData data in items)
        {
            GameObject go = Instantiate(fishSlotPrefab, content);
            FishSlot slot = go.GetComponent<FishSlot>();

            if (slot != null)
            {
                slot.SetupTrader(data, trader);
                _allSlots.Add(slot);
            }
        }
    }
    
    // 판매 완료된 슬롯을 관리 리스트에서 제거
    public void RemoveSlots(List<FishSlot> selectedSlots)
    {
        if (selectedSlots == null) return;

        foreach (var slot in selectedSlots)
        {
            if (_allSlots.Contains(slot))
            {
                _allSlots.Remove(slot);
            }
        }
    }

    // 등급 리스트를 받아 슬롯 끄고 켜기
    public void ApplyFilter(List<EFish_Rarity> selectedRates)
    {
        foreach (FishSlot slot in _allSlots)
        {
            if (slot != null && slot.GetFishData() != null)
            {
                bool show = selectedRates.Contains(slot.GetFishData().fishRarity);
                slot.gameObject.SetActive(show);
            }
        }
    }

    public void ClearList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        _allSlots.Clear();
    }

    public List<FishSlot> GetAllSlots() => _allSlots;
}
