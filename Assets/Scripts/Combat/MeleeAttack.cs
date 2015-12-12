using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField]
    private MeleeWeapon prefab = null;
    [SerializeField]
    private Transform meleeWeaponOrigin = null;
    [SerializeField]
    private float duration = 0.1f;
    [SerializeField]
    private float cooldown = 0.1f;
    [SerializeField]
    private float impulseMagnitude = 20.0f;
    [SerializeField]
    private float attackRangeSpherecastRadius = 0.1f;
    [SerializeField]
    private float attackRangeSpherecastDistance = 0.5f;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    private float previousAttackTime = 0.0f;

    private HashSet<Character> charactersHitThisAttack = new HashSet<Character>();

    [HideInInspector]
    public MeleeWeapon MeleeWeapon
    {
        get; private set;
    }

    public bool CharacterInAttackRange { get; private set; }

    public void EnableMeleeColliders()
    {
        MeleeWeapon.CollidersEnabled = true;
    }

    public void DisableMeleeColliders()
    {
        MeleeWeapon.CollidersEnabled = false;
    }

    public void Attack()
    {
        if (!enabled)
        {
            return;
        }

        if ((Time.time - previousAttackTime) >= cooldown)
        {
            previousAttackTime = Time.time;
            charactersHitThisAttack.Clear();
            character.Animator.MeleeAttack();
        }
    }

    private void Awake()
    {
        MeleeWeapon = Instantiate(prefab);
        MeleeWeapon.transform.SetParent(meleeWeaponOrigin, false);
        MeleeWeapon.CollidedWithCharacter += OnWeaponCollidedWithCharacter;

        CharacterInAttackRange = false;
    }

    private void OnEnable()
    {
        meleeWeaponOrigin.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        meleeWeaponOrigin.gameObject.SetActive(false);
    }

    private void Update()
    {
        CharacterInAttackRange = false;

        Vector3 attackDirection = character.Aiming.transform.forward;
        Vector3 attackOrigin = character.Aiming.transform.position;

        RaycastHit[] hits = Physics.SphereCastAll(
            attackOrigin, 
            attackRangeSpherecastRadius, 
            attackDirection, 
            attackRangeSpherecastDistance);

        for (int i = 0; i < hits.Length; ++i)
        {
            BodyCollider characterBody = hits[i].transform.GetComponent<BodyCollider>();
            if (characterBody == null || characterBody.Character == this.character)
            {
                continue;
            }

            CharacterInAttackRange = true;
            break;
        }
    }

    private void OnWeaponCollidedWithCharacter(Character character, Vector3 impactPoint)
    {
        if (character == this.character || charactersHitThisAttack.Contains(character))
        {
            return;
        }

        charactersHitThisAttack.Add(character);

        Vector3 impactDirection = (character.transform.position - transform.position).normalized;
        float angle = MathExtensions.AngleAroundAxis(transform.forward, impactDirection, transform.up);
        float popAngle = MeleeWeapon.KnockbackPopAngleCurve.Evaluate(angle);
        float knockbackForce = MeleeWeapon.KnockbackForceCurve.Evaluate(angle);

        Vector3 knockbackDirection = Quaternion.AngleAxis(-popAngle, transform.right) * Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        character.Movement.AddImpulse(knockbackDirection * knockbackForce);
    }
}
