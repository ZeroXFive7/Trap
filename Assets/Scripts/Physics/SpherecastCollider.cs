using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpherecastCollider : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float radius = 0.0f;

    private Vector3 previousPosition = Vector3.zero;

    public event System.Action<Transform, Vector3> Collision;

    private void OnEnable()
    {
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (Collision == null)
        {
            return;
        }

        Vector3 direction = transform.position - previousPosition;
        float distance = direction.magnitude;
        if (distance < 0.001f)
        {
            return;
        }
        else
        {
            direction /= distance;
        }

        RaycastHit[] hits = Physics.SphereCastAll(previousPosition, radius, direction, distance, layerMask.value);
        for (int i = 0; i < hits.Length; ++i)
        {
            Collision(hits[i].transform, previousPosition + direction * hits[i].distance);
        }

        previousPosition = transform.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Selection.activeTransform == transform || Selection.activeTransform == transform.parent)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
#endif
}
