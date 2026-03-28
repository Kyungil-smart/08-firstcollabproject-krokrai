using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishSlot : MonoBehaviour
{
    [Header("원본 프리팹")]
    public Image fishImage;     // 물고기 이미지

    [Header("상점 전용 프리팹(도감용에선 비워둠)")]
    public TextMeshProUGUI priceText;
    public GameObject highlightUI;
    public Toggle slotToggle;

    private FishData _currentFishData;
    private TraderUI _traderUI;
    private FishListManager _fishListManager; // 도감 매니저 참조용

    // 상점용 셋업 함수
    public void SetupTrader(FishData data, TraderUI trader)
    {
        _currentFishData = data;
        _traderUI = trader;

        // 물고기 이미지 설정
        if (fishImage != null)
        {
            fishImage.sprite = data.fishSprite;
        }

        // priceText가 연결돼 있을 때 실행(상점 전용)
        if (priceText != null)
        {
            // "N0"로 세자리 당 ',' 찍어주기 
            priceText.text = data.price.ToString("N0");
        }

        if (highlightUI != null) highlightUI.SetActive(false);

        // 토글 리스너 설정
        if (slotToggle != null)
        {
            slotToggle.onValueChanged.RemoveAllListeners();
            slotToggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    // 도감용 셋업 함수
    public void SetupFishBook(FishData data, FishListManager fishListManager)
    {
        _currentFishData = data;
        _fishListManager = fishListManager;

        if (fishImage != null)
        {
            fishImage.sprite = data.fishSprite;
        }

        // 도감용에서는 상점 UI 비활성화
        if (priceText != null) priceText.gameObject.SetActive(false);
        if (slotToggle != null) slotToggle.gameObject.SetActive(false);
        if (highlightUI != null) highlightUI.SetActive(false);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (highlightUI != null)
        {
            highlightUI.SetActive(isOn);
        }

        if(_traderUI != null)
        {
            _traderUI.OnSlotChanged();
        }
    }

    // 현재 물고기 데이터 반환 함수
    public FishData GetFishData()
    {
        return _currentFishData;
    }
}