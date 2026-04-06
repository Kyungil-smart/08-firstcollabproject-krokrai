using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FishListManager : MonoBehaviour
{
    [Header("FishData 연결")]
    public FishData currentFish;            // FishData 스크립트 연결

    [Header("이미지 어드레서블")]
    public AddressableImageLoader imageLoader;

    [Header ("도감 번호, 이름, 등급 데이터 연결")]
    public TextMeshProUGUI fishNumText;     // FishNum TMP 연결
    public TextMeshProUGUI fishNameText;    // FishName TMP 연결
    public TextMeshProUGUI fishRateText;    // FishRate TMP 연결
    

    [Header("Details 텍스트 오브젝트 연결")]
    public TextMeshProUGUI descriptionText;     // Details TMP 연결

    [Header("분류, 길이, 무게, 잡은 날짜 연결")]
    public TextMeshProUGUI groupText;       // Group TMP 연결
    public TextMeshProUGUI lengthText;      // Length TMP 연결
    public TextMeshProUGUI weightText;      // Weight TMP 연결
    public TextMeshProUGUI caughtDateText;  // CaughtData TMP 연결

    private void OnEnable()
    {
        if(DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged += LanguageChanged;
        }
        UpdateFishUI();
    }

    private void Start()
    {
        // 시작 시 초기화
        UpdateFishUI();
    }

    private void OnDisable()
    {
        if(DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged -= LanguageChanged;
        }

        // 상세창을 닫으면 메모리에서 이미지를 해제
        if (imageLoader != null)
        {
            imageLoader.SetImage(null);
        }
    }

    private void LanguageChanged(Language newLanguage)
    {
        UpdateFishUI();
    }

    public void UpdateFishUI()
    {
        if(currentFish == null) return;

        bool isCaught = currentFish.isCaught;

        // 현재 언어가 한국어인가
        bool isKor = DataTower.instance.languageSetting == Language.KOR;

        string displayName = isKor ? currentFish.korName : currentFish.engName;
        string displayDescription = isKor ? currentFish.korDescription : currentFish.engDescription;
        
        // 삼항 연산자 잡았으면 물고기 정보 띄우고 아니면 ???
        SafeLink(fishNumText, currentFish.fishID);
        SafeLink(fishNameText, isCaught ? displayName : "???");
        SafeLink(fishRateText, isCaught ? currentFish.fishRarity.ToString() : "???");
        SafeLink(groupText, isCaught ? currentFish.fishType.ToString() : "???");
        SafeLink(lengthText, isCaught ? $"{currentFish.length} cm" : "???");
        SafeLink(weightText, isCaught ? $"{currentFish.weight} kg" : "???");
        SafeLink(descriptionText, isCaught ? displayDescription : "???");
        SafeLink(caughtDateText, isCaught ? currentFish.caughtDate : "???");

        if (imageLoader != null)
        {
            // isCaught를 넘겨 로더 내부에서 색상을 처리
            imageLoader.SetImage(currentFish.fishSprite, isCaught);
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
        
        // 비활성화 상태면 활성화
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}
