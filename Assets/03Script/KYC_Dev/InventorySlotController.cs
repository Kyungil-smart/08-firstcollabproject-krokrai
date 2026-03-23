using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class InventorySlotController : MonoBehaviour
{
    /// <summary>
    /// 적용되는 아이템 SO
    /// ToDo:Temp_Item을 나중에 정식 아이템 SO로 변경 할 것
    /// </summary>
    public Temp_Item ItemInfo;
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// 인벤토리 슬롯이 보유중인 SO를 지정
    /// ToDo:Temp_Item을 나중에 정식 아이템 SO로 변경 할 것
    /// </summary>
    /// <param name="itemInfo"></param>
    public void SetInfo(Temp_Item itemInfo)
    {
        ItemInfo = itemInfo;
        SetImage();
    }
    
    /// <summary>
    /// 현재 인벤토리 슬롯이 어떤 아이콘인지 지정
    /// </summary>
    private void SetImage()
    {
        _image.sprite = ItemInfo.ItemIcon;
    }
    
    
    
}
