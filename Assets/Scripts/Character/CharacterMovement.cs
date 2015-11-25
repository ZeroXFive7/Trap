using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    [SerializeField]
    private float locomotionSpeed = 10.0f;
    [SerializeField]
    private float locomotionFallingAcceleration = 10.0f;

    [Header("Sprinting")]
    [SerializeField]
    private float sprintingSpeed = 20.0f;

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

    public void SnapToPosition(Vector3 position)
    {
        movementVelocity = Vector3.zero;
        locomotionVelocity = Vector3.zero;
        impulseVelocity = Vector3.zero;

        transform.position = position;
    }

    public void Move(Vector3 localMoveDirection, bool sprint)
    {
        locomotionVelocity = transform.TransformDirection(localMoveDirection).normalized * (sprint ? sprintingSpeed : locomotionSpeed);
    }

    public void Jump()
    {
        if (character.Collider.isGrounded)
        {
            impulseVelocity = -gravityAcceleration.normalized * jumpSpeed;
        }
    }

    public void Impulse(Vector3 force)
    {
        impulseVelocity = force;
    }

    private void Update()
    {
        float linearMovementSpeed = 0.0f;

        if (character.Collider.isGrounded)
        {
            movementVelocity = locomotionVelocity;
            linearMovementSpeed = movementVelocity.magnitude;
        }
        else
        {
            Vector3 locomotionAcceleration = locomotionVelocity.normalized * locomotionFallingAcceleration;
            movementVelocity += (locomotionAcceleration + gravityAcceleration) * Time.deltaTime;
        }

        movementVelocity += impulseVelocity;

        character.Collider.Move(movementVelocity * Time.deltaTime);
        character.Animator.LinearMovementSpeed = linearMovementSpeed;

        // Reset state.
        locomotionVelocity = Vector3.zero;
        impulseVelocity = Vector3.zero;
    }
}
