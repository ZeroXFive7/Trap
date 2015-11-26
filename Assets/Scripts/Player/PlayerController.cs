using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerCamera Camera = null;
    [HideInInspector]
    public Character Character = null;
    [HideInInspector]
    public PlayerUI UI = null;

    public int PlayerInputId
    {
        get
        {
            if (playerInput == null)
            {
                return -1;
            }
            return playerInput.id;
        }
        set
        {
            playerInput = Rewired.ReInput.players.GetPlayer(value);
        }
    }

    private Rewired.Player playerInput = null;

    private void Update()
    {
#if UNITY_EDITOR
        if (Cursor.visible)
        {
            return;
        }
#endif

        if (playerInput == null)
        {
            return;
        }

        // Move.
        Vector3 localMoveDirection = new Vector3(playerInput.GetAxis("Move Horizontal"), 0.0f, playerInput.GetAxis("Move Vertical"));
        Vector3 moveDirection = transform.TransformDirection(localMoveDirection);
        Character.Movement.Move(moveDirection);

        // Sprint.
        if (playerInput.GetButton("Sprint"))
        {
            Character.Movement.Sprint();
        }

        // Aim.
        Character.Aiming.Aim(playerInput.GetAxis("Look Horizontal"), playerInput.GetAxis("Look Vertical"));

        Camera.IsThirdPerson = !playerInput.GetButton("Toggle Perspective");
        Character.MeleeAttack.enabled = Camera.IsThirdPerson;
        Character.RangedAttack.enabled = !Camera.IsThirdPerson;

        // Jump.
        if (playerInput.GetButton("Jump"))
        {
            Character.Movement.Jump();
        }

        // Attack.
        if (playerInput.GetButton("Fire"))
        {
            if (Camera.IsThirdPerson)
            {
                Character.MeleeAttack.Attack();
            }
            else
            {
                Character.RangedAttack.Fire();
            }
        }

        // Dash.
        if (playerInput.GetButton("Dash"))
        {
            Vector3 dashDirection = Character.transform.forward;
            if (moveDirection.magnitude >= 0.01f)
            {
                dashDirection = moveDirection.normalized;
            }

            Character.Movement.Dash(dashDirection);
        }
    }
}
