using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;
    [SerializeField]
    private CharacterSteering steering;
    [SerializeField]
    private PlayerAiming aiming;

    public PlayerInput Input
    {
        get { return input; }
    }

    public CharacterSteering Steering
    {
        get { return steering; }
    }

    public PlayerAiming Aiming
    {
        get { return aiming; }
    }
}
