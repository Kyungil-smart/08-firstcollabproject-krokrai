using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [Header("각 탭의 화면들")]
    [SerializeField] private List<GameObject> _panels = new List<GameObject>();

    private void OnEnable()
    {
        // 창이 켜질 때 사운드 페이지로 초기화
        OpenTab(0);
    }

    // 버튼클릭 시 인덱스 번호로 전달
    public void OpenTab(int tabIndex)
    {
        for (int i = 0; i < _panels.Count; i++)
        {
            // 전달받은 번호와 일치하는 화면 켜고 나머지는 끔
            _panels[i].SetActive(i == tabIndex);
        }
    }
}
