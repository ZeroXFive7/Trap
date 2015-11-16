using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    [Header("Gravity")]
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
    private Player player = null;
    [SerializeField]
    private CharacterController characterController = null;

    public bool IsJumping { get; private set; }

    private Vector3 movementVelocity = Vector3.zero;
    private Vector3 gravityVelocity = Vector3.zero;

    private float jumpTimeElapsed = 0.0f;
    private float jumpSpeed = 0.0f;

    private void Start()
    {
        IsJumping = false;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        UpdateJumping(deltaTime);

        // Euler integration.
        Vector3 movementAcceleration = GetMovementAcceleration();
        if (!characterController.isGrounded)
        {
            movementAcceleration = Vector3.ClampMagnitude(movementAcceleration, maxInAirMovementAcceleration);
        }
        movementVelocity += movementAcceleration * deltaTime;
        movementVelocity = Vector3.ClampMagnitude(movementVelocity, maxMovementSpeed);

        if (!characterController.isGrounded && !IsJumping)
        {
            Vector3 gravityAcceleration = GetGravityAcceleration();
            gravityVelocity += gravityAcceleration * deltaTime;
            gravityVelocity = Vector3.ClampMagnitude(gravityVelocity, maxGravitySpeed);
        }
        else if (IsJumping)
        {
            gravityVelocity = -gravityDirection * jumpSpeed;
        }

        characterController.Move((movementVelocity + gravityVelocity)* Time.deltaTime);
    }

    private void UpdateJumping(float deltaTime)
    {
        if (IsJumping)
        {
            IsJumping = jumpTimeElapsed <= jumpDuration && !characterController.isGrounded;
            jumpTimeElapsed += deltaTime;
        }
        else if (characterController.isGrounded && player.Input.Jump)
        {
            IsJumping = true;
            jumpTimeElapsed = 0.0f;
            jumpSpeed = jumpHeight / jumpDuration;
        }
    }

    private Vector3 GetMovementAcceleration()
    {
        Vector3 targetVelocity = new Vector3(player.Input.Movement.x, 0.0f, player.Input.Movement.y);
        targetVelocity = transform.TransformDirection(targetVelocity) * maxMovementSpeed;

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
