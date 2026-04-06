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

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

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

        if (DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged += LanguageChanged;
        }

        // 창이 실행될 때 기존에 있던 슬롯과 중첩되지 않게 다시 생성
        StopAllCoroutines();
        StartCoroutine(InitBookRoutine());
    }

    private void OnDisable()
    {
        if (DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged -= LanguageChanged;
        }       
    }

    private void LanguageChanged(Language language)
    {
        UpdateCompletionUI();
    }

    private IEnumerator InitBookRoutine()
    {
        // 하위 UI들이 배치된 후 다음 로직 실행
        GenerateBook();

        yield return _waitForEndOfFrame;

        UpdateLayouts();
        UpdateCompletionUI();
    }

    public void GenerateBook()
    {
        if(dataContainer == null || _containers == null) return;
        
        // 기존 모든 등급 컨테이너의 자식을 비활성화
        foreach (Transform container in _containers.Values)
        {
            foreach (Transform child in container)
            {
                child.gameObject.SetActive(false);
            }
        }

        // 등급별로 현재 사용 중인 슬롯 인덱스 추적
        Dictionary<EFish_Rarity, int> containerFishCount = new Dictionary<EFish_Rarity, int>();

        // 슬롯, 데이터 생성, 재활용
        foreach (ScriptableObject obj in dataContainer.objs)
        {
            FishData fishData = obj as FishData;

            if (fishData == null) continue;

            if (_containers.TryGetValue(fishData.fishRarity, out Transform targetParent))
            {
                // 등급을 처음 만났다면 0으로 초기화
            if (!containerFishCount.ContainsKey(fishData.fishRarity))
            {
                containerFishCount[fishData.fishRarity] = 0;
            }
            
            // 현재 이 등급의 몇 번째 슬롯을 처리할 차례인지
            int currentIndex = containerFishCount[fishData.fishRarity];
            GameObject slotGameObject;

            // 컨테이너에 자식 슬롯이 currentIndex보다 많다면 기존 걸 사용
            if (currentIndex < targetParent.childCount)
            {
                slotGameObject = targetParent.GetChild(currentIndex).gameObject;
                slotGameObject.SetActive(true);
            }

            else
            {
                slotGameObject = Instantiate(fishSlot, targetParent, false);
            }

            // 슬롯 셋업
            FishSlot slotScript = slotGameObject.GetComponent<FishSlot>();

            if(slotScript != null)
            {
                slotScript.SetupFishBook(fishData, fishListManager);
            }
            
            // 현재 등급 다음을 위해 카운트 증가
            containerFishCount[fishData.fishRarity]++;
            }
            
        }
        
    }
    private void UpdateLayouts()
    {
        // 하위 등급별 컨테이너부터 리빌드
        foreach (var container in _containers.Values)
        {
            // 화면에 켜져있는 등급만 리빌드
            if (container.gameObject.activeInHierarchy)
            {
                RectTransform rect = container.GetComponent<RectTransform>();

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
