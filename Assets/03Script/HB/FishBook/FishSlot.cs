using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    // 로드된 이미지의 관리, 출력
    private AsyncOperationHandle<Sprite> _handle;

    // 상점용 셋업 함수
    public void SetupTrader(FishData data, TraderUI trader)
    {
        _currentFishData = data;
        _traderUI = trader;

        if (fishImage != null)
        {
            LoadImageAsync(data.fishSprite);
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

    private void LoadImageAsync(string address)
    {
        // 주소가 없거나 "NullException"이면 돌아가기
        if (string.IsNullOrEmpty(address) || address == "NullException")
        {
            Debug.Log("이미지 주소가 비었습니다.");
            return;
        }

        // 로드된 이미지가 있다면, 불러오기 전 이미지를 메모리에서 해제
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }

        // 구글 시트에 있는 이미지를 가져옴(결과물은 지금 X, _handle 이라는 영수증만 우선)
        _handle = Addressables.LoadAssetAsync<Sprite>(address);

        _handle.Completed += (operation) =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                fishImage.sprite = operation.Result;

                fishImage.preserveAspect = true;
            }
        };
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


        // 도감용에서는 상점 UI 비활성화
        if (priceText != null) priceText.gameObject.SetActive(false);
        if (slotToggle != null) slotToggle.gameObject.SetActive(false);
        if (highlightUI != null) highlightUI.SetActive(false);

        if (fishImage != null)
        {
            // 불러올 이미지 주소(string) 
            string targetAddress = data.isCaught ? data.fishSprite : data.silhouetteSprite;

            // 이미지 로드
            LoadImageAsync(targetAddress);
        }
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

    private void OnDestroy()
    {
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }    
    }
}