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
        // Aktif yangın sayısı limitin üstündeyse spawn etme
        if (CurrentActiveFireCount() >= maxActiveFires)
            return;

        if (firePoints == null || firePoints.Length == 0)
            return;

        // Rastgele bir point seç, uygun değilse birkaç kez dene
        const int maxTries = 10;
        for (int i = 0; i < maxTries; i++)
        {
            FireSpawnPoint point = firePoints[Random.Range(0, firePoints.Length)];

            if (point == null) continue;
            if (point.HasActiveFire) continue;

            GameObject fire = point.SpawnRandomFire();
            if (fire != null)
            {
                // Başarılı spawn, çık
                return;
            }
        }
    }

    int CurrentActiveFireCount()
    {
        // Tüm yangın prefab’larına "Fire" tag’ini verirsen:
        return GameObject.FindGameObjectsWithTag("Fire").Length;
    }
}
