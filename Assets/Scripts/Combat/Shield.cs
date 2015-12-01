using UnityEngine;
using System.Collections.Generic;

public class Shield : MonoBehaviour
{
    private struct ImpactPoint
    {
        public Vector3 LocalPosition;
        public float HoleRadius;
        public float FractureRadius;
        public float FalloffRate;
    };

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    private List<ImpactPoint> impactPoints = new List<ImpactPoint>();

    public Character Character
    {
        get { return character; }
    }

    public void Impact(Vector3 position, float impactForce)
    {
        float innerRadius = 0.1f;
        float outerRadius = 0.5f;

        impactPoints.Add(new ImpactPoint()
        {
            LocalPosition = transform.InverseTransformPoint(position),
            HoleRadius = innerRadius,
            FractureRadius = outerRadius,
            FalloffRate = 1.0f
        });
    }

    public void ClearImpacts()
    {
        impactPoints.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < impactPoints.Count; ++i)
        {
            Gizmos.DrawSphere(transform.TransformPoint(impactPoints[i].LocalPosition), impactPoints[i].HoleRadius);
        }
    }
}
