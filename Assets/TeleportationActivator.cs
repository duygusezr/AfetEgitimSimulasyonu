using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportationActivator : MonoBehaviour
{
    public XRRayInteractor teleportInteractor;
    public InputActionProperty teleportActivatorAction;

    void Start()
    {
        // Baþta ýþýnlayýcý devre dýþý
        teleportInteractor.gameObject.SetActive(false);

        // Tuþ basýldýðýnda aktif hale getir
        teleportActivatorAction.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        teleportInteractor.gameObject.SetActive(true);
    }

    // Her frame çaðrýlýr
    void Update()
    {
        // Tuþ býrakýldýðýnda ýþýnlayýcýyý kapat
        if (teleportActivatorAction.action.WasReleasedThisFrame())
        {
            teleportInteractor.gameObject.SetActive(false);
        }
    }
}
