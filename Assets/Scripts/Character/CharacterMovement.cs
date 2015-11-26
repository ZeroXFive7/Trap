using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    [SerializeField]
    private float locomotionSpeed = 10.0f;
    [SerializeField]
    private float locomotionTimeToMaxAcceleration = 0.1f;
    [SerializeField]
    private float locomotionMaxAcceleration = 100.0f;
    [SerializeField]
    private float locomotionFallingAcceleration = 10.0f;

    [Header("Sprinting")]
    [SerializeField]
    private float sprintingSpeed = 20.0f;
    [SerializeField]
    private float maxSprintDuration = 10.0f;

    [Header("Dashing")]
    [SerializeField]
    private float dashSpeed = 3.0f;
    [SerializeField]
    private float dashDuration = 0.166f;
    [SerializeField]
    private float dashCooldown = 1.0f;

    [Header("Gravity")]
    [SerializeField]
    private Vector3 gravityAcceleration = new Vector3(0.0f, -10.0f, 0.0f);

    [Header("Jumping")]
    [SerializeField]
    private float jumpSpeed = 5.0f;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    private Vector3 movementVelocity = Vector3.zero;
    private Vector3 locomotionVelocity = Vector3.zero;
    private Vector3 impulseVelocity = Vector3.zero;
    private float outOfControlTimer = 0.0f;
    private float sprintTimer = 0.0f;
    private float dashCooldownTimer = 0.0f;

    public bool IsSprinting
    {
        get
        {
            return sprintTimer > 0.0f;
        }
    }

    public void SnapToPosition(Vector3 position)
    {
        movementVelocity = Vector3.zero;
        locomotionVelocity = Vector3.zero;
        impulseVelocity = Vector3.zero;

        transform.position = position;
    }

    public void Move(Vector3 moveDirection)
    {
        if (moveDirection.magnitude <= 0.01f)
        {
            sprintTimer = 0.0f;
        }

        locomotionVelocity = moveDirection.normalized;
    }

    public void Sprint()
    {
        sprintTimer = maxSprintDuration;
    }

    public void Jump()
    {
        if (character.Collider.isGrounded)
        {
            Impulse(-gravityAcceleration.normalized * jumpSpeed, 0.0f);
        }
    }

    public void Dash(Vector3 direction)
    {
        if (dashCooldownTimer <= 0.0f)
        {
            Impulse(direction * dashSpeed, dashDuration);
            dashCooldownTimer = dashCooldown;
        }
    }

    public void Impulse(Vector3 force, float outOfControlDuration)
    {
        impulseVelocity = force;
        outOfControlTimer = Mathf.Max(outOfControlTimer, outOfControlDuration);
    }

    private void Update()
    {
        if (character.Collider.isGrounded && outOfControlTimer <= 0.0f)
        {
            Vector3 targetMovementVelocity = locomotionVelocity * (IsSprinting ? sprintingSpeed : locomotionSpeed);
            Vector3 acceleration = targetMovementVelocity - movementVelocity;
            acceleration /= locomotionTimeToMaxAcceleration;
            acceleration = Vector3.ClampMagnitude(acceleration, locomotionMaxAcceleration);

            movementVelocity += acceleration * Time.deltaTime;

            character.Animator.LinearMovementSpeed = locomotionVelocity.magnitude;
        }
        else
        {
            Vector3 locomotionAcceleration = locomotionVelocity.normalized * locomotionFallingAcceleration;
            movementVelocity += (locomotionAcceleration + gravityAcceleration) * Time.deltaTime;

            character.Animator.LinearMovementSpeed = 0.0f;
        }

        movementVelocity += impulseVelocity;

        // Actually move body and resolve collisions.
        character.Collider.Move(movementVelocity * Time.deltaTime);

        // Update timers.
        DecrementTimer(ref outOfControlTimer);
        DecrementTimer(ref sprintTimer);
        DecrementTimer(ref dashCooldownTimer);

        // Reset state.
        locomotionVelocity = Vector3.zero;
        impulseVelocity = Vector3.zero;
    }

    private void DecrementTimer(ref float timer)
    {
        timer = Mathf.Max(0.0f, timer - Time.deltaTime);
    }
}
