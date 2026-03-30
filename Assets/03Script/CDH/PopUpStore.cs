using UnityEngine;

public class PopUpStore : MonoBehaviour
{
    public GameObject fisherView;
    public GameObject restaurantView;

    public void OpenStoreUI()
    {
        fisherView.SetActive(false);

        restaurantView.SetActive(true);
    }

    public void CloseStoreUI()
    {
        fisherView.SetActive(true);

        restaurantView.SetActive(false);
    }
}
