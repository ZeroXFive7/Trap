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

    public void Aim(float yaw, float pitch)
    {
        Vector2 aimRotation = new Vector2(pitch * pitchSpeed, yaw * yawSpeed);
        currentRotation += aimRotation * Time.deltaTime;
        currentRotation = ClampRotation(currentRotation);
    }

    public void Impulse(Vector2 impulse, float duration)
    {
        StartCoroutine(ImpulseCoroutine(impulse, duration));
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(currentRotation.x, 0.0f, 0.0f);
        transform.parent.rotation = Quaternion.Euler(0.0f, currentRotation.y, 0.0f);
    }

    private IEnumerator ImpulseCoroutine(Vector2 impulse, float duration)
    {
        Vector2 velocity = impulse;

        // Parabolic motion:
        // velocity_final = velocity_initial + acceleration * duration.
        // velocity_final = -velocity_initial.
        // acceleration = (-velocity_initial - velocity_initial) / duration.
        Vector2 acceleration = (-2.0f * velocity) / duration;

        float timer = 0.0f;
        while (timer < duration)
        {
            currentRotation += velocity * Time.deltaTime;
            currentRotation = ClampRotation(currentRotation);

            velocity += acceleration * Time.deltaTime;
            timer += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }
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
