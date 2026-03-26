using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FilterManager : MonoBehaviour
{
    [Header("등급을 순서대로 넣어주세요")]
    public Toggle[] gradeToggles;
    
    // 무한루프 방지
    private bool _isUpdating = false;

    private void Awake()
    {
        for (int i = 0; i < gradeToggles.Length; i++)
        {
            int index = i;
            gradeToggles[i].onValueChanged.AddListener((isOn) => ToggleValueChanged(index, isOn));
        }
    }
    private void OnEnable()
    {
        ResetFilter();
    }

    public void ResetFilter()
    {
        if (gradeToggles == null || gradeToggles.Length == 0) return;

        _isUpdating = true;
        for(int i = 0; i < gradeToggles.Length; i++)
        {
            gradeToggles[i].isOn = (i == 0);
        }
        _isUpdating = false;

        Debug.Log("A(전체)만 선택됨");
    }

    private void ToggleValueChanged(int index, bool isOn)
    {
        if (_isUpdating || !isOn) return;

        _isUpdating = true;
        
        // A를 선택한 경우 B~I는 선택 해제
        if (index == 0)
        {
            for (int i = 1; i < gradeToggles.Length; i++)
            {
                gradeToggles[i].isOn = false;
            }
        }
        // B~I 중 하나를 선택한 경우 A해제
        else
        {
            gradeToggles[0].isOn = false;
        }

        _isUpdating = false;
    }

    public List<EFish_Rarity> SelectedRates()
    {
        List<EFish_Rarity> selected = new List<EFish_Rarity>();
        
        // 맨 위에 있는 A(전체) 버튼은 1부터 매칭해서 모든 등급을 다 반환
        if (gradeToggles[0].isOn)
        {
            selected.Add(EFish_Rarity.Trash);
            selected.Add(EFish_Rarity.Normal);
            selected.Add(EFish_Rarity.Fine);
            selected.Add(EFish_Rarity.Superior);
            selected.Add(EFish_Rarity.Rare);
            selected.Add(EFish_Rarity.Elite);
            selected.Add(EFish_Rarity.Fantastic);
            selected.Add(EFish_Rarity.Legendary);
            return selected;
        }

        // A버튼이 아니라면 선택된 것만 반환
        for (int i = 1; i < gradeToggles.Length; i++)
        {
            if (gradeToggles[i].isOn)
            {
                selected.Add((EFish_Rarity)(i-1));
            }
        }
        
        return selected;
    }
}
