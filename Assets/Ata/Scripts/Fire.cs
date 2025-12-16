using UnityEngine;

public enum FireType { A, B, C, F, Gas, D }

public class Fire : MonoBehaviour
{
    public FireType fireType;
    public float health = 3f;

    public void Extinguish(float amount)
    {
        health -= amount;

        if (health <= 0)
            Destroy(gameObject);
    }
}
