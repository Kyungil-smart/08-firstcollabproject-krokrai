using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InventorySlotController : MonoBehaviour
{
    /// <summary>
    /// 적용되는 아이템 SO
    /// </summary>
    public FishData ItemInfo;
    private Image _image;
    private AsyncOperationHandle<Sprite> _handle;
    private AddressableImageLoader _imageLoader;
    
    [SerializeField] private Image _baseImage;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    private void OnDestroy()
    {
        // 슬롯이 파괴될 때 반납
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }
    } 

    /// <summary>
    /// 인벤토리 슬롯이 보유중인 SO를 지정
    /// ToDo:Temp_Item을 나중에 정식 아이템 SO로 변경 할 것
    /// </summary>
    /// <param name="itemInfo"></param>
    public void SetInfo(FishData itemInfo)
    {
        ItemInfo = itemInfo;
        SetImage();
    }
    
    /// <summary>
    /// 현재 인벤토리 슬롯이 어떤 아이콘인지 지정
    /// </summary>
    private void SetImage()
    {
        LoadImageAsync (ItemInfo.fishSprite);
    }
    
    private void LoadImageAsync(string address)
    {
        if (string.IsNullOrEmpty(address) || address == "NullException") return;

        // 새로운 이미지를 부르기 전, 기존에 빌린 이미지가 있다면 반납
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }

        // 이미지 주문 시작
        _handle = Addressables.LoadAssetAsync<Sprite>(address);

        _handle.Completed += (operation) =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                // UI 이미지에 결과 적용
                _image.sprite = operation.Result;
            }
        };
    }
}
