using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    // 현재 체크된 슬롯
    public List<FishSlot> SelectedSlots(List<FishSlot> allSlots)
    {
        return allSlots.Where(s => s != null && s.gameObject.activeInHierarchy && s.slotToggle.isOn).ToList();
    }

    // 현재 화면에 보이는 모든 슬롯이 체크되었는지
    public bool IsAllSelected(List<FishSlot> allSlots)
    {
        var currentSlots = allSlots.Where(s => s != null && s.gameObject.activeInHierarchy).ToList();

        return currentSlots.Count > 0 && currentSlots.All(s => s.slotToggle.isOn);
    }

    // 체크된 물고기들의 가격
    public long TotalSelectedPrice(List<FishSlot> allSlots)
    {
        return SelectedSlots(allSlots).Sum(s => (long)s.GetFishData().price);
    }

    // 전체 선택/ 해제
    public void AllSelection(List<FishSlot> allSlots, bool isOn)
    {
        foreach (FishSlot slot in allSlots)
        {
            if (slot != null && slot.gameObject.activeInHierarchy)
            {
                slot.slotToggle.isOn = isOn;
            }
        }
    }
}
