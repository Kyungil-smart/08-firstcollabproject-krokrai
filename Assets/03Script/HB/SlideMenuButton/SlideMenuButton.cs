using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideMenuButton : MonoBehaviour
{
    [System.Serializable]
    public struct SlideData
    {
        public RectTransform rect;      // 화살표 버튼
        public Vector2 closedPos;       // 닫혔을 때 위치
        public Vector2 openedPos;         // 열렸을 때 위치
    }

    [Header("슬라이드 데이터")]
    [SerializeField] private List<SlideData> _buttonList = new List<SlideData>();
    [SerializeField] private float _moveSpeed = 0.5f;           // 슬라이드 움직임 속도
    [SerializeField] private float _buttonDelay = 0.05f;        // 버튼 간 딜레이

    private bool _isOpen = false;
    public bool _isMoving = false;

    public void ToggleMenu()
    {
        // 버튼이 slide될 때는 중복 실행 하지않음
        if(_isMoving) return;

        StartCoroutine(ButtonSlideRoutine(!_isOpen));
    }

    // 전체 버튼들이 순차적으로 이동
    private IEnumerator ButtonSlideRoutine(bool targetState)
    {
        _isMoving = true;

        // 닫힐 때 리스트를 뒤집어서 가까운 버튼 부터 들어감
        if (!targetState) _buttonList.Reverse();

        for(int i = 0; i < _buttonList.Count; i++)
        {
            // 각 버튼의 개별 이동 코루틴
            StartCoroutine(IndividualSlide(_buttonList[i],targetState));

            // 다음 버튼이 출발하기 전까지 대기
            yield return new WaitForSeconds(_buttonDelay);
        }

        // 리스트 순서를 원복
        if(!targetState) _buttonList.Reverse();

        // 모든 버튼이 이동할 때까지 대기
        yield return new WaitForSeconds((_buttonList.Count * _buttonDelay) + _moveSpeed);

        _isOpen = targetState;
        _isMoving = false;
    }

    // 개별 버튼의 이동, 활성화 상태 제어
    private IEnumerator IndividualSlide (SlideData data, bool targetState)
    {
        // 열릴 때 이동 시작 전 오브젝트 활성화
        if(targetState) data.rect.gameObject.SetActive(true);

        float elapsed = 0f;
        Vector2 startPos = data.rect.anchoredPosition;
        Vector2 endPos = targetState ? data.openedPos : data.closedPos;

        while (elapsed < _moveSpeed)
        {
            elapsed += Time.deltaTime;

            // 시작과 끝 부드럽게 감속
            float normalizedTime = Mathf.SmoothStep(0, 1, elapsed / _moveSpeed);
            data.rect.anchoredPosition = Vector2.Lerp(startPos, endPos, normalizedTime);
            yield return null;
        }

        // 최종 목적지 좌표 고정
        data.rect.anchoredPosition = endPos;

        // 닫힐 때 이동이 완전히 완료된 후 비활성화
        if (!targetState) data.rect.gameObject.SetActive(false);
    }
}
