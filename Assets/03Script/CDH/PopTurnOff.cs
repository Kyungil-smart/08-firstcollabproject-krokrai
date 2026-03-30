using UnityEngine;

public class PopTurnOff : MonoBehaviour
{
    public GameObject popTurnOffUi;
    public void OpenTurnOffUI()
    {
        popTurnOffUi.SetActive(true);
    }

    public void CloseTurnOffUI()
    {
        popTurnOffUi.SetActive(false);
    }
}
