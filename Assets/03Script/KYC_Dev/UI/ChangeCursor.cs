using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] private Texture2D _normalCursor;
    [SerializeField] private Texture2D _controlSizeCursor;
    
    public void ChangeCursorNormal()
    {
        if (_normalCursor == null)
        {
            Debug.Log("Normal");
            return;
        }
        Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
    }

    public void ChangeCursorControlSize()
    {
        if (_controlSizeCursor == null)
        {
            Debug.Log("ControlSize");
            return;
        }
        Cursor.SetCursor(_controlSizeCursor, Vector2.zero, CursorMode.Auto);
    }
}
