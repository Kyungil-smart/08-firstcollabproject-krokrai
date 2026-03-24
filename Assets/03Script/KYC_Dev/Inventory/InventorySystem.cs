using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class InventorySystem : MonoBehaviour
{
    // 싱글톤 여부는 담당자님이 결정

    // 테스트용 아이템
    // 주의 : 구현 한계 상 빈 아이템이 존재 해야됨 (_empty)
    [SerializeField] private Temp_Item _empty;
    [SerializeField] private Temp_Item _item01;
    [SerializeField] private Temp_Item _item02;
    [SerializeField] private Temp_Item _item03;
    
    /// <summary>
    /// 인벤토리 최대 슬롯 > 업그레이드 단계에서 변화 하지만 일단 임의 값 입력
    /// </summary>
    public int InventorySlotMax;
    
    /// <summary>
    /// 아이템 관리 리스트
    /// </summary>
    public List<Temp_Item> Items = new();
    
    /// <summary>
    /// 인벤토리에 변화가 있을 때 구독한 View에서 받기 위함
    /// ToDo:Temp_Item을 나중에 정식 아이템 SO로 변경 할 것
    /// </summary>
    public event Action OnInventorySet;
    public event Action OnInventoryChanged;
    public event Action<int,int> OnInventoryExtended;

    private void Awake()
    {
        InventoryInit();
    }

    private void Start()
    {
        Debug.Log("InventorySystem Start");
        OnInventorySet?.Invoke();
    }

    private void InventoryInit()
    {
        for (int i = 0; i < InventorySlotMax; i++)
        {
            Items.Add(_empty);
        }
    }

    /// <summary>
    /// 인벤토리에 아이템 삽입 코드
    /// ToDo:Temp_Item을 나중에 정식 아이템 SO로 변경 할 것
    /// </summary>
    /// <param name="item"></param>
    public void Insert(Temp_Item item)
    {
        if(!Items.Contains(_empty)) return;
        Items.Remove(_empty);
        Items.Insert(0, item);
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 입력 된 숫자 만큼 인벤토리 확장
    /// </summary>
    /// <param name="slot"></param>
    public void InventoryExtend(int slot)
    {
        int lagacySlotMax = InventorySlotMax;
        InventorySlotMax += slot;
        Debug.Log(InventorySlotMax);
        for (int i = 0; i < slot; i++)
        {
            Items.Add(_empty);
        }
        Debug.Log(Items.Count);
        OnInventoryExtended?.Invoke(lagacySlotMax, slot);
    }

    /// <summary>
    /// 지정된 아이템 제거
    /// ToDo:Temp_Item을 나중에 정식 아이템 SO로 변경 할 것
    /// </summary>
    /// <param name="item"></param>
    public void Erase(Temp_Item item)
    {
        if(!Items.Contains(item)) return;
        Items.Remove(item);
        Items.Add(_empty);
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 모든 아이템 제거
    /// </summary>
    public void EraseAll()
    {
        Items.Clear();
        InventoryInit();
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 아이템 정렬
    /// </summary>
    /// <param name="sortBy">1. 이름순 / 2. 타입순 / 3. 레어리티 순</param>
    public void SortItems(int sortBy)
    {
        switch (sortBy)
        {
            case 1:
                Items.Sort((a, b) => a.Name.CompareTo(b.Name));
                OnInventoryChanged?.Invoke();
                break;
            case 2:
                Items.Sort((a, b) => a.ItemType.CompareTo(b.ItemType));
                OnInventoryChanged?.Invoke();
                break;
            case 3:
                Items.Sort((a, b) => a.Rarity.CompareTo(b.Rarity));
                OnInventoryChanged?.Invoke();
                break;
            default:
                break;
        }
    }
    
    
    /// <summary>
    /// Inventory Test 코드 : 1번 아이템 삭제
    /// </summary>
    public void OnClickErase01()
    {
        Erase(_item01);
    }
    /// <summary>
    /// Inventory Test 코드 : 전체 삭제
    /// </summary>
    public void OnClickEraseAll()
    {
        EraseAll();
    }
    /// <summary>
    /// Inventory Test 코드 : 1번 아이템 추가
    /// </summary>
    public void OnClickInsert01()
    {
        Insert(_item01);
    }
    /// <summary>
    /// Inventory Test 코드 : 2번 아이템 추가
    /// </summary>
    public void OnClickInsert02()
    {
        Insert(_item02);
    }
    /// <summary>
    /// Inventory Test 코드 : 3번 아이템추가
    /// </summary>
    public void OnClickInsert03()
    {
        Insert(_item03);
    }

    public void OnClickExtend05()
    {
        InventoryExtend(5);
    }
}
