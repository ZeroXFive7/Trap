using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private new PlayerCamera camera;

    public PlayerMovement Movement
    {
        get { return movement; }
    }

    public PlayerCamera Camera
    {
        get { return camera; }
    }
}
