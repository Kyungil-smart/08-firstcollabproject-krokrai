using UnityEngine;
using UnityEngine.InputSystem;

public class TempWidgetMask : MonoBehaviour
{
    [SerializeField] GameObject[] mask;
    [SerializeField] GameObject _backgroundSprite;
    [SerializeField] GameObject _fishingBote;

    GameObject currentmask;

    bool t;

    // 창은 그대로 두고 실제로 보여지는 부분만 조절하게 하는건 어떤지?
    // 힘든 경우에는 실제 창 크기를 줄여서 작업하자.
    // 카메라를 이용해서 UI와 상호작용해 찍는범위를 정하자.

    // 현재 가능할 거 같은 방법 : layermask를 이용한 방법. Max Min 정도는 문제 없이 작동할 거 같은 느낌.
    // 크기 조절관련은. 버튼 또는 마우스 다운 및 업 상태를 체크해서 실시간으로 크기를 바꾸는 것.
    // 최소 값 이하로는 내려가지 않게 예외 처리 필요.
    // 프로파일링으로 성능 검사가 필요할 거 같으나, 현재 생각으로는 큰 문제가 없을 거라고 예상함.
    // 현재 평가 : 가장 합리적임.

    private void Start()
    {
        t = true;
        currentmask = mask[0];
    }

    public void changeMask()
    {
        t = !t;
        currentmask?.SetActive(false);
        currentmask = t ? mask[0] : mask[1];
        currentmask?.SetActive(true);
    }
}
