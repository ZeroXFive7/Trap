using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHitPoints = 10.0f;
    [SerializeField]
    private float initialHitPoints = 10.0f;
    [SerializeField]
    private LayerMask safeVolumeLayers;

    [Header("Component References")]
    [SerializeField]
    Character character = null;

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

        character.Movement.SnapToPosition(spawnPoint.position);
    }

    public void Die()
    {
        HitPoints = 0.0f;
        IsDead = true;

        character.Shield.ClearImpacts();
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
