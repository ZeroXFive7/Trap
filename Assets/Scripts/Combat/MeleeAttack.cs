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

    private Vector3 lastHitDirection;

    private HashSet<Character> charactersHitThisAttack = new HashSet<Character>();

    [HideInInspector]
    public MeleeWeapon MeleeWeapon
    {
        get; private set;
    }

    public bool CharacterInAttackRange { get; private set; }

    public void Attack()
    {
        if ((Time.time - previousAttackTime) >= cooldown)
        {
            previousAttackTime = Time.time;
            StartCoroutine(AttackCoroutine());
        }
    }

    private void Awake()
    {
        MeleeWeapon = Instantiate(prefab);
        MeleeWeapon.transform.SetParent(meleeWeaponOrigin, false);
        MeleeWeapon.CollidedWithCharacter += OnWeaponCollidedWithCharacter;

        CharacterInAttackRange = false;
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
            CharacterMeleeBodyCollider characterBody = hits[i].transform.GetComponent<CharacterMeleeBodyCollider>();
            if (characterBody == null || characterBody.Character == this.character)
            {
                continue;
            }

            CharacterInAttackRange = true;
            break;
        }
    }

    private void OnWeaponCollidedWithCharacter(Character other, Vector3 impactPoint)
    {
        if (other == character || charactersHitThisAttack.Contains(other))
        {
            return;
        }

        charactersHitThisAttack.Add(other);

        other.Health.AddImpactPoint(impactPoint);

        Vector3 impactDirection = (other.transform.position - transform.position).normalized;//.normalized;
       
        float angle = MathExtensions.AngleAroundAxis(transform.forward, impactDirection, transform.up);
        float popAngle = MeleeWeapon.KnockbackPopAngleCurve.Evaluate(angle);
        float knockbackForce = MeleeWeapon.KnockbackForceCurve.Evaluate(angle);

        Debug.Log("angle: " + angle + ", pop: " + popAngle + ", force: " + knockbackForce);
        Vector3 knockbackDirection = Quaternion.AngleAxis(-popAngle, transform.right) * Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        other.Movement.Impulse(knockbackDirection * knockbackForce, 0.5f);

        lastHitDirection = knockbackDirection;
    }

    private IEnumerator AttackCoroutine()
    {
        charactersHitThisAttack.Clear();
        MeleeWeapon.CanCollide = true;
        character.Animator.MeleeAttack();

        float timer = 0.0f;
        while (timer < duration)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
        }

        MeleeWeapon.CanCollide = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + lastHitDirection);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
