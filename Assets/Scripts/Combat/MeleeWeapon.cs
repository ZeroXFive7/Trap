using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Tooltip("X axis is vertical impact distance from top of character collider.  Y axis is knockback angle from XZ plane.")]
    [SerializeField]
    private AnimationCurve verticalKnockbackAngleCurve;
    [Tooltip("X axis is horizontal impact distance from center of character collider.  Y axis is knockback angle from YZ plane.")]
    [SerializeField]
    private AnimationCurve horizontalKnockbackAngleCurve;
    [SerializeField]
    private MeleeWeaponCollider[] colliders = null;

    public event System.Action<Character, Vector3> CollidedWithCharacter;

    public bool CanCollide
    {
        set
        {
            for (int i = 0; i < colliders.Length; ++i)
            {
                colliders[i].enabled = value;
            }
        }
    }

    private void Awake()
    {
        CanCollide = false;
    }

    private void Update()
    {
        if (CollidedWithCharacter == null)
        {
            return;
        }

        for (int i = 0; i < colliders.Length; ++i)
        {
            RaycastHit[] collisions = colliders[i].Collisions;
            if (collisions == null)
            {
                continue;
            }

            for (int j = 0; j < collisions.Length; ++j)
            {
                CharacterMeleeBodyCollider characterBody = collisions[j].transform.GetComponent<CharacterMeleeBodyCollider>();
                if (characterBody == null)
                {
                    continue;
                }

                CollidedWithCharacter(characterBody.Character, collisions[j].point);
            }
        }
    }
}
