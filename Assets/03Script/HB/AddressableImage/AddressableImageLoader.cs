using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;


public class AddressableImageLoader : MonoBehaviour
{
    [Header("단일 이미지 로드 시")]
    [SerializeField] private Image _targetImage;

    [Header("그룹으로 로드 할 때")]
    [SerializeField] private List<Image> _targetSlots = new List<Image>();
    private AsyncOperationHandle<Sprite> _handle;                   // 단일 이미지 로드
    private AsyncOperationHandle<IList<Sprite>> _groupHandle;       // 그룹 이미지 로드

    private void Awake()
    {
        // 인스펙터에서 안 끌어놔도 찾도록
        if(_targetImage== null)
        {
            _targetImage = GetComponent<Image>();
        } 
    }

    public void LoadImage(string address, bool preserveAspect = true)
    {
        // 주소 검증
        if (string.IsNullOrEmpty(address) || address == "NullException")
        {
            // 이미 부르고 있는 게 있다면 해제
            ResetImage();
            return;
        }

        // 기존에 로든된 게 있다면 해제
        ReleaseImage();

        // 어드레서블 로드
        _handle = Addressables.LoadAssetAsync<Sprite>(address);

        _handle.Completed += (operation) =>
        {
            // 오브젝트가 파괴되었거나 비활성화된 경우 처리 방지
            if (this == null || !gameObject.activeInHierarchy) return;

            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                _targetImage.sprite = operation.Result;
                _targetImage.preserveAspect = preserveAspect;
            }
        };
    }

    public void LoadByLabel(string labelName)
    {
        ReleaseGroup();

        // 전체 로드 되도록
        _groupHandle = Addressables.LoadAssetsAsync<Sprite>(labelName, null);

        _groupHandle.Completed += (op) =>
        {
            if (this == null || op.Status != AsyncOperationStatus.Succeeded) return;

            // 로드된 걸 리스트로 복사
            List<Sprite> sortedList = new List<Sprite>(op.Result);

            // 이름순으로 정렬
            sortedList.Sort((a, b) => string.Compare(a.name, b.name));

            // 정렬된 리스트를 슬롯에 순서대로 배치
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (i >= _targetSlots.Count) break;
            
                _targetSlots[i].sprite = sortedList[i];
                _targetSlots[i].preserveAspect = true;
            }
        };
    }

    public void ResetImage()
    {
        ReleaseImage();
        if (_targetImage != null)
        {
            _targetImage.sprite = null;
        }
    }

    public void ReleaseImage()
    {
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }
    }

    public void ReleaseGroup()
    {
        if (_groupHandle.IsValid())
        {
            Addressables.Release(_groupHandle);
        }

        foreach (var slot in _targetSlots)
        {
            if (slot != null) slot.sprite = null;
        }
    }

    private void OnDestroy()
    {
        ReleaseImage();
        ReleaseGroup();    
    }
}
