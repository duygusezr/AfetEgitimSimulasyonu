using System.Collections;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    public FireSpawnPoint[] firePoints;
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 15f;
    public int maxActiveFires = 4;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float wait = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(wait);

            TrySpawnFire();
        }
    }

    void TrySpawnFire()
    {
        // Aktif yangın sayısı limiti
        if (CurrentActiveFireCount() >= maxActiveFires)
            return;

        if (firePoints == null || firePoints.Length == 0)
            return;

        const int maxTries = 10;
        for (int i = 0; i < maxTries; i++)
        {
            FireSpawnPoint point = firePoints[Random.Range(0, firePoints.Length)];

            if (point == null) continue;
            if (point.HasActiveFire) continue;

            GameObject fireGO = point.SpawnRandomFire();
            if (fireGO == null) continue;

            // 🔥 NORMAL YANGIN
            Fire normalFire = fireGO.GetComponent<Fire>();
            if (normalFire != null)
            {
                normalFire.ActivateFire();   // 🔑 ceza artık buradan başlar
                return;
            }

            // 🔌 ELEKTRİK YANGINI
            ElectricFire electricFire = fireGO.GetComponent<ElectricFire>();
            if (electricFire != null)
            {
                electricFire.ActivateFire(); // 🔑 ceza artık buradan başlar
                return;
            }

            // Eğer ikisi de yoksa (hatalı prefab)
            Debug.LogWarning(
                $"Spawn edilen objede Fire veya ElectricFire yok: {fireGO.name}"
            );
            return;
        }
    }

    int CurrentActiveFireCount()
    {
        return GameObject.FindGameObjectsWithTag("Fire").Length;
    }
}
