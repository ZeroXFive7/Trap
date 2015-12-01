using UnityEngine;
using System.Collections.Generic;

public class MeleeWeapon : MonoBehaviour
{
    [Tooltip("X axis is angle from player forward vector.  Y axis is knockback angle from XZ plane.")]
    [SerializeField]
    private AnimationCurve knockbackPopAngleCurve;
    [Tooltip("X axis is angle of impact vector from player forward vector.  Y axis is force of knockback.")]
    [SerializeField]
    private AnimationCurve knockbackForceCurve;
    [SerializeField]
    private SpherecastCollider[] colliders = null;

    public event System.Action<Shield, Vector3> CollidedWithShield;

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

    public bool CollidersEnabled
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
        CollidersEnabled = false;

        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].Collision += OnCollision;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].Collision -= OnCollision;
        }
    }

    private void OnCollision(Transform collider, Vector3 collisionPoint)
    {
        Shield shield = collider.GetComponent<Shield>();
        if (shield != null && CollidedWithShield != null)
        {
            CollidedWithShield(shield, collisionPoint);
        }
    }
}
