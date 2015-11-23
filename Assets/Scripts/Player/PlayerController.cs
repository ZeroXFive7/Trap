using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input = null;
    [SerializeField]
    private new PlayerCamera camera = null;
    [SerializeField]
    private Character character = null;

    [SerializeField]
    private float yawAimSpeed = 200.0f;
    [SerializeField]
    private float pitchAimSpeed = 200.0f;

    private void Update()
    {
        PlayerInput.InputData currentInput = input.CurrentInput;

        // Move.
        Vector3 steeringDirection = new Vector3(currentInput.Movement.x, 0.0f, currentInput.Movement.y);
        steeringDirection = transform.TransformDirection(steeringDirection);
        character.Steering.MoveTowards(transform.position + steeringDirection, 0.0f);

        // Aim.
        character.Steering.Aim(currentInput.Look.x * pitchAimSpeed, currentInput.Look.y * yawAimSpeed);

        camera.IsThirdPerson = !currentInput.AimDownSights;

        // Jump.
        if (currentInput.Jump)
        {
            character.Steering.Jump();
        }

        // Attack.
        if (currentInput.Attack)
        {
            character.MeleeAttack.Attack();
        }
    }
}
