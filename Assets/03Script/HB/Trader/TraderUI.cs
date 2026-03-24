using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraderUI : MonoBehaviour
{
    [Header("Filter UI Settings")]
    public GameObject filterPanel;              // 필터 등급 창
    public FilterManager filterManager;
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
            // 처음 열 때 A만 체크되도록 초기화 호출
            filterManager.ResetFilter();
        }
        else
        {
            // 닫을 때 필터링해서 UI에 띄우기
            ApplyFilter();
            // 필터창이 열려있는 경우
            filterPanel.SetActive(false);
            // 다시 필터 버튼으로 원복
            filterButtonText.text = "Filter";
        }
    }

    private void ApplyFilter()
    {
        var SelectedGrade = filterManager.SelectedGrade();
        Debug.Log($"선택된 등급 개수: {SelectedGrade.Count}개");
    }

    public void CloseTrader()
    {
        gameObject.SetActive(false);
    }
}
