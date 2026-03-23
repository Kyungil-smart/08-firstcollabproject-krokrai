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

        // 물고기 번호, 이름, 등급 정보 전달
        fishNumText.text = currentFish.fishNum;
        fishNameText.text = currentFish.fishName;
        fishRateText.text = currentFish.fishRate;

        // 물고기 이미지 정보 전달
        fishDisplayImage.sprite = currentFish.fishSprite;

        // 물고기 종류, 길이, 무게 정보 전달
        groupText.text = currentFish.groupName;
        lengthText.text = currentFish.length;
        weightText.text = currentFish.weight;

        // 상세 설명 부분 '설명(information)'버튼의 내용으로 초기화
        detailsText.text = currentFish.infoButton;
    }

    // 테스트용 코드
    public void ChangeFishData(FishData newData)
    {
        Debug.Log("버튼 눌림");
        if (newData == null) return;

        currentFish = newData;
        UpdateFishUI();
        Debug.Log($"{newData.fishName}으로 교체");
    }

    // Information 버튼 클릭 이벤트
    public void OnClickInformation()
    {
        if(currentFish != null)
        {
            detailsText.text = currentFish.infoButton;
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
}
