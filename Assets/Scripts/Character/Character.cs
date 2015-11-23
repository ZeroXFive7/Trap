using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private CharacterHealth health = null;
    [SerializeField]
    private CharacterSteering steering = null;
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

    public CharacterSteering Steering
    {
        get { return steering; }
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
