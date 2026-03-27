using UnityEngine;

public class ToStore : MonoBehaviour
{
    public GameObject fisherView;
    public GameObject restaurantView;

    public void OpenStore()
    {
        fisherView.SetActive(false);

        restaurantView.SetActive(true);
    }

    public void CloseStore()
    {
        fisherView.SetActive(true);

        restaurantView.SetActive(false);
    }
}
