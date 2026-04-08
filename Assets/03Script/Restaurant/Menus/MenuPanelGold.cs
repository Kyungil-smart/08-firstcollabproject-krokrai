using System.Collections;
using TMPro;
using UnityEngine;

public class MenuPanelGold : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _gold;

    private void OnEnable()
    {
        if (DataTower.instance == null)
        {
            StartCoroutine(Retry());
        }
        else
        {
            DataTower.instance.OnChangedMoney += SetGold;
            SetGold(DataTower.instance.money);
        }
    }

    IEnumerator Retry()
    {
        while (DataTower.instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDisable()
    {
        DataTower.instance.OnChangedMoney -= SetGold;
    }

    public void SetGold(ulong gold)
    {
        _gold.text = DataTower.instance.languageSetting == Language.KOR ? $"보유 금액 : {DataTower.instance.money} gold" : $"Money : {DataTower.instance.money} gold";
    }
}
