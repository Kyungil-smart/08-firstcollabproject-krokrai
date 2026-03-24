using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TempWidgetMask : MonoBehaviour
{
    [Header("실제 표시 방식 설정")]
    [Tooltip("Sprite Mask 입력")][SerializeField] GameObject[] mask;
    [SerializeField] GameObject _backgroundSprite;
    [SerializeField] GameObject _fishingBote;

    [SerializeField] Image _image;

    GameObject _currentmask;

    bool t;

    // 창은 그대로 두고 실제로 보여지는 부분만 조절하게 하는건 어떤지?
    // 힘든 경우에는 실제 창 크기를 줄여서 작업하자.
    // 카메라를 이용해서 UI와 상호작용해 찍는범위를 정하자.

    // 현재 가능할 거 같은 방법 : layermask를 이용한 방법. Max Min 정도는 문제 없이 작동할 거 같은 느낌.
    // 크기 조절관련은. 버튼 또는 마우스 다운 및 업 상태를 체크해서 실시간으로 크기를 바꾸는 것.
    // 최소 값 이하로는 내려가지 않게 예외 처리 필요.
    // 프로파일링으로 성능 검사가 필요할 거 같으나, 현재 생각으로는 큰 문제가 없을 거라고 예상함.
    // 현재 평가 : 가장 합리적임.

    // 수평은 배, 수직은 배경과 배로 움직일 수 있게, 작업 필요
    // 이동 방식은 UI에 이미지(아무거나 상관없)를 드래그 형태로 하며,
    // 화면 또는 특정 범위 밖으로 나가지 않게 작업 필수.



    private void Start()
    {
        t = true;
        _currentmask = mask[0];
    }

    public void changeMask()
    {
        t = !t;
        _currentmask?.SetActive(false);
        _currentmask = t ? mask[0] : mask[1];
        _currentmask?.SetActive(true);
    }
}
