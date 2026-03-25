using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject
{
    [Header("물고기 포획, 해금 상태")]
    public bool isCaught;           // 물고기를 한 번이라도 잡았는가
    
    [Header("물고기 정보")]
    public string fishNum;          // 물고기 번호
    public string fishName;         // 물고기 이름
    public string fishRate;         // 물고기 등급
    public Sprite fishSprite;       // 물고기 이미지(유니티 프로그램내에서 첨부)

    public string groupName;        // 그룹
    public string length;           // 물고기 길이
    public string weight;           // 물고기 무게

    [Header("물고기 상세 설명")]

    public string infoButton;      // Information 버튼
    public string effectButton;    // Effect 버튼

    public void SetData(string[] datas)
    {
        if (datas.Length < 8) return;
        fishNum = datas[0];
        fishName = datas[1];
        fishRate = datas[2];
        groupName = datas[3];
        length = datas[4];
        weight = datas[5];
        infoButton = datas[6];
        effectButton = datas[7];
    }
    
}
