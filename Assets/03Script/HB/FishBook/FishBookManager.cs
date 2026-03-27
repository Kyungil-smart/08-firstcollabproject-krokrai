using UnityEngine;
using System.Collections.Generic;

public class FishBookManager : MonoBehaviour
{
    [Header("기본 설정")]
    public FishListManager fishListManager;     // 물고기 도감 상세정보창
    public GameObject fishSlot;                 // 도감용 물고기 창
    public DataContainer dataContainer;         // FishDataContainer 연결

    [System.Serializable]
    public struct RarityContainer
    {
        public EFish_Rarity rarity;
        public Transform container;
    }
    [Header("등급별 컨테이너 설정")]
    public List<RarityContainer> raritySettings;

    private Dictionary<EFish_Rarity, Transform> _containers;



    private void Awake()
    {
        _containers = new Dictionary<EFish_Rarity, Transform>();
        foreach (var settings in raritySettings)
        {
            if (settings.container != null && !_containers.ContainsKey(settings.rarity))
            {
                _containers.Add(settings.rarity, settings.container);
            }
        }

    }

    public void GenerateBook()
    {
        // 생성된 슬롯이 있으면 제거
        foreach (Transform container in _containers.Values)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }

        // 데이터 컨테이너 체크
        if (dataContainer == null)
        {
            return;
        }

        // 슬롯, 데이터 생성
        foreach (ScriptableObject obj in dataContainer.objs)
        {
            FishData fishData = obj as FishData;

            if (fishData == null) continue;

            if (_containers.TryGetValue(fishData.fishRarity, out Transform targetParent))
            {
                GameObject go = Instantiate(fishSlot, targetParent);
                go.GetComponent<FishSlot>()?.Setup(fishData, fishListManager);
            }
        }
    }
    private void Start()
    {
        GenerateBook();
    }
}
