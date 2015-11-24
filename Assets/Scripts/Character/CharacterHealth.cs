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

    [Header("Component References")]
    [SerializeField]
    CharacterMovement steering = null;

    public float HitPoints { get; private set; }

    public bool IsDead { get; private set; }

    public float FractionalHitPoints
    {
        get
        {
            return HitPoints / maxHitPoints;
        }
    }

    public void Spawn(Transform spawnPoint)
    {
        IsDead = false;
        HitPoints = initialHitPoints;

        steering.SnapToPosition(spawnPoint.position);
    }

    public void Die()
    {
        HitPoints = 0.0f;
        IsDead = true;
    }

    public void Respawn()
    {
        Die();

        Transform spawnPoint = GameplayManager.Instance.CurrentLevel.GetRandomSpawnPoint();
        Spawn(spawnPoint);
    }

    private void Start()
    {
        IsDead = false;
        HitPoints = initialHitPoints;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.LayerIsInLayerMask(safeVolumeLayers))
        {
            Respawn();
        }
    }
}
