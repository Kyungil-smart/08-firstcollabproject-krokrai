using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe 0", menuName = "Scriptable Objects/RecipeContainer")]
[Serializable]
public class RecipeContainer : ScriptableObject, IDataSeter
{
    public string recipe_ID;
    public ERecipe_Type recipe_Type;
    public string recipe_KName;
    public string recipe_EName;
    public int prices;
    public int yield;
    public string ingredient;
    public int count;
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
        recipe_Type = ChangeToEnum(datas[1]);
        recipe_KName = datas[2] != "" ? datas[2] : "NullException";
        recipe_EName = datas[3] != "" ? datas[3] : "NullException";
        if (!int.TryParse(datas[4], out prices))
            prices = -1;
        if (!int.TryParse(datas[5], out yield))
            yield = -1;
        ingredient = datas[6] != "" ? datas[6] : "NullException";
        if (!int.TryParse(datas[7], out count))
            count = -1;
        dish_Sprite = datas[8] != "" ? datas[8] : "NullException";
        silhouette_Sprite = datas[9] != "" ? datas[9] : "NullException";
        KDescription = datas[10] != "" ? datas[10] : "NullException";
        EDescription = datas[11] != "" ? datas[11] : "NullException";
    }

    ERecipe_Type ChangeToEnum(string s)
    {
        switch(s)
        {
            case "튀김":
                return ERecipe_Type.FRIED;
            case "삶음":
                return ERecipe_Type.BOILED;
            case "볶음":
                return ERecipe_Type.STIR_FRIED;
            case "구이":
                return ERecipe_Type.GRILLED;
            case "조림":
                return ERecipe_Type.BRAISED;
            case "회":
                return ERecipe_Type.RAW;
            default:
                return ERecipe_Type.Null;
        }
    }
}
