using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(Image))]
public class AddressableImageLoader : MonoBehaviour
{
    private Image _targetImage;
    private AsyncOperationHandle<Sprite> _handle;
    private string _lastAddress;

    void Awake()
    {
        if (_targetImage == null)
        {
            _targetImage = GetComponent<Image>();
        }
    }

    // 함수 가져다 쓰면 이미지 변경 가능-> 변수명.SetImage("이미지 이름");
    public void SetImage(string address, bool isCaught = true)
    {

        // 주소가 비었을 때 방어 코드
        if (string.IsNullOrEmpty(address))
        {
            ReleaseOldImage();
            _lastAddress = null;
            if(_targetImage != null) _targetImage.sprite = null;
            return;
        }
        
        // 중복 방지
        if (_lastAddress == address)
        {
            if(_targetImage != null)
            {
                _targetImage.color = isCaught ? Color.white : Color.black;
            }
            return;
        }

        // 기존 메모리 해제
        ReleaseOldImage();

        _lastAddress = address;


        // 비동기 로드 시작
        _handle = Addressables.LoadAssetAsync<Sprite>(address);
        _handle.Completed += (handle) =>
        {
            // 이미지가 로드되는 도중에 오브젝트가 파괴되었는지 확인 (에러 방지)
            if (this == null || _targetImage == null)
            {
                if(handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Addressables.Release(handle);
                }    
                return;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _targetImage.sprite = handle.Result;

                // 잡았다면 원래색, 못 잡았다면 검은색
                _targetImage.color = isCaught ? Color.white : Color.black;
            }

            else
            {
                Debug.LogWarning($"[Addressable] 로드 실패: {address}");
                _targetImage.sprite = null;
            }
        };
    }

    private void ReleaseOldImage()
    {
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }
    }

    // 오브젝트 파괴될 때 메모리 반납

    private void OnDestroy()
    {
        ReleaseOldImage();
    }
}
