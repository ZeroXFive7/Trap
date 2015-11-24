using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput Input = null;
    public PlayerCamera Camera = null;
    public Character Character = null;

    private void Update()
    {
        PlayerInput.InputData currentInput = Input.CurrentInput;

        // Move.
        Character.Movement.Move(new Vector3(currentInput.Movement.x, 0.0f, currentInput.Movement.y));

        // Aim.
        Character.Aiming.Aim(currentInput.Look.x, currentInput.Look.y);

        Camera.IsThirdPerson = !currentInput.AimDownSights;

        // Jump.
        if (currentInput.Jump)
        {
            Character.Movement.Jump();
        }

        // Attack.
        if (currentInput.Attack)
        {
            Character.MeleeAttack.Attack();
        }
    }
}
