using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SimpleBagCollector : MonoBehaviour
{
    [Header("Eþyanýn görsel olarak gireceði nokta")]
    public Transform hidePoint;
    public float disappearDelay = 0.15f;

    private TutorialManager tutorialManager;

    private void Start()
    {
        // GÜNCELLENEN KISIM BURASI:
        // FindObjectOfType yerine FindFirstObjectByType kullandýk.
        tutorialManager = FindFirstObjectByType<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();

        if (grab == null)
            return;

        if (!other.gameObject.activeInHierarchy) return;

        Debug.Log("Çanta ile temas eden eþya: " + other.name);

        StartCoroutine(CollectRoutine(other, grab));
    }

    private IEnumerator CollectRoutine(Collider other, XRGrabInteractable grab)
    {
        GameObject item = other.gameObject;

        var interactor = grab.firstInteractorSelecting;
        if (interactor != null && grab.interactionManager != null)
        {
            grab.interactionManager.SelectExit(interactor, grab);
        }

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (hidePoint != null)
        {
            item.transform.position = hidePoint.position;
            item.transform.rotation = hidePoint.rotation;
        }
        else
        {
            item.transform.position = transform.position;
        }

        yield return new WaitForSeconds(disappearDelay);

        if (tutorialManager != null)
        {
            tutorialManager.GoreviIlerlet(GorevTipi.CantaHazirlama);
            Debug.Log("TutorialManager'a haber verildi: +1 Eþya");
        }

        item.SetActive(false);
    }
}