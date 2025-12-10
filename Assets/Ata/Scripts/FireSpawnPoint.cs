using UnityEngine;

public class FireSpawnPoint : MonoBehaviour
{
    [Tooltip("Bu noktada çıkabilecek yangın prefabları (A, B, C, F vs.)")]
    public GameObject[] possibleFires;  // Örn: sadece Elektrik + Katı

    [Tooltip("Yangının spawn olacağı nokta, boş bırakılırsa bu obje kullanılır.")]
    public Transform spawnTransform;

    public bool HasActiveFire
    {
        get
        {
            Transform t = spawnTransform != null ? spawnTransform : transform;
            return t.childCount > 0;
        }
    }

    public GameObject SpawnRandomFire()
    {
        if (possibleFires == null || possibleFires.Length == 0)
        {
            Debug.LogWarning($"FireSpawnPoint '{name}' için possibleFires boş.");
            return null;
        }

        if (HasActiveFire)
        {
            // Bu noktada zaten yangın var, yenisini çıkarma
            return null;
        }

        int index = Random.Range(0, possibleFires.Length);
        GameObject prefab = possibleFires[index];

        Transform t = spawnTransform != null ? spawnTransform : transform;
        GameObject fireInstance = Instantiate(prefab, t.position, t.rotation, t);
        return fireInstance;
    }
}
