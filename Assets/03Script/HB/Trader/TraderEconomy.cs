using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraderEconomy : MonoBehaviour
{
    public Button sellButton;                       // 판매 버튼
    public TextMeshProUGUI goldText;                // 현재 보유 골드TMP
    public TextMeshProUGUI totalPriceText;          // 선택된 물고기의 합계 금액TMP

    public void UpdateGoldUI(ulong money)
    {
        Debug.Log($"전달받은 금액 : {money}");

        if (goldText == null) return;

        bool isKor = DataTower.instance.languageSetting == Language.KOR;
        string moneyLabel = isKor ? "보유 금액" : "Money";

        goldText.text = $"{moneyLabel}: {money.TextFormatCurrency()} Gold";
    }

    public void UpdatePriceUI(long total)
    {
        if (totalPriceText == null) return;

        bool isKor = DataTower.instance.languageSetting == Language.KOR;
        string priceLabel = isKor ? "총 판매 금액" : "Total Price";

        totalPriceText.text = $"{priceLabel}: {((ulong)total).TextFormatCurrency()} Gold";
        sellButton.interactable = total > 0;    
    } 

    public bool CompleteTrade(List<FishSlot> selectedSlots, long totalPrice)
    {
        // 데이터 타워에 판매금액을 추가
        if (!DataTower.instance.TryMoenyChanged((ulong)totalPrice, false)) return false;

        foreach (var slot in selectedSlots)
        {   
            // 실제 데이터 삭제
            DataTower.instance.Items.Remove(slot.GetFishData());

            // 오브젝트 파괴
            Destroy(slot.gameObject);
        }

        return true;
    }
}
