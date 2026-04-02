using System;
using UnityEngine;
using UnityEngine.UI;
using Kirurobo;
using UnityEngine.Serialization;

public class TouckLockModuleController : MonoBehaviour
{
    [Header("씬에 있는거 넣을 것")]
    [SerializeField] UniWindowController _uniWindowController;
    
    [Header("잠금 화면 때 보여질 캔버스들에 해당 컴포넌트 넣고 목록에 추가할것")]
    [SerializeField] CanvasGroup[] _canvasGroups;
    
    [Header("잠금 화면 때 보여질 캔버스 안에 있는 모든 이미지적 요소 목록에 추가")]
    [SerializeField] Image[] _images;
    
    [Header("잠금화면때 보여지지 않을 UI 요소(프리팹 최 상단) 넣을 것")] 
    [SerializeField] GameObject[] _closeUis;

    [Header("잠금 상태 최대 투명도")]
    [SerializeField] [Range(0.3f, 0.95f)] float _capOpacity;
    [Tooltip("이미지끼리 겹치면 투명도가 의도대로 적용 안되서 추가 보정치 필요")]
    [SerializeField] [Range(0.05f, 0.25f)] float _calibrationOpacity;

    private float _tempOpacity;
    
    /// <summary>
    /// ToDo:데이터 타워에 투명도 조절 변수 들어오면 지울 것
    /// </summary>
    private float _testOpacity;

    private void Awake()
    {
        gameObject.SetActive(false);
        _tempOpacity = -1;
        _testOpacity = 1f;
    }

    public void TouchLockEnable()
    {
        LockInvoke();
    }
    
    public void TouchLockDisable()
    {
        UnlockInvoke();
    }

    private void LockInvoke()
    {
        gameObject.SetActive(true);
        
        // ToDo : 데이터 타워에 투명도 조절 변수 들어오면 수정 할것
        if (_testOpacity > _capOpacity)
        {
            _tempOpacity = _testOpacity;
        }
        
        foreach (CanvasGroup canvas in _canvasGroups)
        {
            if (canvas.alpha > _capOpacity)
            {
                canvas.alpha = _capOpacity;
            }
            canvas.interactable = false;
        }

        foreach (Image image in _images)
        {
            if (image.color.a > _capOpacity)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, _capOpacity-_calibrationOpacity);
            }
        }

        foreach (GameObject ui in _closeUis)
        {
            ui.SetActive(false);
        }
        _uniWindowController.opacityThreshold = 0.99f;
    }

    private void UnlockInvoke()
    {
        foreach (CanvasGroup canvas in _canvasGroups)
        {
            if (_tempOpacity > 0)
            {
                canvas.alpha = _tempOpacity;
            }
            canvas.interactable = true;
        }
        
        foreach (Image image in _images)
        {
            if (_tempOpacity > 0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, _tempOpacity);
            }
        }
        _uniWindowController.opacityThreshold = 0.3f;
        _tempOpacity = -1f;
        
        gameObject.SetActive(false);
    }
    
}
