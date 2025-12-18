using UnityEngine;

public class NormalFireSpawner : MonoBehaviour
{
    public Fire firePrefab;
    public Transform spawnPoint;

    public void SpawnFire()
    {
        Fire fire = Instantiate(
            firePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 🔑 ceza bundan sonra başlar
        fire.ActivateFire();
    }
}
