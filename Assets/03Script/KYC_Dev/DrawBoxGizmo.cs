using System;
using UnityEngine;

public class DrawBoxGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position,transform.localScale);
    }
}
