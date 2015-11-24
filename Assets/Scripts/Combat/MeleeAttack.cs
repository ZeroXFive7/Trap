using UnityEngine;
using System.Collections;

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
            previousAttackTime = Time.time;
            StartCoroutine(AttackCoroutine());
        }
    }

    private void Awake()
    {
        MeleeWeapon = Instantiate(prefab);
        MeleeWeapon.transform.SetParent(meleeWeaponOrigin, false);
        MeleeWeapon.CollidedWithCharacter += OnWeaponCollidedWithCharacter;
    }

    private void OnWeaponCollidedWithCharacter(Character other, Vector3 position)
    {
        if (other == character)
        {
            return;
        }

        Vector3 otherDirection = (Vector3.up + other.transform.position - transform.position).normalized;
        other.Movement.Impulse(otherDirection * impulseMagnitude);
    }

    private IEnumerator AttackCoroutine()
    {
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
}
