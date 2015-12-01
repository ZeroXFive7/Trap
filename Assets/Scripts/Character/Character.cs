using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Health health = null;
    [SerializeField]
    private CharacterMovement movement = null;
    [SerializeField]
    private CharacterAiming aiming = null;
    [SerializeField]
    private new CharacterController collider = null;
    [SerializeField]
    private MeleeAttack meleeAttack = null;
    [SerializeField]
    private RangedAttack rangedAttack = null;
    [SerializeField]
    private Shield shield = null;
    [SerializeField]
    private CharacterAnimator animator = null;
    [SerializeField]
    private new MeshRenderer renderer = null;

    public Health Health
    {
        get { return health; }
    }

    public CharacterMovement Movement
    {
        get { return movement; }
    }

    public CharacterAiming Aiming
    {
        get { return aiming; }
    }

    public CharacterController Collider
    {
        get { return collider; }
    }

    public MeleeAttack MeleeAttack
    {
        get { return meleeAttack; }
    }

    public RangedAttack RangedAttack
    {
        get { return rangedAttack; }
    }

    public Shield Shield
    {
        get { return shield; }
    }

    public CharacterAnimator Animator
    {
        get { return animator; }
    }

    public MeshRenderer Renderer
    {
        get { return renderer; }
    }
}
