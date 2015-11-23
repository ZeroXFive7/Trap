using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField]
    private MeleeWeapon prefab = null;
    [SerializeField]
    private Transform meleeWeaponOrigin = null;
    [SerializeField]
    private float cooldown = 0.1f;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    [HideInInspector]
    public MeleeWeapon MeleeWeapon
    {
        get; private set;
    }

    private float previousAttackTime = 0.0f;

    public void Attack()
    {
        if ((Time.time - previousAttackTime) >= cooldown)
        {
            character.Animator.MeleeAttack();
            previousAttackTime = Time.time;
        }
    }

    private void Awake()
    {
        MeleeWeapon = Instantiate(prefab);
        MeleeWeapon.transform.SetParent(meleeWeaponOrigin, false);
    }
}
