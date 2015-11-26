using UnityEngine;
using System.Collections.Generic;

public class RangedAttack : MonoBehaviour
{
    [SerializeField]
    private RangedWeapon prefab = null;
    [SerializeField]
    private Transform rangedWeaponOrigin = null;

    [Header("Component References")]
    [SerializeField]
    private Character character = null;

    HashSet<Character> charactersHitThisAttack = new HashSet<Character>();

    public RangedWeapon RangedWeapon
    {
        get; private set;
    }

    public void Fire()
    {
        charactersHitThisAttack.Clear();

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
        for (int i = 0; i < hits.Length; ++i)
        {
            CharacterBodyCollider colliderBody = hits[i].transform.GetComponent<CharacterBodyCollider>();
            if (colliderBody == null)
            {
                continue;
            }

            Character colliderCharacter = colliderBody.Character;
            if (colliderCharacter != this.character && !charactersHitThisAttack.Contains(colliderCharacter))
            {
                charactersHitThisAttack.Add(colliderCharacter);
                colliderCharacter.Movement.Impulse(transform.forward * RangedWeapon.KnockbackForce, 0.1f);
            }
        }
    }

    private void Awake()
    {
        RangedWeapon = Instantiate(prefab);
        RangedWeapon.transform.SetParent(rangedWeaponOrigin, false);

        enabled = false;
    }

    private void OnEnable()
    {
        rangedWeaponOrigin.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        rangedWeaponOrigin.gameObject.SetActive(false);
    }
}
