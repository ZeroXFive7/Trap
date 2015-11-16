using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private PlayerAiming aiming;

    public PlayerInput Input
    {
        get { return input; }
    }

    public PlayerMovement Movement
    {
        get { return movement; }
    }

    public PlayerAiming Aiming
    {
        get { return aiming; }
    }
}
