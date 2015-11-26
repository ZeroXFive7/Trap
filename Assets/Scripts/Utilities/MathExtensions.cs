using UnityEngine;

public static class MathExtensions
{
    public static float AngleAroundAxis(Vector3 from, Vector3 to, Vector3 rotationAxis)
    {
        from = Vector3.ProjectOnPlane(from, rotationAxis);
        to = Vector3.ProjectOnPlane(to, rotationAxis);

        return Vector3.Angle(from, to) * RotationAroundAxisDirection(from, to, rotationAxis);
    }

    public static float RotationAroundAxisDirection(Vector3 from, Vector3 to, Vector3 rotationAxis)
    {
        return Mathf.Sign(Vector3.Dot(Vector3.Cross(from, to), rotationAxis));
    }
}
