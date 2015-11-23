using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;

    private static readonly int isThirdPersonId = Animator.StringToHash("IsThirdPerson");
    private static readonly int linearMovementSpeedId = Animator.StringToHash("LinearMovementSpeed");
    private static readonly int meleeAttackId = Animator.StringToHash("MeleeAttack");

    public bool IsThirdPerson
    {
        set
        {
            animator.SetBool(isThirdPersonId, value);
        }
    }

    public float LinearMovementSpeed
    {
        set
        {
            animator.SetFloat(linearMovementSpeedId, value);
        }
    }

    public void MeleeAttack()
    {
        animator.SetTrigger(meleeAttackId);
    }
}
