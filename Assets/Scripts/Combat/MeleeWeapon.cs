using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField]
    private LayerMask characterBodyLayer;

    public event System.Action<Character, Vector3> CollidedWithCharacter;

    public bool CanCollide = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!CanCollide)
        {
            return;
        }

        CharacterMeleeBodyCollider characterBody = other.GetComponent<CharacterMeleeBodyCollider>();
        if (characterBody == null || CollidedWithCharacter == null)
        {
            return;
        }

        CollidedWithCharacter(characterBody.Character, Vector3.zero);
    }
}
