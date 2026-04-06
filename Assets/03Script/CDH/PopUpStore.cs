using UnityEngine;

public class PopUpStore : MonoBehaviour
{
    public GameObject fisherView; // 낚시 진행 화면을 담당하는 오브젝트
    public GameObject restaurantView; // 레스토랑 진행 화면을 담당하는 오브젝트

    /// <summary>
    /// 레스토랑 화면 On 낚시화면 Off
    /// </summary>
    public void OpenStoreUI()
    {
        fisherView.SetActive(false);

        restaurantView.SetActive(true);
    }
    /// <summary>
    /// 레스토랑 화면 Off 낚시화면 On
    /// </summary>
    public void CloseStoreUI()
    {
        fisherView.SetActive(true);

        restaurantView.SetActive(false);
    }
}
