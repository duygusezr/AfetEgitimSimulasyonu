using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("Extinguisher Type")]
    public ExtinguisherType extinguisherType; // Inspector’dan seçilir

    [Header("Spray")]
    public ParticleSystem sprayParticles;
    public AudioSource spraySound;
    public Transform sprayPoint;
    public float sprayRange = 3f;

    [Header("Fire Detection")]
    public LayerMask fireLayer;

    XRGrabInteractable grab;
    bool isSpraying;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        grab.activated.AddListener(OnActivate);
        grab.deactivated.AddListener(OnDeactivate);
        grab.selectExited.AddListener(OnSelectExit);
    }

    void OnDestroy()
    {
        grab.activated.RemoveListener(OnActivate);
        grab.deactivated.RemoveListener(OnDeactivate);
        grab.selectExited.RemoveListener(OnSelectExit);
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

    void OnSelectExit(SelectExitEventArgs args)
    {
        ForceStopSpray();
    }

    void ForceStopSpray()
    {
        if (!isSpraying) return;

        isSpraying = false;
        sprayParticles.Stop();
        spraySound.Stop();
    }

    void CheckFireHit()
    {
        Ray ray = new Ray(sprayPoint.position, sprayPoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * sprayRange, Color.red);

        if (!Physics.Raycast(ray, out RaycastHit hit, sprayRange, fireLayer))
            return;

        // 🔌 Elektrik yangını
        ElectricFire electricFire = hit.collider.GetComponentInParent<ElectricFire>();
        if (electricFire != null)
        {
            electricFire.TryExtinguish(Time.deltaTime, extinguisherType);
            return;
        }

        // 🔥 Normal yangın
        Fire fire = hit.collider.GetComponentInParent<Fire>();
        if (fire != null)
        {
            fire.TryExtinguish(Time.deltaTime, extinguisherType);
            return;
        }
    }


}
