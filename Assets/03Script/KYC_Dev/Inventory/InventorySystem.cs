using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class InventorySystem : MonoBehaviour
{
    // 싱글톤 여부는 담당자님이 결정

    // 테스트용 아이템
    // 주의 : 구현 한계 상 빈 아이템이 존재 해야됨 (_empty)
    [Header("FishData")]
    [SerializeField] private FishData _empty;
    [SerializeField] private FishData[] _datas;
    
    private Dictionary<string, FishData> _fishDatas = new ();
    private Dictionary<int, FishData> _fishAcquisitionDatas = new();
    private WaitForEndOfFrame _wfEndOfFrame = new();

    /// <summary>
    /// 현재 인벤토리 내부에 있는 물고기 숫자 기록용
    /// </summary>
    public int InventoryCount { get; private set; }

    #region Events
    
    // 인벤토리 view 관련 이벤트
    public event Action OnInventorySet;
    public event Action OnInventoryChanged;
    public event Action<int,int> OnInventoryExtended;
    public event Action<int> OnInventoryCountChanged;

    #endregion
    

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

        InventoryCount = 0;
        
        OnInventorySet?.Invoke();
        OnInventoryCountChanged?.Invoke(InventoryCount);
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
        
        RecordRecentCaught(_fishDatas[fishID]);
        InventoryCount++;
        
        OnInventoryChanged?.Invoke();
        OnInventoryCountChanged?.Invoke(InventoryCount);
        Debug.Log($"{fishID} inserted");
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
        
        DeleteLastestCaught(_fishDatas[fishID]);
        InventoryCount--;
        
        OnInventoryChanged?.Invoke();
        OnInventoryCountChanged?.Invoke(InventoryCount);
    }
    
    /// <summary>
    /// 고양이가 랜덤으로 가져가는 기능에 대응하는 함수
    /// </summary>
    public void SetCatFood()
    {
        if(_fishAcquisitionDatas.Count == 0) return;
        if(_fishAcquisitionDatas.Count == 1)
        {
            Erase(_fishAcquisitionDatas[1].fishID);
        }
        else
        {
            int temp = Random.Range(1, _fishAcquisitionDatas.Count);
            Erase(_fishAcquisitionDatas[temp].fishID);
        }
    }

    /// <summary>
    /// 모든 아이템 제거
    /// </summary>
    public void EraseAll()
    {
        DataTower.instance.Items.Clear();
        _fishAcquisitionDatas.Clear();
        StartCoroutine(InventoryStartRoutine());
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 아이템 정렬
    /// </summary>
    /// <param name="sortBy">1. 이름순 / 2. 획득 순 / 3. 레어리티 순</param>
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
                DataTower.instance.Items.Clear();
                
                for (int i = _fishAcquisitionDatas.Count; i > 0; i--)
                {
                    DataTower.instance.Items.Insert(0, _fishAcquisitionDatas[i]);
                }

                if (DataTower.instance.InventorySlotMax - _fishAcquisitionDatas.Count > 0)
                {
                    for (int i = 0; i < DataTower.instance.InventorySlotMax - _fishAcquisitionDatas.Count; i++)
                    {
                        DataTower.instance.Items.Add(_empty);
                    }
                }
                
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

    private void RecordRecentCaught(FishData data)
    {
        if (_fishAcquisitionDatas.Count != 0)
        {
            for (int i = _fishAcquisitionDatas.Count; i > 0; i--)
            {
                FishData temp = _fishAcquisitionDatas[i];
                _fishAcquisitionDatas.TryAdd(i + 1, temp);
                _fishAcquisitionDatas.Remove(i);
                
            }
        }
        _fishAcquisitionDatas.TryAdd(1, data);
    }

    private void DeleteLastestCaught(FishData data)
    {
        if(_fishAcquisitionDatas.Count == 0) return;
        
        for (int i = _fishAcquisitionDatas.Count; i > 0; i--)
        {
            if (_fishAcquisitionDatas[i] == data)
            {
                _fishAcquisitionDatas.Remove(i);
                break;
            }
        }

        for (int i = 1; i <= _fishAcquisitionDatas.Count; i++)
        {
            if (!_fishAcquisitionDatas.ContainsKey(i))
            {
                _fishAcquisitionDatas.TryAdd(i, _fishAcquisitionDatas[i+1]); 
                _fishAcquisitionDatas.Remove(i+1);
            }
        }
    }
}
