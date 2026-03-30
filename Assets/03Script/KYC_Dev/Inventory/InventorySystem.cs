using System;
using System.Collections;
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
    private WaitForEndOfFrame _wfEndOfFrame = new();
    
    public event Action OnInventorySet;
    public event Action OnInventoryChanged;
    public event Action<int,int> OnInventoryExtended;

    private void Awake()
    {
        InventoryInit();
    }

    private void Start()
    {
        StartCoroutine(InventoryStartRoutine());
    }

    private void InventoryInit()
    {
        foreach (FishData data in _datas)
        {
            _fishDatas.TryAdd(data.fishID, data);
        }
    }
    
    private IEnumerator InventoryStartRoutine()
    {
        while (DataTower.instance == null)
        {
            yield return _wfEndOfFrame;
        }
        
        for (int i = 0; i < DataTower.instance.InventorySlotMax; i++)
        {
            DataTower.instance.Items.Add(_empty);
        }
        
        OnInventorySet?.Invoke();
    }

    /// <summary>
    /// 인벤토리에 아이템 삽입하는 코드
    /// </summary>
    /// <param name="fishID">해당 되는 FishID를 **오타없이** 입력</param>
    public void Insert(string fishID)
    {
        if(!DataTower.instance.Items.Contains(_empty)) return;
        DataTower.instance.Items.Remove(_empty);
        DataTower.instance.Items.Insert(0, _fishDatas[fishID]);
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 입력 된 숫자 만큼 인벤토리 확장
    /// </summary>
    /// <param name="slot"></param>
    public void InventoryExtend(int slot)
    {
        int legacySlotMax = DataTower.instance.InventorySlotMax;
        DataTower.instance.InventorySlotMax += slot;
        for (int i = 0; i < slot; i++)
        {
            DataTower.instance.Items.Add(_empty);
        }
        OnInventoryExtended?.Invoke(legacySlotMax, slot);
    }

    
    /// <summary>
    /// 인벤토리에서 아이템 삭제
    /// </summary>
    /// <param name="fishID">해당 되는 FishID를 **오타없이** 입력</param>
    public void Erase(string fishID)
    {
        if(!DataTower.instance.Items.Contains(_fishDatas[fishID])) return;
        DataTower.instance.Items.Remove(_fishDatas[fishID]);
        DataTower.instance.Items.Add(_empty);
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 모든 아이템 제거
    /// </summary>
    public void EraseAll()
    {
        DataTower.instance.Items.Clear();
        InventoryStartRoutine();
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
                        DataTower.instance.Items.Sort((a, b) => a.engName.CompareTo(b.engName));
                        break;
                    case Language.KOR:
                        DataTower.instance.Items.Sort((a, b) => a.korName.CompareTo(b.korName));
                        break;
                    default:
                        break;
                }
                OnInventoryChanged?.Invoke();
                break;
            case 2:
                DataTower.instance.Items.Sort((a, b) => a.fishType.CompareTo(b.fishType));
                OnInventoryChanged?.Invoke();
                break;
            case 3:
                DataTower.instance.Items.Sort((a, b) => a.fishRarity.CompareTo(b.fishRarity));
                OnInventoryChanged?.Invoke();
                break;
            default:
                break;
        }
    }
}
