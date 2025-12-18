using UnityEngine;

public class ElectricFire : MonoBehaviour
{
    [Header("Fire Settings")]
    public float health = 3f;
    public VR_ElectricLever electricLever;

    [Header("Score While Alive")]
    public float alivePenaltyInterval = 1.5f;
    public int alivePenaltyAmount = -1;

    [Header("Wrong Action Penalty")]
    public int wrongActionPenalty = -2;
    public float wrongActionCooldown = 0.8f;

    [Header("Extinguish Reward")]
    public int extinguishReward = +5;

    float lastAlivePenaltyTime;
    float lastWrongActionTime;

    bool isActiveFire = false; // 🔑 KRİTİK

    bool IsAlive => health > 0;

    // 🔥 Spawner yangını oluşturduğunda çağır
    public void ActivateFire()
    {
        isActiveFire = true;
        lastAlivePenaltyTime = Time.time;
    }

    void Update()
    {
        if (!isActiveFire || !IsAlive) return;

        // 🔥 Yangın oyunda durdukça ceza
        if (Time.time - lastAlivePenaltyTime > alivePenaltyInterval)
        {
            ScoreManager.Instance.AddScore(alivePenaltyAmount);
            lastAlivePenaltyTime = Time.time;
        }
    }

    // 🧯 Söndürücüden çağrılır
    public void TryExtinguish(float amount, ExtinguisherType extinguisherType)
    {
        if (!isActiveFire) return;

        // ❌ CO2 ZORUNLU
        if (extinguisherType != ExtinguisherType.CO2)
        {
            ApplyWrongActionPenalty();
            return;
        }

        // ❌ Elektrik kesilmemiş
        if (electricLever == null || !electricLever.IsPowerOff())
        {
            ApplyWrongActionPenalty();
            return;
        }

        // ✅ Doğru koşullar
        health -= amount;

        if (health <= 0)
        {
            ScoreManager.Instance.AddScore(extinguishReward);
            electricLever.ResetLever();
            Destroy(gameObject);
        }
    }

    void ApplyWrongActionPenalty()
    {
        if (Time.time - lastWrongActionTime > wrongActionCooldown)
        {
            ScoreManager.Instance.AddScore(wrongActionPenalty);
            lastWrongActionTime = Time.time;
        }
    }
}
