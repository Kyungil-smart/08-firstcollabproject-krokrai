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

    public List<int> SelectedGrade()
    {
        List<int> selected = new List<int>();
        for (int i = 0; i < gradeToggles.Length; i++)
        {
            if (gradeToggles[i].isOn) selected.Add(i + 1);
        }
        return selected;
    }
}
