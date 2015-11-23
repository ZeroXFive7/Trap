using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterSteering : MonoBehaviour
{
    static private readonly Vector3 gravityDirection = new Vector3(0.0f, -1.0f, 0.0f);

    [Header("Movement")]
    [SerializeField]
    private float maxMovementSpeed = 10.0f;
    [SerializeField]
    private float maxMovementAcceleration = 10.0f;
    [SerializeField]
    private float maxInAirMovementAcceleration = 2.0f;
    [SerializeField]
    private float timeToMaxMovementAcceleration = 0.1f;

    [Header("Rotation")]
    [SerializeField]
    private Transform aimingTransform = null;
    [SerializeField]
    private float minYawAngle;

    [Header("Gravity")]
    [SerializeField]
    private bool useGravity = true;
    [SerializeField]
    private float maxGravitySpeed = 10.0f;
    [SerializeField]
    private float maxGravityAcceleration = 10.0f;
    [SerializeField]
    private float timeToMaxGravityAcceleration = 0.1f;

    [Header("Jumping")]
    [SerializeField]
    private float jumpDuration = 1.0f;
    [SerializeField]
    private float jumpHeight = 2.0f;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    public bool IsJumping { get; private set; }

    private Vector3 movementVelocity = Vector3.zero;
    private Vector3 gravityVelocity = Vector3.zero;

    private float jumpTimeElapsed = 0.0f;
    private float jumpSpeed = 0.0f;

    private Vector3 moveTargetPosition = Vector3.zero;
    private float moveTargetDecelerationDistance = 0.0f;
    private float aimYaw = 0.0f;
    private float aimPitch = 0.0f;

    public void MoveTowards(Vector3 moveTarget, float decelerationDistance, bool snap = false)
    {
        moveTargetPosition = moveTarget;
        moveTargetDecelerationDistance = decelerationDistance;

        if (snap)
        {
            IsJumping = false;
            movementVelocity = Vector3.zero;
            gravityVelocity = Vector3.zero;
            jumpTimeElapsed = float.MaxValue;

            transform.position = moveTarget;
        }
    }

    public void Aim(float yaw, float pitch)
    {
        aimYaw = yaw;
        aimPitch = pitch;
    }

    public void Jump()
    {
        if (character.Collider.isGrounded)
        {
            IsJumping = true;
            jumpTimeElapsed = 0.0f;
            jumpSpeed = jumpHeight / jumpDuration;
        }
    }

    private void Start()
    {
        IsJumping = false;
        moveTargetPosition = transform.position;
    }

    private void Update()
    {
        UpdatePosition();
        UpdateOrientation();
    }

    private void UpdatePosition()
    {
        // Euler integration.
        Vector3 movementAcceleration = GetMovementAcceleration();
        if (useGravity && !character.Collider.isGrounded)
        {
            movementAcceleration = Vector3.ClampMagnitude(movementAcceleration, maxInAirMovementAcceleration);
        }
        movementVelocity += movementAcceleration * Time.deltaTime;
        movementVelocity = Vector3.ClampMagnitude(movementVelocity, maxMovementSpeed);

        if (useGravity)
        {
            if (IsJumping)
            {
                gravityVelocity = -gravityDirection * jumpSpeed;
                jumpTimeElapsed += Time.deltaTime;
            }
            else if (!character.Collider.isGrounded)
            {
                Vector3 gravityAcceleration = GetGravityAcceleration();
                gravityVelocity += gravityAcceleration * Time.deltaTime;
                gravityVelocity = Vector3.ClampMagnitude(gravityVelocity, maxGravitySpeed);
            }

            IsJumping = jumpTimeElapsed <= jumpDuration && !character.Collider.isGrounded;
        }

        character.Collider.Move((movementVelocity + gravityVelocity) * Time.deltaTime);
        character.Animator.LinearMovementSpeed = movementVelocity.magnitude;

    }

    private void UpdateOrientation()
    {
        // Yaw.
        float yaw = aimYaw * Time.deltaTime;
        transform.Rotate(Vector3.up * yaw, Space.Self);

        // Pitch.
        Vector3 previousPosition = aimingTransform.position;
        Quaternion previousRotation = aimingTransform.rotation;

        float pitch = aimPitch * Time.deltaTime;
        aimingTransform.RotateAround(transform.position, transform.right, pitch);

        // Avoid gimble lock by disallowing rotations that align forward vector with world up.
        float angleFromY = Vector3.Angle(aimingTransform.forward, Vector3.up);
        if (angleFromY < minYawAngle || angleFromY > 180.0f - minYawAngle)
        {
            aimingTransform.position = previousPosition;
            aimingTransform.rotation = previousRotation;
        }
    }

    private Vector3 GetMovementAcceleration()
    {
        Vector3 moveTargetDirection = moveTargetPosition - transform.position;
        float distanceToMoveTarget = moveTargetDirection.magnitude;

        float desiredSpeed = maxMovementSpeed;
        if (distanceToMoveTarget < moveTargetDecelerationDistance)
        {
            desiredSpeed *= distanceToMoveTarget / moveTargetDecelerationDistance;
        }

        Vector3 targetVelocity = moveTargetDirection.normalized * desiredSpeed;

        Vector3 acceleration = targetVelocity - movementVelocity;
        acceleration /= timeToMaxMovementAcceleration;
        acceleration = Vector3.ClampMagnitude(acceleration, maxMovementAcceleration);

        return acceleration;
    }

    private Vector3 GetGravityAcceleration()
    {
        Vector3 targetVelocity = gravityDirection * maxGravitySpeed;

        Vector3 acceleration = targetVelocity - gravityVelocity;
        acceleration /= timeToMaxGravityAcceleration;
        acceleration = Vector3.ClampMagnitude(acceleration, maxGravityAcceleration);

        return acceleration;
    }
}
