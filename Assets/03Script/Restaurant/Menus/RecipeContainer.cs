using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe 0", menuName = "Scriptable Objects/RecipeContainer")]
[Serializable]
public class RecipeContainer : ScriptableObject, IDataSeter
{
    public string recipe_ID;
    public EFish_Rarity recipeRarity;
    public string recipe_KName;
    public string recipe_EName;
    public int prices;
    public int yield;
    public string ingredient;
    public string dish_Sprite;
    public string silhouette_Sprite;
    public string KDescription;
    public string EDescription;

    public void SetData(string[] datas)
    {
        if (datas == null)
        {
            Debug.Log("읽어드린 정보가 없습니다. Null Exception");
            return;
        }

        recipe_ID = datas[0] != "" ? datas[0] : "NullException" ;
        recipeRarity = ChangeToEnum(datas[1]);
        recipe_KName = datas[2] != "" ? datas[2] : "NullException";
        recipe_EName = datas[3] != "" ? datas[3] : "NullException";
        if (!int.TryParse(datas[4], out prices))
            prices = -1;
        if (!int.TryParse(datas[5], out yield))
            yield = -1;
        ingredient = datas[6] != "" ? datas[6] : "NullException";
        dish_Sprite = datas[7] != "" ? datas[7] : "NullException";
        silhouette_Sprite = datas[8] != "" ? datas[8] : "NullException";
        KDescription = datas[9] != "" ? datas[9] : "NullException";
        EDescription = datas[10] != "" ? datas[10] : "NullException";
    }

    
    EFish_Rarity ChangeToEnum(string s)
    {
        switch(s)
        {
            case "Trash":
                return EFish_Rarity.Trash;
            case "Normal":
                return EFish_Rarity.Normal;
            case "Fine":
                return EFish_Rarity.Fine;
            case "Superior":
                return EFish_Rarity.Superior;
            case "Rare":
                return EFish_Rarity.Rare;
            case "Elite":
                return EFish_Rarity.Elite;
            case "Fantastic":
                return EFish_Rarity.Fantastic;
            case "Legendary":
                return EFish_Rarity.Legendary;
            default:
                return EFish_Rarity.Null;
        }
    }
}
