using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class ChangeCursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _normalCursor;
    [SerializeField] private Texture2D _expendCursor;
    
    /// <summary>
    /// 기본 커서의 형태를 변환할건지 체크 //
    /// T = 게임에서 지정한 형태로 변환 / F = 변환하지 않고 윈도우 설정 커서 사용
    /// </summary>
    public bool useOriginalCursor;

    private void Awake()
    {
        useOriginalCursor = false;
    }
    
    private void ChangeCursorNull()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    
    private void ChangeCursorNormal()
    {
        Vector2 hotSpot = new Vector2(_normalCursor.width / 11f, _normalCursor.height / 6f);
        Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// 커서를 기본형태로 바꿔주는 함수
    /// </summary>
    public void ChangeCursorBasic()
    {
        switch (useOriginalCursor)
        {
            case true:
                ChangeCursorNormal();
                break;
            case false:
                ChangeCursorNull();
                break;
        }
    }
    
    /// <summary>
    /// 커서를 확장 형태로 바꿔주는 함수
    /// </summary>
    public void ChangeCursorExpend()
    {
        Vector2 hotSpot = new Vector2(_expendCursor.width / 2f, _expendCursor.height / 2f);
        Cursor.SetCursor(_expendCursor, hotSpot, CursorMode.Auto);
    }

    
}
