using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Temp_Item", menuName = "Scriptable Objects/Temp_Item")]
public class Temp_Item : ScriptableObject
{
    public int ID;
    public string Name;
    [field:TextArea] public string Description;
    
    /// <summary>
    /// 아이템 등급 분류 숫자로 표기
    /// ToDo:세부 사항은 기획에 맞춰 변경
    /// </summary>
    public int Rarity;
    
    /// <summary>
    /// 아이템 종류 숫자로 표기
    /// ToDo:세부 사항은 기획에 맞춰 변경
    /// </summary>
    public int ItemType;
    public Sprite ItemIcon;
}
