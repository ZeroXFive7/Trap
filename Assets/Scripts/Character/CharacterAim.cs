using UnityEngine;

public class CharacterAim : MonoBehaviour
{
    [SerializeField]
    private float minYawAngle = 10.0f;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    public void AimAt(float yaw, float pitch)
    {
        // Yaw.
        float deltaYaw = yaw * Time.deltaTime;
        transform.parent.Rotate(Vector3.up * deltaYaw, Space.Self);

        // Pitch.
        Vector3 previousPosition = transform.position;
        Quaternion previousRotation = transform.rotation;

        float deltaPitch = pitch * Time.deltaTime;
        transform.RotateAround(transform.parent.position, transform.parent.right, deltaPitch);

        // Avoid gimble lock by disallowing rotations that align forward vector with world up.
        float angleFromY = Vector3.Angle(transform.forward, Vector3.up);
        if (angleFromY < minYawAngle || angleFromY > 180.0f - minYawAngle)
        {
            transform.position = previousPosition;
            transform.rotation = previousRotation;
        }
    }
}
