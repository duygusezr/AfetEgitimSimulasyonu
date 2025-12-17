using UnityEngine;

public enum FireType { A, B, C, F, Gas, D }

public class Fire : MonoBehaviour
{
    public FireType fireType;
    public float health = 3f;

    [Header("Score Penalty While Fire Is Alive")]
    public float alivePenaltyInterval = 1.5f;
    public int alivePenaltyAmount = -1;

    float lastAlivePenaltyTime;

    public bool IsAlive => health > 0;

    void Update()
    {
        if (!IsAlive) return;

        // 🔥 Yangın oyunda durduğu sürece ceza
        if (Time.time - lastAlivePenaltyTime > alivePenaltyInterval)
        {
            ScoreManager.Instance.AddScore(alivePenaltyAmount);
            lastAlivePenaltyTime = Time.time;
        }
    }

    public bool CanBeExtinguishedBy(ExtinguisherType extinguisher)
    {
        switch (fireType)
        {
            case FireType.A:
                return extinguisher == ExtinguisherType.Water ||
                       extinguisher == ExtinguisherType.Foam ||
                       extinguisher == ExtinguisherType.DryChemical;

            case FireType.B:
                return extinguisher == ExtinguisherType.Foam ||
                       extinguisher == ExtinguisherType.DryChemical;

            case FireType.C:
                return extinguisher == ExtinguisherType.CO2 ||
                       extinguisher == ExtinguisherType.DryChemical;

            case FireType.F:
                return extinguisher == ExtinguisherType.DryChemical;

            case FireType.D:
                return extinguisher == ExtinguisherType.MetalPowder;

            case FireType.Gas:
                return extinguisher == ExtinguisherType.DryChemical;

            default:
                return false;
        }
    }

    public void Extinguish(float amount)
    {
        health -= amount;

        if (health <= 0)
            Destroy(gameObject);
    }
}
