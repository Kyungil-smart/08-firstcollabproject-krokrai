using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timeScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Time.timeScale = 1;
        SetTimeScaleToTextMeshPro();
    }

    public void UpTime()
    {
        Time.timeScale += 0.5f;
        SetTimeScaleToTextMeshPro();
    }

    public void DownTime()
    {
        Time.timeScale -= 0.5f;
        SetTimeScaleToTextMeshPro();
    }

    private void SetTimeScaleToTextMeshPro()
    {
        _timeScale.text = $"시간 배율 : {Time.timeScale}";
    }
}
