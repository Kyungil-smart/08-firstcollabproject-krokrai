using UnityEngine;

[CreateAssetMenu(fileName = "MenuUpgData", menuName = "Scriptable Objects/MenuUpgData")]
public class MenuUpgData : ScriptableObject, IDataSeter
{
    public int MenuPanelUpgLevel;
    public int MenuPanelUpgCost;
    public int MenuPanelEffectValue;

    public void SetData(string[] datas)
    {
        if(datas == null)
        {
            Debug.LogError($"{this.name}에 입력된 데이터가 없습니다.");
        }
        datas[0].TryParseForSO(out MenuPanelUpgLevel, this.name);
        datas[1].TryParseForSO(out MenuPanelUpgCost);
        datas[2].TryParseForSO(out MenuPanelEffectValue);
    }
}
