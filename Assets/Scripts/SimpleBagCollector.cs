using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SimpleBagCollector : MonoBehaviour
{
    [Header("Eþyanýn görsel olarak gireceði nokta")]
    public Transform hidePoint;
    public float disappearDelay = 0.15f;

    // --- YENÝ EKLENDÝ: Etiket Kontrolü ---
    [Header("Filtre Ayarlarý")]
    public string kabulEdilenTag = "CantayaGirebilir";
    // Unity'de eþyalara bu Tag'i vermeyi unutmayýn!

    private TutorialManager tutorialManager;

    private void Start()
    {
        // FindFirstObjectByType kullanýmý (Güncel ve doðru olan)
        tutorialManager = FindFirstObjectByType<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // --- 1. KONTROL: ETÝKET (TAG) DOÐRU MU? ---
        if (!other.CompareTag(kabulEdilenTag))
        {
            // Eðer objenin etiketi "CantayaGirebilir" deðilse,
            // iþlemi hemen durdur. (Yastýk, kitap vb. çantaya girmez)
            return;
        }
        // ------------------------------------------

        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();

        if (grab == null)
            return;

        if (!other.gameObject.activeInHierarchy) return;

        Debug.Log("Doðru eþya bulundu ve alýnýyor: " + other.name);

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