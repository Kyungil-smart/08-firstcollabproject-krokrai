using UnityEngine;

public class TestSetImage : MonoBehaviour
{
    public AddressableImageLoader imageLoader;

    [ContextMenu("Test_Load_Fish1")]
    public void TestLoadFish()
    {
        imageLoader.SetImage("Fish_1", true);
    }

    [ContextMenu("Test_Load_Fish2")]
    public void TestLoadFish2()
    {
        imageLoader.SetImage("Fish_2", true);
    }

    [ContextMenu("Test_Load_Fish3")]
    public void TestLoadFish3()
    {
        imageLoader.SetImage("Fish_3", true);
    }

    [ContextMenu("Test_Load_Fish4")]
    public void TestLoadFish4()
    {
        imageLoader.SetImage("Fish_4", true);
    }
}
