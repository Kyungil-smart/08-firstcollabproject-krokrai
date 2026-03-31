using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// 이미지 없는 곳에 붙이는 것 방지
[RequireComponent(typeof(Image))]
public class AddressableImageLoader : MonoBehaviour
{
    [Header("이미지 컴포넌트")]
    [SerializeField] private Image _targetImage;
    private AsyncOperationHandle<Sprite> _handle;

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

    private void OnDestroy()
    {
        ReleaseImage();        
    }
}
