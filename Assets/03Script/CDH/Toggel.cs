using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Toggel : MonoBehaviour
{
    [Header("Animation Setting")]
    public Animator menuAnimator;

    [Header("Arrow Image Setting")]
    public Image arrowImage;
    public Sprite openSprite;
    public Sprite closeSprite;

    private bool _isMenuOpen = false;

    public void OnToggleButtonClick()
    {
        _isMenuOpen = !_isMenuOpen;

        if (_isMenuOpen)
        { 
            menuAnimator.SetTrigger("OpenMenu");
        }
        else
        { 
            menuAnimator.SetTrigger("CloseMenu");
        }

        ChangeArrowImage();
    }

    private void ChangeArrowImage()
    {
        if (arrowImage == null || openSprite == null || closeSprite == null)
        {
            Debug.Log("이미지와 스프라이트가 연결되지 않았습니다.");
            return;
        }

        arrowImage.sprite = _isMenuOpen ? openSprite : closeSprite;
    }
}
