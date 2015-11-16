using UnityEngine;
using System.Collections;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHitPoints = 10.0f;
    [SerializeField]
    private float initialHitPoints = 10.0f;
    [SerializeField]
    private LayerMask safeVolumeLayers;

    public float HitPoints { get; private set; }

    public float FractionalHitPoints
    {
        get
        {
            return HitPoints / maxHitPoints;
        }
    }

    public void Spawn()
    {
        HitPoints = initialHitPoints;
    }

    public void Die()
    {
        HitPoints = 0.0f;
    }

    private void Start()
    {
        Spawn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.LayerIsInLayerMask(safeVolumeLayers))
        {
            Die();
        }
    }
}
