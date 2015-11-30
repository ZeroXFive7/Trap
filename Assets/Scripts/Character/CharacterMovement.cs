using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Mass")]
    [SerializeField]
    private float mass = 1.0f;

    [Header("Locomotion")]
    [SerializeField]
    private float locomotionSpeed = 10.0f;
    [SerializeField]
    private float locomotionFallingAcceleration = 10.0f;

    [Header("Impulse")]
    [SerializeField]
    private float minImpulseMagnitude = 0.1f;
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

    private List<Vector3> impulseVelocities = new List<Vector3>();
    private List<int> impulseRemovalList = new List<int>();

    private float dashCooldownTimer = 0.0f;

    public void SnapToPosition(Vector3 position)
    {
        desiredLocomotionVelocity = Vector3.zero;
        locomotionVelocity = Vector3.zero;

        jumpVelocity = Vector3.zero;

        impulseVelocities.Clear();
        impulseRemovalList.Clear();

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
            Impulse(direction * dashSpeed);
            dashCooldownTimer = dashCooldown;
        }
    }

    public void Impulse(Vector3 force)
    {
        impulseVelocities.Add(force);
    }

    private void Update()
    {
        Vector3 movementVelocity = Vector3.zero;

        jumpVelocity += gravityAcceleration * Time.deltaTime;

        movementVelocity += jumpVelocity;
        movementVelocity += UpdateLocomotionVelocity();
        movementVelocity += UpdateImpulseVelocity();

        // Actually move body and resolve collisions.
        character.Collider.Move(movementVelocity * Time.deltaTime);

        if (character.Collider.isGrounded)
        {
            jumpVelocity = Vector3.zero;
        }

        dashCooldownTimer = Mathf.Max(0.0f, dashCooldownTimer - Time.deltaTime);
    }

    private Vector3 UpdateLocomotionVelocity()
    {
        if (character.Collider.isGrounded)
        {
            locomotionVelocity = desiredLocomotionVelocity;
        }
        else
        {
            locomotionVelocity += desiredLocomotionVelocity.normalized * locomotionFallingAcceleration * Time.deltaTime;
        }
        desiredLocomotionVelocity = Vector3.zero;

        return locomotionVelocity;
    }

    private Vector3 UpdateImpulseVelocity()
    {
        Vector3 impulseVelocity = Vector3.zero;

        impulseRemovalList.Clear();
        for (int i = 0; i < impulseVelocities.Count; ++i)
        {
            float magnitude = impulseVelocities[i].magnitude;
            if (magnitude > minImpulseMagnitude)
            {
                impulseVelocity += impulseVelocities[i];
                impulseVelocities[i] = impulseVelocities[i].normalized * Mathf.Max(0.0f, magnitude - impulseDeceleration * Time.deltaTime);
            }
            else
            {
                impulseRemovalList.Add(i);
            }
        }

        for (int i = impulseRemovalList.Count - 1; i >= 0; --i)
        {
            impulseVelocities.RemoveAt(i);
        }

        return impulseVelocity;
    }
}
