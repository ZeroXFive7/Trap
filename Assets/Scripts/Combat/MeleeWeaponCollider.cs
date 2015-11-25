using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeleeWeaponCollider : MonoBehaviour
{
    [SerializeField]
    private float radius = 0.0f;

    private Vector3 previousPosition = Vector3.zero;

    public RaycastHit[] Collisions { get; private set; }

    private void OnEnable()
    {
        previousPosition = transform.position;
    }

    private void OnDisable()
    {
        Collisions = null;
    }

    private void Update()
    {
        Vector3 direction = transform.position - previousPosition;
        float distance = direction.magnitude;
        if (distance > 0.0f)
        {
            direction /= distance;
        }

        Collisions = Physics.SphereCastAll(previousPosition, radius, direction, distance);
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
