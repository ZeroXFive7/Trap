using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private CharacterHealth health = null;
    [SerializeField]
    private CharacterMovement movement = null;
    [SerializeField]
    private CharacterAiming aiming = null;
    [SerializeField]
    private new CharacterController collider = null;
    [SerializeField]
    private MeleeAttack meleeAttack = null;
    [SerializeField]
    private CharacterAnimator animator = null;
    [SerializeField]
    private new MeshRenderer renderer = null;

    public CharacterHealth Health
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

    public CharacterAnimator Animator
    {
        get { return animator; }
    }

    public MeshRenderer Renderer
    {
        get { return renderer; }
    }
}
