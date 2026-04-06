using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Toggel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animation Setting")]
    public Animator menuAnimator;

    [Header("Arrow Image Setting")]
    public Image arrowImage;
    public Sprite openSprite; // 메뉴가 열려있을 때 이미지
    public Sprite closeSprite; // 메뉴가 닫혀있을 때 이미지

    [Header("Arrow Image Setting")]
    public Sprite openPressedSprite; // 메뉴가 열린 상태에서 눌렸을 때 이미지
    public Sprite closePressedSprite; // 메뉴가 닫힌 상태에서 눌렸을 때 이미지

    private bool _isMenuOpen = false; // 현재 메뉴의 개폐 상태

    /// <summary>
    /// 마우스 버튼을 눌렀을 때 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isMenuOpen)
        {
            arrowImage.sprite = openPressedSprite;
        }

        else
            arrowImage.sprite = closePressedSprite;
    }

    /// <summary>
    /// 마우스 버튼을 땠을 때 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        _isMenuOpen = !_isMenuOpen;

        if (_isMenuOpen)
        {
            menuAnimator.SetTrigger("OpenMenu");
        }

        else
            menuAnimator.SetTrigger("CloseMenu");

        ChangeArrowImage();
    }

    /// <summary>
    /// 메뉴 상태에 따라 이미지를 업데이트
    /// </summary>
    private void ChangeArrowImage()
    {
        if (arrowImage == null || openSprite == null || closeSprite == null)
        {
            Debug.Log("이미지와 스프라이트가 연결되지 않았습니다.");
            return;
        }

        if (_isMenuOpen)
        {
            arrowImage.sprite = openSprite;
        }
        else
        {
            arrowImage.sprite = closeSprite;
        }
    }
}
