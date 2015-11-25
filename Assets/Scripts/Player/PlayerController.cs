using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput Input = null;
    public PlayerCamera Camera = null;
    public Character Character = null;

    [SerializeField]
    private float dashForce = 5.0f;
    [SerializeField]
    private float dashTime = 0.2f;

    private void Update()
    {
        PlayerInput.InputData currentInput = Input.CurrentInput;

        // Move.
        Vector3 moveDirection = transform.TransformDirection(new Vector3(currentInput.Movement.x, 0.0f, currentInput.Movement.y));
        Character.Movement.Move(moveDirection, currentInput.Sprint);

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
            //Character.Movement.Impulse(moveDirection * dashForce, dashTime);
            Character.MeleeAttack.Attack();
        }

        Camera.Reticle.HighlightRed = Character.MeleeAttack.CharacterInAttackRange;
    }
}
