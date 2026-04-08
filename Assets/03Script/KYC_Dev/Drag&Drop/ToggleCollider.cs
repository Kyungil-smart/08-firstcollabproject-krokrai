using UnityEngine;

public class ToggleCollider : MonoBehaviour
{
    [SerializeField] private Collider2D[] _colliders;
    
    public void EnableColliders()
    {
        foreach (Collider2D collider in _colliders)
        {
            collider.enabled = true;
        }
    }

    public void DisableColliders()
    {
        foreach (Collider2D collider in _colliders)
        {
            collider.enabled = false;
        }
    }
}
