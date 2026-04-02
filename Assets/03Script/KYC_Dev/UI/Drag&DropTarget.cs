using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragNDropTarget : MonoBehaviour
{
    public bool isMove;

    private void Awake()
    {
        isMove = false;
    }

    private void Update()
    {
        MovingPosition();
    }

    public void MovingPosition()
    {
        if(isMove)
        {
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            transform.position = currentPosition;
        }
    }
}
