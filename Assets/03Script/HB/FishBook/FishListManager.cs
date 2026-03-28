using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("분류, 길이, 무게 연결")]
    public TextMeshProUGUI groupText;       // Group TMP 연결
    public TextMeshProUGUI lengthText;      // Length TMP 연결
    public TextMeshProUGUI weightText;      // Weight TMP 연결

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
        SafeLink(descriptionText, isCaught ? currentFish.korDescription : "아직 발견되지 않음");

        // 이미지 업데이트
        if (fishDisplayImage != null)
        {
            // 잡았다면 물고기 이미지, 못 잡았다면 실루엣
            Sprite displaySprite = isCaught ? currentFish.fishSprite : currentFish.silhouetteSprite;

            if (displaySprite != null)
            {
                fishDisplayImage.sprite = displaySprite;
                fishDisplayImage.color = Color.white;
            }
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

    /* 기획에서 사라진 버튼
    // Information 버튼 클릭 이벤트
    public void OnClickInformation()
    {
        if(currentFish != null)
        {
            detailsText.text = currentFish.korDescription;
        }
    }

    // Effect 버튼 클릭 이벤트
    public void OnClickEffect()
    {
        if(currentFish != null)
        {
            detailsText.text = currentFish.effectButton;
        }
    }
    */
}
