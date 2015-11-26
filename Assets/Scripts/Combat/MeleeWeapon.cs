using UnityEngine;
using System.Collections.Generic;

public class MeleeWeapon : MonoBehaviour
{
    private class CharacterCollision
    {
        public Character Character;
        public List<Vector3> CollisionPoints = new List<Vector3>();

        public Vector3 CollisionCentroid
        {
            get
            {
                Vector3 centroid = Vector3.zero;
                for (int i =0; i < CollisionPoints.Count; ++i)
                {
                    centroid += CollisionPoints[i];
                }
                return centroid / (float)CollisionPoints.Count;
            }
        }
    };

    [Tooltip("X axis is angle from player forward vector.  Y axis is knockback angle from XZ plane.")]
    [SerializeField]
    private AnimationCurve knockbackPopAngleCurve;
    [Tooltip("X axis is angle of impact vector from player forward vector.  Y axis is force of knockback.")]
    [SerializeField]
    private AnimationCurve knockbackForceCurve;
    [SerializeField]
    private SpherecastCollider[] colliders = null;

    public AnimationCurve KnockbackPopAngleCurve
    {
        get
        {
            return knockbackPopAngleCurve;
        }
    }

    public AnimationCurve KnockbackForceCurve
    {
        get
        {
            return knockbackForceCurve;
        }
    }

    public event System.Action<Character, Vector3> CollidedWithCharacter;

    private List<CharacterCollision> characterCollisions = new List<CharacterCollision>();

    public bool CanCollide
    {
        set
        {
            for (int i = 0; i < colliders.Length; ++i)
            {
                colliders[i].enabled = value;
            }
            enabled = value;
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
            // Someone must be listening to character collision events.
            return;
        }

        characterCollisions.Clear();

        for (int colliderIndex = 0; colliderIndex < colliders.Length; ++colliderIndex)
        {
            RaycastHit[] collisions = colliders[colliderIndex].Collisions;
            if (collisions == null)
            {
                continue;
            }

            for (int collisionIndex = 0; collisionIndex < collisions.Length; ++collisionIndex)
            {
                CharacterBodyCollider collidedCharacterBody = collisions[collisionIndex].transform.GetComponent<CharacterBodyCollider>();
                if (collidedCharacterBody == null)
                {
                    // Can only hit characters.
                    continue;
                }

                int existingCollisionIndex = characterCollisions.FindIndex(c => c.Character == collidedCharacterBody.Character);
                if (existingCollisionIndex < 0)
                {
                    CharacterCollision newCharacterCollision = new CharacterCollision();
                    newCharacterCollision.Character = collidedCharacterBody.Character;
                    newCharacterCollision.CollisionPoints.Add(collisions[collisionIndex].point);
                    characterCollisions.Add(newCharacterCollision);
                }
                else
                {
                    characterCollisions[existingCollisionIndex].CollisionPoints.Add(collisions[collisionIndex].point);
                }
            }
        }

        for (int characterCollisionIndex = 0; characterCollisionIndex < characterCollisions.Count; ++characterCollisionIndex)
        {
            CollidedWithCharacter(characterCollisions[characterCollisionIndex].Character, characterCollisions[characterCollisionIndex].CollisionCentroid);
        }
    }
}
