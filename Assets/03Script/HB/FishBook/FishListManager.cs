using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class FishListManager : MonoBehaviour
{
    [Header("FishData 연결")]
    public FishData currentFish;            // FishData 스크립트 연결

    [Header ("도감 번호, 이름, 등급 데이터 연결")]
    public TextMeshProUGUI fishNumText;     // FishNum TMP 연결
    public TextMeshProUGUI fishNameText;    // FishName TMP 연결
    public TextMeshProUGUI fishRateText;    // FishRate TMP 연결
    public Image fishDisplayImage;          // FishImage Image 연결

    [Header("Details 텍스트 오브젝트 연결")]
    public TextMeshProUGUI descriptionText;     // Details TMP 연결

    [Header("분류, 길이, 무게, 잡은 날짜 연결")]
    public TextMeshProUGUI groupText;       // Group TMP 연결
    public TextMeshProUGUI lengthText;      // Length TMP 연결
    public TextMeshProUGUI weightText;      // Weight TMP 연결
    public TextMeshProUGUI caughtDateText;  // CaughtData TMP 연결

    private AsyncOperationHandle<Sprite> _displayHandle;

    private void Start()
    {
        // 시작 시 초기화
        UpdateFishUI();
    }

    public void UpdateFishUI()
    {
        if(currentFish == null) return;

        bool isCaught = currentFish.isCaught;
        
        // 삼항 연산자 잡았으면 물고기 정보 띄우고 아니면 ???
        SafeLink(fishNumText, currentFish.fishID);
        SafeLink(fishNameText, isCaught ? currentFish.korName : "???");
        SafeLink(fishRateText, isCaught ? currentFish.fishRarity.ToString() : "???");
        SafeLink(groupText, isCaught ? currentFish.fishType.ToString() : "???");
        SafeLink(lengthText, isCaught ? $"{currentFish.length} cm" : "???");
        SafeLink(weightText, isCaught ? $"{currentFish.weight} kg" : "???");
        SafeLink(descriptionText, isCaught ? currentFish.korDescription : "???");
        SafeLink(caughtDateText, isCaught ? currentFish.caughtDate : "???");

        // 이미지 업데이트
        if (fishDisplayImage != null)
        {
            // 다른 물고기 클릭 시  이전 물고기는 메모리에서 지움
            if (_displayHandle.IsValid())
            {
                Addressables.Release(_displayHandle);
            }    

            // 시트에서 이미지 파일 주소(string)을 확인
            string address = isCaught ? currentFish.fishSprite :currentFish.silhouetteSprite; 

            // 주소가 없거나 "NullException"이면 돌아가기
            if (string.IsNullOrEmpty(address) || address == "NullException")
            {
                Debug.Log("이미지 주소가 비었습니다.");
                return;
            }

            // 주소가 있을 때만 이미지 불러오기
            _displayHandle = Addressables.LoadAssetAsync<Sprite>(address);    
            // UI에 배치
            _displayHandle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {   
                    // 로드된 이미지 적용
                    fishDisplayImage.sprite = operation.Result;
                }
            };
                   
        }
    }

    private void SafeLink(TextMeshProUGUI tmp, string content)
    {
        if (tmp != null)
        {
            tmp.text = content;
        }
    }

    
    public void ChangeFishData(FishData newData)
    {
        Debug.Log("버튼 눌림");
        if (newData == null) return;

        currentFish = newData;
        UpdateFishUI();
        
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if(_displayHandle.IsValid())
        {
            Addressables.Release(_displayHandle);
        }
    }
}
