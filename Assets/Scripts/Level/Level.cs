using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints = null;

    public Transform GetClosestSpawnPoint(Vector3 position)
    {
        Transform closestSpawnPoint = null;
        float sqrMaxDistance = float.MaxValue;
        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            float sqrDistance = (spawnPoints[i].position - position).sqrMagnitude;
            if (sqrDistance < sqrMaxDistance)
            {
                closestSpawnPoint = spawnPoints[i];
                sqrMaxDistance = sqrDistance;
            }
        }
        return closestSpawnPoint;
    }
}
