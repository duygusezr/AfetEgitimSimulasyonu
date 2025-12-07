using System.Collections;
using System.Collections.Generic; // YENÝ: Listeler için gerekli
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SimpleBagCollector : MonoBehaviour
{
    [Header("Eþyanýn görsel olarak gireceði nokta")]
    public Transform hidePoint;
    public float disappearDelay = 0.15f;

    [Header("Filtre Ayarlarý")]
    public string kabulEdilenTag = "CantayaGirebilir";

    private TutorialManager tutorialManager;

    // YENÝ: Þu an iþlenen eþyalarýn listesi (Çift saymayý önler)
    private HashSet<GameObject> islenenEsyalar = new HashSet<GameObject>();

    private void Start()
    {
        tutorialManager = FindFirstObjectByType<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Etiket Kontrolü
        if (!other.CompareTag(kabulEdilenTag)) return;

        // YENÝ: 2. Çift Sayma Kontrolü
        // Eðer bu eþya zaten iþleme alýndýysa, ikinci kez sayma!
        if (islenenEsyalar.Contains(other.gameObject)) return;

        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
        if (grab == null) return;
        if (!other.gameObject.activeInHierarchy) return;

        // Eþyayý "Ýþlenenler" listesine ekle
        islenenEsyalar.Add(other.gameObject);

        Debug.Log("Doðru eþya alýndý: " + other.name);
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
        }

        item.SetActive(false);

    }
}