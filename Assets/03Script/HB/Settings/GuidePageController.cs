using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuidePageController : MonoBehaviour
{
    [Header("데이터 바구니")]
    [SerializeField] private DataContainer _guideContainer;     // Guide컨테이너 참조

    [Header("UI 연결")]
    [SerializeField] private Image _displayImage;       // 가이드 띄워줄 화면
    [SerializeField] private TextMeshProUGUI _pageText; // 페이지를 나타내는 텍스트
    [SerializeField] private Button _leftButton;        // 이전 버튼
    [SerializeField] private Button _rightButton;       // 다음 버튼

    private int _currentIndex = 0;

    private void Start()
    {
        UpdateUI(0);
    }

    
    private void OnEnable()
    {
        // 오브젝트가 확성화 될 때 1페이지로 띄우기
        UpdateUI(0);
    }

    public void ShowNext() => UpdateUI(_currentIndex + 1);
    public void ShowPrev() => UpdateUI(_currentIndex -1);
    private void UpdateUI(int index)
    {
        // 컨테이너 속 SO리스트
        var pages = _guideContainer.objs;

        if(pages == null || pages.Length == 0) return;

        // 인덱스가 범위를 벗어나지 않게 고정
        _currentIndex = Mathf.Clamp(index, 0, pages.Length -1);

        // 현재 페이지 데이터를 가져와서 화면 갱신
        if(pages[_currentIndex] is GuidePageSO pageData)
        {
            _displayImage.sprite = pageData.guideImage;
            _pageText.text = $"{_currentIndex + 1} / {pages.Length}";
        }

        // 첫 페이지면 왼쪽 버튼 비활성화, 마지막이면 오른쪽 버튼 비활성화
        _leftButton.interactable = (_currentIndex > 0);
        _rightButton.interactable = (_currentIndex < pages.Length - 1);
    }
}
