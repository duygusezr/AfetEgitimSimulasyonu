using UnityEngine;

public class ElectricFire : MonoBehaviour
{
    [Header("Fire Settings")]
    public float health = 3f;

    [Header("Auto Find Lever")]
    public string mainKnobName = "Main Knob"; // 👈 SAHNEDEKİ OBJENİN ADI
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

    bool isActiveFire = false;

    bool IsAlive => health > 0;

    // 🔥 Spawner çağırır
    public void ActivateFire()
    {
        isActiveFire = true;
        lastAlivePenaltyTime = Time.time;
    }

    void Awake()
    {
        // 🔌 SAHNEDE "Main Knob" ALTINDAN VR_ElectricLever BUL
        GameObject knob = GameObject.Find(mainKnobName);
        if (knob != null)
        {
            electricLever = knob.GetComponentInChildren<VR_ElectricLever>();
        }

        if (electricLever == null)
        {
            Debug.LogError(
                $"[ElectricFire] '{mainKnobName}' altında VR_ElectricLever bulunamadı!"
            );
        }
    }

    void Update()
    {
        if (!isActiveFire || !IsAlive) return;

        // 🔥 Yangın durdukça ceza
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

        // ❌ CO₂ ZORUNLU
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
