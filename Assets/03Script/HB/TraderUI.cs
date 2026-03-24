using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraderUI : MonoBehaviour
{
    [Header("Filter UI Settings")]
    public GameObject filterPanel;              // 필터 등급 창
    public TextMeshProUGUI filterButtonText;    // 필터 버튼의 텍스트

    private bool isFilterMode = false;          // 현재 필터창이 열려 있는지

    public void OnFilterButtonClicked()
    {
        isFilterMode = !isFilterMode;

        if (isFilterMode)
        {
            // 필터창 열기
            filterPanel.SetActive(true);
            // 버튼 글자 변경
            filterButtonText.text = "Confirm";
        }
        else
        {
            // 필터창이 열려있는 경우
            filterPanel.SetActive(false);
            // 다시 필터 버튼으로 원복
            filterButtonText.text = "Filter";
        }
    }

    public void CloseTrader()
    {
        gameObject.SetActive(false);
    }
}
