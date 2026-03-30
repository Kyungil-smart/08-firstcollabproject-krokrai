using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FishSlot : MonoBehaviour
{
    [Header("원본 프리팹")]
    public Image fishImage;     // 물고기 이미지
    public FishListManager fishListManager; // 도감 매니저 참조용

    [Header("상점 전용 프리팹(도감용에선 비워둠)")]
    public TextMeshProUGUI priceText;
    public GameObject highlightUI;
    public Toggle slotToggle;

    private FishData _currentFishData;
    private TraderUI _traderUI;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        // 도감용 FishSlot일 때만 동작
        if(fishListManager != null && _currentFishData != null)
        {
            // FishList 오브젝트에 데이터 전달, UI갱신
            fishListManager.ChangeFishData(_currentFishData);

            // FishList 오브젝트 활성화
            fishListManager.gameObject.SetActive(true);
        }
    }

    // 도감용 셋업 함수
    public void SetupFishBook(FishData data, FishListManager fishListManager)
    {
        _currentFishData = data;
        this.fishListManager = fishListManager;

        if (fishImage != null)
        {
            if (data.isCaught)
            {
                fishImage.sprite = data.fishSprite;
            }
            else
            {
                fishImage.sprite = data.silhouetteSprite;
            }
        }


        // 도감용에서는 상점 UI 비활성화
        if (priceText != null) priceText.gameObject.SetActive(false);
        if (slotToggle != null) slotToggle.gameObject.SetActive(false);
        if (highlightUI != null) highlightUI.SetActive(false);
    }

    public void OnSlotClick()
    {
        if (fishListManager == null) Debug.LogError($"{gameObject.name}: 매니저가 연결되지 않았습니다");
        if (_currentFishData == null) Debug.LogError($"{gameObject.name}: 물고기 데이터가 없습니다");

        if (fishListManager != null && _currentFishData != null)
        {
            Debug.Log($"{_currentFishData.korName} 클릭됨, 상세창을 엽니다");
            fishListManager.ChangeFishData(_currentFishData);
            fishListManager.gameObject.SetActive(true);
        }
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