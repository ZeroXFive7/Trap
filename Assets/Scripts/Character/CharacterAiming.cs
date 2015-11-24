using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField]
    private float yawSpeed = 200.0f;
    [SerializeField]
    private float pitchSpeed = 200.0f;
    [SerializeField]
    private float minYawAngle = 10.0f;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    public void Aim(float yaw, float pitch)
    {
        // Yaw.
        float deltaYaw = yaw * yawSpeed * Time.deltaTime;
        transform.parent.Rotate(Vector3.up * deltaYaw, Space.Self);

        // Pitch.
        Vector3 previousPosition = transform.position;
        Quaternion previousRotation = transform.rotation;

        float deltaPitch = pitch * pitchSpeed * Time.deltaTime;
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
