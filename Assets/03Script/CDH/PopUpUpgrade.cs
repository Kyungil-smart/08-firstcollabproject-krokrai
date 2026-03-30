using UnityEngine;

public class PopUpUpgrade : MonoBehaviour
{
    public GameObject popUpgradeUi;
    public void OpenUpgradeUI()
    {
        popUpgradeUi.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        popUpgradeUi.SetActive(false);
    }
}
