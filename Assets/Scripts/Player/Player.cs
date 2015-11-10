using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private new PlayerCamera camera;

    public PlayerInput Input
    {
        get { return input; }
    }

    public PlayerMovement Movement
    {
        get { return movement; }
    }

    public PlayerCamera Camera
    {
        get { return camera; }
    }
}
