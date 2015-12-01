using UnityEngine;
using System.Collections;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField]
    private float yawSpeed = 200.0f;
    [SerializeField]
    private float pitchSpeed = 200.0f;
    [SerializeField]
    private float minYawAngle = -360.0f;
    [SerializeField]
    private float maxYawAngle = 360.0f;
    [SerializeField]
    private float minPitchAngle = -70.0f;
    [SerializeField]
    private float maxPitchAngle = 70.0f;
    [SerializeField]
    private float impulseDeceleration = 5.0f;

    private Vector2 currentRotation = Vector2.zero;

    private ImpulseSolver impulses = new ImpulseSolver();

    public Vector2 CurrentRotation
    {
        get { return currentRotation; }
    }

    public void Aim(float yaw, float pitch)
    {
        Vector2 aimRotation = new Vector2(pitch * pitchSpeed, yaw * yawSpeed);
        currentRotation += aimRotation * Time.deltaTime;
        currentRotation = ClampRotation(currentRotation);
    }

    public void Impulse(Vector2 initialVelocity, float duration)
    {
        impulses.AddImpulseForDuration(initialVelocity, duration, true);
    }

    private void Update()
    {
        impulses.Update(Time.deltaTime);

        currentRotation += (Vector2)impulses.TotalVelocityThisFrame;
        currentRotation = ClampRotation(currentRotation);

        transform.localRotation = Quaternion.Euler(currentRotation.x, 0.0f, 0.0f);
        transform.parent.rotation = Quaternion.Euler(0.0f, currentRotation.y, 0.0f);
    }

    private Vector3 ClampRotation(Vector3 rotation)
    {
        return new Vector3(
            ClampAngle(rotation.x, minPitchAngle, maxPitchAngle), 
            ClampAngle(rotation.y, minYawAngle, maxYawAngle), 
            0.0f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle %= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
