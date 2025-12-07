using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SoketTagFiltresi : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
{
    [Header("Ýzin Verilen Etiket")]
    public string hedefTag = "SabitlemeParcasi";

    public bool canProcess => isActiveAndEnabled;

    // --- HOVER FÝLTRESÝ (Mýknatýs Etkisi) ---
    public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
    {
        // Etiket doðruysa TRUE (izin ver), yanlýþsa FALSE (reddet)
        return interactable != null && interactable.transform.CompareTag(hedefTag);
    }

    // --- SELECT FÝLTRESÝ (Yerleþme/Snap Etkisi) ---
    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        // Etiket doðruysa TRUE (izin ver), yanlýþsa FALSE (reddet)
        return interactable != null && interactable.transform.CompareTag(hedefTag);
    }
}