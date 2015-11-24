using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private CharacterHealth health = null;
    [SerializeField]
    private CharacterMovement movement = null;
    [SerializeField]
    private CharacterAim aim = null;
    [SerializeField]
    private new CharacterController collider = null;
    [SerializeField]
    private MeleeAttack meleeAttack = null;
    [SerializeField]
    private CharacterAnimator animator = null;

    public CharacterHealth Health
    {
        get { return health; }
    }

    public CharacterMovement Movement
    {
        get { return movement; }
    }

    public CharacterAim Aim
    {
        get { return aim; }
    }

    public CharacterController Collider
    {
        get { return collider; }
    }

    public MeleeAttack MeleeAttack
    {
        get { return meleeAttack; }
    }

    public CharacterAnimator Animator
    {
        get { return animator; }
    }
}
