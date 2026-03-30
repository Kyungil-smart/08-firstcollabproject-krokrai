using UnityEngine;

public class PopSetUp : MonoBehaviour
{
    public GameObject setUpUi;
    public void OpenSetUpUI()
    {
        setUpUi.SetActive(true);
    }

    public void CloseSetUpUI()
    {
        setUpUi.SetActive(false);
    }
}
