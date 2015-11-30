using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    [SerializeField]
    private float knockbackForce = 3.0f;
    [SerializeField]
    private float recoilDuration = 0.2f;
    [SerializeField]
    private Vector2 recoilPitchYaw;

    public float KnockbackForce
    {
        get { return knockbackForce; }
    }

    public float RecoilDuration
    {
        get { return recoilDuration; }
    }

    public Vector2 RecoilPitchYaw
    {
        get { return recoilPitchYaw; }
    }
}
