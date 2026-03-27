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
    [Header("FishData")]
    [SerializeField] private FishData _empty;
    [SerializeField] private FishData[] _datas;
    
    private Dictionary<string, FishData> _fishDatas = new ();
    
    /// <summary>
    /// 인벤토리 최대 슬롯 > 업그레이드 단계에서 변화 하지만 일단 임의 값 입력
    /// </summary>
    public int InventorySlotMax;
    
    /// <summary>
    /// 아이템 관리 리스트
    /// </summary>
    public List<FishData> Items = new();
    
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
        OnInventorySet?.Invoke();
    }

    private void InventoryInit()
    {
        foreach (FishData data in _datas)
        {
            _fishDatas.TryAdd(data.fishID, data);
        }
        
        for (int i = 0; i < InventorySlotMax; i++)
        {
            Items.Add(_empty);
        }
    }

    /// <summary>
    /// 인벤토리에 아이템 삽입하는 코드
    /// </summary>
    /// <param name="fishID">해당 되는 FishID를 **오타없이** 입력</param>
    public void Insert(string fishID)
    {
        if(!Items.Contains(_empty)) return;
        Items.Remove(_empty);
        Items.Insert(0, _fishDatas[fishID]);
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
        for (int i = 0; i < slot; i++)
        {
            Items.Add(_empty);
        }
        OnInventoryExtended?.Invoke(lagacySlotMax, slot);
    }

    
    /// <summary>
    /// 인벤토리에서 아이템 삭제
    /// </summary>
    /// <param name="fishID">해당 되는 FishID를 **오타없이** 입력</param>
    public void Erase(string fishID)
    {
        if(!Items.Contains(_fishDatas[fishID])) return;
        Items.Remove(_fishDatas[fishID]);
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
                switch (DataTower.instance.languageSetting)
                {
                    case Language.ENG:
                        Items.Sort((a, b) => a.engName.CompareTo(b.engName));
                        break;
                    case Language.KOR:
                        Items.Sort((a, b) => a.korName.CompareTo(b.korName));
                        break;
                    default:
                        break;
                }
                OnInventoryChanged?.Invoke();
                break;
            case 2:
                Items.Sort((a, b) => a.fishType.CompareTo(b.fishType));
                OnInventoryChanged?.Invoke();
                break;
            case 3:
                Items.Sort((a, b) => a.fishRarity.CompareTo(b.fishRarity));
                OnInventoryChanged?.Invoke();
                break;
            default:
                break;
        }
    }
}
