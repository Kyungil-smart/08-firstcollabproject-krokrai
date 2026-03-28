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
