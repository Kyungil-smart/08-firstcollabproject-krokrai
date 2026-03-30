using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class FishBookManager : MonoBehaviour
{
    [Header("기본 설정")]
    public FishListManager fishListManager;     // 물고기 도감 상세정보창
    public GameObject fishSlot;                 // 도감용 물고기 창
    public DataContainer dataContainer;         // FishDataContainer 연결
    public ScrollRect scrollRect;               // 스크롤바
    public TextMeshProUGUI fishBookCompletionText;  // 도감 완성도

    // 구조체가 인스펙터창에서 참조할 수 있도록
    [System.Serializable]
    public struct RarityContainer
    {
        public EFish_Rarity rarity;
        public Transform container;
    }

    [Header("등급별 컨테이너 설정")]
    public List<RarityContainer> raritySettings;

    // 탐색 속도를 위해 등급을 Key값으로 사용하는 딕셔너리
    private Dictionary<EFish_Rarity, Transform> _containers;

    private void Awake()
    {
        _containers = new Dictionary<EFish_Rarity, Transform>();

        // 인스펙터에서 설정한 리스트를 딕셔너리에 담아 속도 UP
        foreach (var settings in raritySettings)
        {
            if (settings.container != null && !_containers.ContainsKey(settings.rarity))
            {
                _containers.Add(settings.rarity, settings.container);
            }
        }
    }   

    // 도감 창을 닫았다가 다시 열었을 때 첫 페이지로 초기화
    private void OnEnable()
    {
        if(scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }

        // 창이 실행될 때 기존에 있던 슬롯과 중첩되지 않게 다시 생성
        StopAllCoroutines();
        StartCoroutine(InitBookRoutine());
    }

    private IEnumerator InitBookRoutine()
    {
        // 다음 프레임까지 대기
        yield return null;

        GenerateBook();
        UpdateCompletionUI();
    }

    public void GenerateBook()
    {
        if(dataContainer == null || _containers == null) return;
        
        // 생성된 슬롯이 있으면 제거
        foreach (Transform container in _containers.Values)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }

        // 슬롯, 데이터 생성
        foreach (ScriptableObject obj in dataContainer.objs)
        {
            FishData fishData = obj as FishData;

            if (fishData == null) continue;

            // 물고기 등급에 맞는 부모 컨테이너가 있는지 확인
            if (_containers.TryGetValue(fishData.fishRarity, out Transform targetParent))
            {
                // 슬롯 생성
                GameObject go = Instantiate(fishSlot, targetParent, false);

                // 생선된 슬롯에 물고기 데이터와 상세창 전달
                go.GetComponent<FishSlot>()?.SetupFishBook(fishData, fishListManager);
            }
        }
        
        Canvas.ForceUpdateCanvases();

        foreach(var container in _containers.Values)
        {
            RectTransform rect = container.GetComponent<RectTransform>();

            if (rect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
        }

        if (scrollRect != null && scrollRect.content != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        }

    }

    private void UpdateCompletionUI()
    {
        if (dataContainer == null || fishBookCompletionText == null) return;

        int totalFish = 0;
        int caughtCount = 0;

        // 물고기 SO 데이터 컨테이너를 확인
        foreach (ScriptableObject obj in dataContainer.objs)
        {  
            // FishData타입으로 형변환
            FishData fish = obj as FishData;

            if(fish != null)
            {
                totalFish++;

                if (fish.isCaught)
                {
                    caughtCount++;
                }
            }
        }
        fishBookCompletionText.text = $"{caughtCount} / {totalFish}";    

    }
}
