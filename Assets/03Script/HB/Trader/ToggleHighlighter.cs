using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleHighlighter : MonoBehaviour
{
    [Header("연결할 하이라이트 오브젝트")]
    public GameObject highlightUI; 

    private Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        
        // 토글 값이 바뀔 때마다 실행될 리스너 등록
        _toggle.onValueChanged.AddListener(SetHighlight);
        
        // 시작 시 현재 토글 상태에 맞춰 초기화
        SetHighlight(_toggle.isOn);
    }

    // 외부에서도 호출 가능하도록 public으로 설정
    public void SetHighlight(bool isOn)
    {
        if (highlightUI != null)
        {
            highlightUI.SetActive(isOn);
        }
    }
}
