using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FireExtinguisherController : MonoBehaviour
{
    public ParticleSystem sprayParticles;
    public AudioSource spraySound;
    public Transform sprayPoint;

    public float sprayRange = 3f;
    public LayerMask fireLayer;

    private XRGrabInteractable grab;
    private bool isSpraying;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        // Trigger basılınca
        grab.activated.AddListener(OnActivate);

        // Trigger bırakılınca
        grab.deactivated.AddListener(OnDeactivate);
    }

    void OnDestroy()
    {
        grab.activated.RemoveListener(OnActivate);
        grab.deactivated.RemoveListener(OnDeactivate);
    }

    void Update()
    {
        if (!isSpraying) return;
        CheckFireHit();
    }

    void OnActivate(ActivateEventArgs args)
    {
        StartSpray();
    }

    void OnDeactivate(DeactivateEventArgs args)
    {
        StopSpray();
    }

    void StartSpray()
    {
        if (isSpraying) return;

        isSpraying = true;
        sprayParticles.Play();
        spraySound.Play();
    }

    void StopSpray()
    {
        if (!isSpraying) return;

        isSpraying = false;
        sprayParticles.Stop();
        spraySound.Stop();
    }

    void CheckFireHit()
    {
        Ray ray = new Ray(sprayPoint.position, sprayPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, sprayRange, fireLayer))
        {
            Fire fire = hit.collider.GetComponent<Fire>();
            if (fire != null)
            {
                fire.Extinguish(Time.deltaTime);
            }
        }
    }
}
