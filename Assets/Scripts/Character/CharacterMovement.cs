using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    static private readonly Vector3 gravityDirection = new Vector3(0.0f, -1.0f, 0.0f);

    [Header("Locomotion")]
    [SerializeField]
    private float maxLocomotionSpeed = 10.0f;
    [SerializeField]
    private float maxLocomotionSpeedInAir = 2.0f;
    [SerializeField]
    private float maxLocomotionAcceleration = 10.0f;
    [SerializeField]
    private float timeToMaxLocomotionAcceleration = 0.05f;

    [Header("Gravity")]
    [SerializeField]
    private bool useGravity = true;
    [SerializeField]
    private float maxGravitySpeed = 10.0f;
    [SerializeField]
    private float maxGravityAcceleration = 10.0f;
    [SerializeField]
    private float timeToMaxGravityAcceleration = 0.05f;

    [Header("Jumping")]
    [SerializeField]
    private float jumpDuration = 1.0f;
    [SerializeField]
    private float jumpHeight = 2.0f;

    [Header("Impulse")]
    [SerializeField]
    private float impulseDuration = 0.5f;
    [SerializeField]
    private float impulseMagnitude = 10.0f;
    [SerializeField]
    private float maxImpulseAcceleration = 10.0f;
    [SerializeField]
    private float timeToMaxImpulseAcceleration = 0.05f;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    public bool IsJumping { get; private set; }

    private Vector3 locomotionVelocity = Vector3.zero;
    private Vector3 gravityVelocity = Vector3.zero;
    private Vector3 impulseVelocity = Vector3.zero;

    private float jumpTimeElapsed = 0.0f;
    private float jumpSpeed = 0.0f;

    public void Move(Vector3 localMoveDirection)
    {
        locomotionVelocity += transform.TransformDirection(localMoveDirection) * maxLocomotionSpeed;
    }

    public void SnapToPosition(Vector3 position)
    {
        IsJumping = false;
        locomotionVelocity = Vector3.zero;
        gravityVelocity = Vector3.zero;
        impulseVelocity = Vector3.zero;
        jumpTimeElapsed = float.MaxValue;

        transform.position = position;
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

    public void Impulse(Vector3 direction)
    {
        impulseVelocity += direction * impulseMagnitude;
    }

    private void Start()
    {
        IsJumping = false;
    }

    private void Update()
    {
        Vector3 impulseDeceleration = GetImpulseAcceleration();
        impulseVelocity += impulseDeceleration * Time.deltaTime;

        // Euler integration.
        Vector3 locomotionAcceleration = GetLocomotionAcceleration();
        locomotionVelocity += locomotionAcceleration * Time.deltaTime;
        locomotionVelocity = Vector3.ClampMagnitude(locomotionVelocity, maxLocomotionSpeed);

        if (useGravity && !character.Collider.isGrounded)
        {
            locomotionVelocity = Vector3.ClampMagnitude(locomotionVelocity, maxLocomotionSpeedInAir);
        }
        else
        {
            locomotionVelocity = Vector3.ClampMagnitude(locomotionVelocity, maxLocomotionSpeed);

        }

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

        character.Collider.Move((locomotionVelocity + gravityVelocity + impulseVelocity) * Time.deltaTime);
        character.Animator.LinearMovementSpeed = locomotionVelocity.magnitude;
    }

    private Vector3 GetLocomotionAcceleration()
    {
        return GetAcceleration(Vector3.zero, locomotionVelocity, timeToMaxLocomotionAcceleration, maxLocomotionAcceleration);
    }

    private Vector3 GetGravityAcceleration()
    {
        Vector3 targetVelocity = gravityDirection * maxGravitySpeed;
        return GetAcceleration(targetVelocity, gravityVelocity, timeToMaxGravityAcceleration, maxGravityAcceleration);
    }

    private Vector3 GetImpulseAcceleration()
    {
        return GetAcceleration(Vector3.zero, impulseVelocity, timeToMaxImpulseAcceleration, maxImpulseAcceleration);
    }

    private Vector3 GetAcceleration(Vector3 desiredVelocity, Vector3 currentVelocity, float timeToMaxAcceleration, float maxAcceleration)
    {
        Vector3 acceleration = desiredVelocity - currentVelocity;
        acceleration /= timeToMaxAcceleration;
        acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);

        return acceleration;
    }
}
