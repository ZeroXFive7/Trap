using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    [SerializeField]
    private float locomotionSpeed = 10.0f;
    [SerializeField]
    private float locomotionFallingAcceleration = 10.0f;

    [Header("Impulse")]
    [SerializeField]
    private float impulseDeceleration = 10.0f;

    [Header("Sprinting")]
    [SerializeField]
    private float sprintingSpeed = 20.0f;

    [Header("Dashing")]
    [SerializeField]
    private float dashSpeed = 3.0f;
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

    private Vector3 desiredLocomotionVelocity = Vector3.zero;
    private Vector3 locomotionVelocity = Vector3.zero;

    private Vector3 jumpVelocity = Vector3.zero;
    private ImpulseSolver impulses = new ImpulseSolver();

    private float dashCooldownTimer = 0.0f;

    public void SnapToPosition(Vector3 position)
    {
        desiredLocomotionVelocity = Vector3.zero;
        locomotionVelocity = Vector3.zero;

        jumpVelocity = Vector3.zero;
        impulses.Clear();

        transform.position = position;
    }

    public void Move(Vector3 moveDirection, bool sprint)
    {
        desiredLocomotionVelocity = moveDirection.normalized * (sprint ? sprintingSpeed : locomotionSpeed);
    }

    public void Jump()
    {
        if (character.Collider.isGrounded)
        {
            jumpVelocity = -gravityAcceleration.normalized * jumpSpeed;
        }
    }

    public void Dash(Vector3 direction)
    {
        if (dashCooldownTimer <= 0.0f)
        {
            AddImpulse(direction * dashSpeed);
            dashCooldownTimer = dashCooldown;
        }
    }

    public void AddImpulse(Vector3 initialVelocity)
    {
        impulses.AddImpulseAndDeceleration(initialVelocity, impulseDeceleration);
    }

    public void AddImpulse(Vector3 initialVelocity, float duration)
    {
        impulses.AddImpulseForDuration(initialVelocity, duration);
    }

    private void Update()
    {
        jumpVelocity += gravityAcceleration * Time.deltaTime;

        if (character.Collider.isGrounded)
        {
            locomotionVelocity = desiredLocomotionVelocity;
            character.Animator.LinearMovementSpeed = desiredLocomotionVelocity.magnitude;
        }
        else
        {
            locomotionVelocity += desiredLocomotionVelocity.normalized * locomotionFallingAcceleration * Time.deltaTime;
            character.Animator.LinearMovementSpeed = 0.0f;
        }
        desiredLocomotionVelocity = Vector3.zero;

        impulses.Update(Time.deltaTime);

        // Actually move body and resolve collisions.
        Vector3 velocity = (jumpVelocity + locomotionVelocity) * Time.deltaTime;
        velocity += impulses.TotalVelocityThisFrame;
        character.Collider.Move(velocity);

        if (character.Collider.isGrounded)
        {
            jumpVelocity = Vector3.zero;
        }

        dashCooldownTimer = Mathf.Max(0.0f, dashCooldownTimer - Time.deltaTime);
    }
}
