using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class InventorySlotController : MonoBehaviour
{
    /// <summary>
    /// 적용되는 아이템 SO
    /// </summary>
    public FishData ItemInfo;
    private Image _image;
    
    [SerializeField]private AddressableImageLoader _imageLoader;
    [SerializeField] private Image _fishImage;
    
    public event Action<FishData> OnItemChanged;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// 인벤토리 슬롯이 보유중인 SO를 지정
    /// + 어드레스 이미지 로더를 이용한 이미지 지정
    /// </summary>
    /// <param name="itemInfo"></param>
    public void SetInfo(FishData itemInfo)
    {
        ItemInfo = itemInfo;
        _imageLoader.SetImage(itemInfo.fishSprite, true);
        OnItemChanged?.Invoke(ItemInfo);
    }
}
