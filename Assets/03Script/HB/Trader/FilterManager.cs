using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterManager : MonoBehaviour
{
    [Header("등급을 순서대로 넣어주세요 (0:전체, 1:Trash...)")]
    public Toggle[] gradeToggles;

    // 무한 루프 방지 (코드로 값을 바꿀 때 리스너 무시)
    private bool _isInternalUpdating = false; 

    private void Awake()
    {
        // 각 토글에 리스너를 등록하여 실시간 상호작용
        for (int i = 0; i < gradeToggles.Length; i++)
        {
            int index = i;
            if (gradeToggles[i] != null)
            {
                gradeToggles[i].onValueChanged.AddListener((isOn) => OnToggleValueChanged(index, isOn));
            }
        }

        ResetFilter();
    }


    // 토글 값이 변할 때 전체/개별 필터의 상태를 자동으로 조절
    private void OnToggleValueChanged(int index, bool isOn)
    {
        if (_isInternalUpdating) return;

        _isInternalUpdating = true;

        // 전체 토글 클릭 시
        if (index == 0) 
        {
            if (isOn)
            {
                // 전체가 켜지면 나머지 모든 개별 등급 토글을 끔
                for (int i = 1; i < gradeToggles.Length; i++)
                {
                    if (gradeToggles[i] != null) gradeToggles[i].isOn = false;
                }
            }
            else
            {
                // 전체를 껐는데 아무것도 안 켜져 있으면 강제로 다시 켜지게 함
                bool anyOtherOn = false;
                for (int i = 1; i < gradeToggles.Length; i++)
                {
                    if (gradeToggles[i] != null && gradeToggles[i].isOn) { anyOtherOn = true; break; }
                }
                if (!anyOtherOn) gradeToggles[0].isOn = true;
            }
        }

        // 개별 등급 토글(1번 이상)을 건드렸을 때
        else 
        {
            if (isOn)
            {
                // 개별 등급이 하나라도 켜지면 전체 토글은 자동으로 끔
                if (gradeToggles[0] != null) gradeToggles[0].isOn = false;
            }
            else
            {
                // 만약 모든 개별 토글이 꺼졌다면 다시 전체를 킴
                bool anyOtherOn = false;
                for (int i = 1; i < gradeToggles.Length; i++)
                {
                    if (gradeToggles[i] != null && gradeToggles[i].isOn) { anyOtherOn = true; break; }
                }
                if (!anyOtherOn && gradeToggles[0] != null) gradeToggles[0].isOn = true;
            }
        }

        // 로직 종료
        _isInternalUpdating = false; 
    }

    // 무한루프 방지 및 필터 리스트 반환
    public List<EFish_Rarity> GetSelectedRates()
    {
        List<EFish_Rarity> selected = new List<EFish_Rarity>();

        // 전체 토글이 켜져 있는 경우 모든 등급 추가
        if (gradeToggles[0].isOn)
        {
            // Legendary까지 모든 Enum 값을 리스트에 담음
            for (int i = (int)EFish_Rarity.Trash; i <= (int)EFish_Rarity.Legendary; i++)
            {
                selected.Add((EFish_Rarity)i);
            }
            return selected;
        }

        // 개별 토글 확인
        for (int i = 1; i < gradeToggles.Length; i++)
        {
            if (gradeToggles[i].isOn)
            {
                // 인덱스 1번이 Trash(0)이므로 i - 1
                selected.Add((EFish_Rarity)(i - 1));
            }
        }

        return selected;
    }

    // 필터 초기화 함수
    public void ResetFilter()
    {
        if (gradeToggles == null || gradeToggles.Length == 0) return;
        
        // 리스너 동작 일시 정지
        _isInternalUpdating = true;

        // 모든 토글을 끕니다.
        foreach (Toggle toggle in gradeToggles)
        {
            if (toggle != null) toggle.isOn = false;
        }

        // 0번(전체) 토글만 다시 켭니다.
        if (gradeToggles[0] != null)
        {
            gradeToggles[0].isOn = true;
        }

        // 리스너 동작 재개
        _isInternalUpdating = false; 
    }
}
