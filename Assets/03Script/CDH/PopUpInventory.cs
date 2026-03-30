using UnityEngine;

public class PopUpInventory : MonoBehaviour
{
    public GameObject popInventoryUi;

    public void OpenInventoryUI()
    {
        popInventoryUi.SetActive(true);
    }

    public void CloseInventoryUI()
    {
        popInventoryUi.SetActive(false);
    }
}