using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TraderFilterUI : MonoBehaviour
{
    public GameObject filterPanel;                  // 필터 등급 창
    public TextMeshProUGUI filterButtonText;        // 필터 버튼의 텍스트
    public CanvasGroup mainContentUI;               // mainContent 오브젝트
    public Toggle selectAllToggle;                  // 전체 선택 토글

    public bool isFilterMode { get; private set; }  // 현재 필터창이 열려 있는지

    // 필터 버튼 클릭 시 실행 토글 On/Off
    public void ToggleFilter()
    {
        SetFilterUIMode(!isFilterMode);
    }

    public void ForceClose()
    {   
        SetFilterUIMode(false);
    }

    public void SetFilterUIMode(bool isFilter)
    {
        isFilterMode = isFilter;
        
        // 필터 패널 활성화/비활성화
        if(filterPanel != null)
        {
            filterPanel.SetActive(isFilterMode);
        }

        //버튼 텍스트 변경
        if (filterButtonText != null)
        {
            filterButtonText.text = isFilterMode ? "Confirm" : "Filter";
        }

        if (mainContentUI != null)
        {
            mainContentUI.interactable = !isFilterMode;
            mainContentUI.blocksRaycasts = !isFilterMode;
        }

        // 필터창이 열려 있을 때 전체 선택을 못 건들게
        if (selectAllToggle != null)
        {
            selectAllToggle.interactable = !isFilterMode;
        }
    }
}
