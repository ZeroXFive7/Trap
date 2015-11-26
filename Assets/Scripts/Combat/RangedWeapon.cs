using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    [SerializeField]
    private float knockbackForce = 3.0f;

    public float KnockbackForce
    {
        get { return knockbackForce; }
    }
}
