using System;
using UnityEngine;

public static class FishEnumKorean
{
    public static string ToName(this EFish_Type type)
    {
        // DataTower가 없으면 영어로
        Language language = DataTower.instance != null ? DataTower.instance.languageSetting : Language.ENG;

        if (language == Language.KOR)
        {
            return type switch
            {
                EFish_Type.Fish => "물고기",
                EFish_Type.Item => "아이템",
                EFish_Type.Seaweed => "해초류",
                EFish_Type.Garbage => "쓰레기",
                _ => "알 수 없음"
            };
        }

        else
        {
            return type.ToString();
        }
    }

    public static string ToName(this EFish_Rarity rarity)
    {
        Language language = DataTower.instance != null ? DataTower.instance.languageSetting : Language.ENG;
    
        if (language == Language.KOR)
        {
            return rarity switch
            {
                EFish_Rarity.Trash => "쓰레기",
                EFish_Rarity.Normal => "일반",
                EFish_Rarity.Fine => "우수",
                EFish_Rarity.Superior => "고급",
                EFish_Rarity.Rare => "희귀",
                EFish_Rarity.Elite => "명품",
                EFish_Rarity.Fantastic => "환상",
                EFish_Rarity.Legendary => "전설",
                _ => "없음"
            };
        }

        else
        {
            return rarity.ToString();
        }
    }
}
