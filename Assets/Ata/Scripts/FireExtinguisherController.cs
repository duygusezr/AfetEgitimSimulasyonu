using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FireExtinguisherController : MonoBehaviour
{
    public ExtinguisherType extinguisherType;

    public ParticleSystem sprayParticles;
    public AudioSource spraySound;
    public Transform sprayPoint;

    public float sprayRange = 3f;
    public LayerMask fireLayer;

    [Header("Score Settings")]
    private int correctScoreAmount = +5;
    private int wrongScoreAmount = -2;

    private float correctScoreCooldown = 0.7f;
    private float wrongScoreCooldown = 0.7f;

    float lastCorrectScoreTime;
    float lastWrongScoreTime;

    XRGrabInteractable grab;
    bool isSpraying;

    void Awake()
{
    grab = GetComponent<XRGrabInteractable>();

    // Trigger (arka tuş)
    grab.activated.AddListener(OnActivate);
    grab.deactivated.AddListener(OnDeactivate);

    // Grip (yan tuş) bırakılınca
    grab.selectExited.AddListener(OnSelectExit);
}


    void OnDestroy()
    {
        grab.activated.RemoveListener(OnActivate);
        grab.deactivated.RemoveListener(OnDeactivate);
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
        // Yandaki tuş bırakıldıysa sprey ZORLA dursun
        ForceStopSpray();
    }

    void ForceStopSpray()
    {
        if (!isSpraying) return;

        isSpraying = false;
        sprayParticles.Stop();
        spraySound.Stop();
    }

    void Update()
    {
        if (isSpraying)
            CheckFireHit();
    }

    void OnActivate(ActivateEventArgs args)
    {
        isSpraying = true;
        sprayParticles.Play();
        spraySound.Play();
    }

    void OnDeactivate(DeactivateEventArgs args)
    {
        ForceStopSpray();
    }


    void CheckFireHit()
    {
        Ray ray = new Ray(sprayPoint.position, sprayPoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * sprayRange, Color.red);

        if (!Physics.Raycast(ray, out RaycastHit hit, sprayRange, fireLayer))
            return;

        Fire fire = hit.collider.GetComponent<Fire>();
        if (fire == null || !fire.IsAlive)
            return;

        if (fire.CanBeExtinguishedBy(extinguisherType))
        {
            if (Time.time - lastCorrectScoreTime > correctScoreCooldown)
            {
                ScoreManager.Instance.AddScore(correctScoreAmount);
                lastCorrectScoreTime = Time.time;
            }

            fire.Extinguish(Time.deltaTime);
        }
        else
        {
            if (Time.time - lastWrongScoreTime > wrongScoreCooldown)
            {
                ScoreManager.Instance.AddScore(wrongScoreAmount);
                lastWrongScoreTime = Time.time;
            }
        }
    }
}
