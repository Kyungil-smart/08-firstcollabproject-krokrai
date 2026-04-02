using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableGroupManager : MonoBehaviour
{
    [Header("설정")]
    public string labelName = "Fish";    // 어드레서블에서 설정한 라벨 이름
    public GameObject slotPrefab;         // FishSlot 또는 AddressableImageLoader가 붙은 프리팹
    public Transform contentParent;       // 슬롯이 배치될 부모

    // 메모리 관리를 위한 핸들 보관함
    private List<AsyncOperationHandle> _loadHandles = new List<AsyncOperationHandle>();

    void Start()
    {
        LoadAndSortByLabel();
    }

    public void LoadAndSortByLabel()
    {
        // 해당 라벨을 가진 에셋들의 위치 정보 먼저 가져옴
        Addressables.LoadResourceLocationsAsync(labelName, typeof(Sprite)).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // 주소 이름을 기준으로 가나다/숫자순 정렬
                var sortedLocations = handle.Result.OrderBy(l => l.PrimaryKey).ToList();


                // 정렬된 명단 순서대로 슬롯을 하나씩 만듦
                foreach (IResourceLocation location in sortedLocations)
                {
                    CreateSlot(location);
                }
            }
            else
            {
                Debug.LogError($"[Manager] {labelName} 라벨을 찾을 수 없음");
            }
        };
    }

    private void CreateSlot(IResourceLocation location)
    {
        // 슬롯 프리팹 생성
        GameObject slotObj = Instantiate(slotPrefab, contentParent);
        
        // 슬롯에 붙은 로더 컴포넌트 가져오기
        var loader = slotObj.GetComponent<AddressableImageLoader>();
        
        if (loader != null)
        {
            // 정렬된 주소를 로더에게 전달하여 이미지를 그림
            loader.SetImage(location.PrimaryKey); 
        }
    }

    private void OnDestroy()
    {
        // 생성된 모든 에셋 핸들을 반납
        foreach (var h in _loadHandles)
        {
            if (h.IsValid()) Addressables.Release(h);
        }
    }
}
