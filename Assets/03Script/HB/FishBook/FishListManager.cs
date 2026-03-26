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
    public TextMeshProUGUI detailsText;     // Details TMP 연결

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
        if(currentFish == null)
        {
            return;
        }

        // 물고기를 잡은 경우 도감에 정보가 공개
        if (currentFish.isCaught)
        {
            // 물고기 번호, 이름, 등급 정보 전달
            fishNumText.text = currentFish.fishID;
            fishNameText.text = currentFish.korName;
            fishRateText.text = currentFish.fishRarity.ToString();

            // 물고기 이미지 정보 전달
            fishDisplayImage.sprite = currentFish.fishSprite;
            fishDisplayImage.color = Color.white;   // 원래 이미지로 표시

            // 물고기 종류, 길이, 무게 정보 전달
            groupText.text = currentFish.fishType.ToString();
            lengthText.text = $"{currentFish.length} cm";
            weightText.text = $"{currentFish.weight} kg";

            // 상세 설명 부분 '설명(information)'버튼의 내용으로 초기화
            detailsText.text = currentFish.korDescription;
        }
        // 못 잡은 경우 정보가 공개되지 않음
        else
        {
            // 물고기 번호, 이름, 등급 정보
            fishNumText.text = currentFish.fishID;
            fishNameText.text = "???";              // 이름 숨김
            fishRateText.text = "???";              // 등급 숨김

            // 물고기 이미지 정보
            if (currentFish.silhouetteSprite != null)
            {
                fishDisplayImage.sprite = currentFish.silhouetteSprite;
            }

            groupText.text = "???";
            lengthText.text = "???";
            weightText.text = "???";
            detailsText.text = "아직 발견되지 않은 물고기입니다.";
        }
    }

    // 테스트용 코드
    public void ChangeFishData(FishData newData)
    {
        Debug.Log("버튼 눌림");
        if (newData == null) return;

        currentFish = newData;
        UpdateFishUI();
        Debug.Log($"{newData.korName}으로 교체");
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
