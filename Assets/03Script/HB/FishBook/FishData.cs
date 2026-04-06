using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject, IDataSeter
{
    [Header("물고기 포획, 해금 상태")]
    public bool isCaught;           // 물고기를 한 번이라도 잡았는가
    
    [Header("물고기 정보")]
    public string fishID;               // 물고기 번호
    public string korName;              // 이름(한국어)
    public string engName;              // 이름(영어)
    public EFish_Rarity fishRarity;     // 물고기 등급
    public EFish_Type fishType;         // 종류
    public float length;                  // 물고기 길이
    public float weight;                  // 물고기 무게
    public string fishSprite;           // 물고기 이미지(유니티 프로그램내에서 첨부)
    public string silhouetteSprite;     // 실루엣 이미지
    public int price;                   // 물고가 판매 가격(소상인)
    public string korDescription;       // 물고기 설명(한국어)
    public string engDescription;       // 물고기 설명(영어)

    public string caughtDate;            // 물고기 잡은 날짜

    public void SetData(string[] datas)
    {
        if (datas == null)
        {
            Debug.Log("읽어드린 정보가 없습니다. Null Exception");
            return;
        }

        // 기획서 순서대로 작성
        // 물고기 아이디, 한글 이름, 영어 이름
        fishID = datas[0] != "" ? datas[0] : "NullException";
        korName = datas[1] != "" ? datas[1] : "NullException";
        engName = datas[2] != "" ? datas[2] : "NullException";

        // Enum 변환
        // 등급, 종류
        fishRarity = ChangeToFishRarityEnum(datas[3]);
        fishType = ChangeToFishTypeEnum(datas[4]);

        // int, float 타입 parsing
        // 길이, 무게, 생선 판매 가격
        if (!float.TryParse(datas[5], out length)) length = -1f;
        if (!float.TryParse(datas[6], out weight)) weight = -1f;
        if (!int.TryParse(datas[8], out price)) price = -1;

        // 이미지 어드레서블사용 (string)
        fishSprite = datas[7] != "" ? datas[7] : "NullException";
        //silhouetteSprite = datas[8] != "" ? datas[8] : "NullException";

        // 설명(한, 영)
        korDescription = datas[9] != "" ? datas[9] : "NullException";
        engDescription = datas[10] != "" ? datas[10] : "NullException";
    }
    
    EFish_Rarity ChangeToFishRarityEnum(string s)
    {
        switch (s)
        {
            case "쓰레기": case "Trash":
                return EFish_Rarity.Trash;
            case "일반": case "Normal":
                return EFish_Rarity.Normal;
            case "우수": case "Fine":
                return EFish_Rarity.Fine;
            case "고급": case "Superior":
                return EFish_Rarity.Superior;
            case "희귀": case "Rare":
                return EFish_Rarity.Rare;
            case "명품": case "Elite":
                return EFish_Rarity.Elite;
            case "환상": case "Fantastic":
                return EFish_Rarity.Fantastic;
            case "전설": case "Legendary":
                return EFish_Rarity.Legendary;
            default: 
                return EFish_Rarity.Null;
        }
    }

    EFish_Type ChangeToFishTypeEnum(string s)
    {
        switch (s)
        {
            case "물고기": case "Fish":
                return EFish_Type.Fish;
            case "아이템": case "Item":
                return EFish_Type.Item;
            case "해초류": case "Seaweed":
                return EFish_Type.Seaweed;
            case "쓰레기": case "Trash":
                return EFish_Type.Garbage;
            default: 
                return EFish_Type.Null;
        }
    }

    // 인스펙터 테스트용
    [ContextMenu("TestCatch")]

    public void CatchFishDate()
    {
        // 처음 잡았을 때만 실행
        if (!isCaught)
        {
            isCaught = true;
            caughtDate = System.DateTime.Now.ToString("yyyy.MM.dd");
            Debug.Log($"{korName} 포획, 잡은 날짜: {caughtDate}");
        }
    }
}
