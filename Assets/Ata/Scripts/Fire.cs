using UnityEngine;

public enum FireType { A, B, F }

public class Fire : MonoBehaviour
{
    public FireType fireType;
    public float health = 3f;

    [Header("Alive Penalty")]
    public float alivePenaltyInterval = 1.5f;
    public int alivePenaltyAmount = -1;

    [Header("Extinguish Reward")]
    public int extinguishReward = +10;

    [Header("Wrong Extinguisher Penalty")]
    public int wrongExtinguisherPenalty = -2;
    public float wrongPenaltyCooldown = 0.8f;

    float lastPenaltyTime;
    float lastWrongPenaltyTime;
    bool isActiveFire = false;

    public bool IsAlive => health > 0;

    // 🔑 Spawner çağırmak zorunda
    public void ActivateFire()
    {
        isActiveFire = true;
        lastPenaltyTime = Time.time;
    }

    void Update()
    {
        if (!isActiveFire || !IsAlive) return;

        // 🔥 Yangın durdukça ceza
        if (Time.time - lastPenaltyTime > alivePenaltyInterval)
        {
            ScoreManager.Instance.AddScore(alivePenaltyAmount);
            lastPenaltyTime = Time.time;
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

            case FireType.F:
                return extinguisher == ExtinguisherType.DryChemical;

            default:
                return false;
        }
    }

    public void TryExtinguish(float amount, ExtinguisherType extinguisherType)
    {
        if (!isActiveFire) return;

        // ❌ Yanlış tüp
        if (!CanBeExtinguishedBy(extinguisherType))
        {
            ApplyWrongPenalty();
            return;
        }

        // ✅ Doğru tüp
        health -= amount;

        if (health <= 0)
        {
            ScoreManager.Instance.AddScore(extinguishReward);
            Destroy(gameObject);
        }
    }

    void ApplyWrongPenalty()
    {
        // ❗ Spam olmasın
        if (Time.time - lastWrongPenaltyTime > wrongPenaltyCooldown)
        {
            ScoreManager.Instance.AddScore(wrongExtinguisherPenalty);
            lastWrongPenaltyTime = Time.time;
        }
    }
}
