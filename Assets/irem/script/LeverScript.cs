using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverPullDown : MonoBehaviour
{
    public Transform leverPivot;
    public float minAngle = 0f;     // yukarı
    public float maxAngle = 90f;    // aşağı
    public float pullSpeed = 5f;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    private bool isGrabbed;

    void Update()
    {
        if (!isGrabbed || interactor == null) return;

        Vector3 handPos = interactor.transform.position;
        Vector3 localHandPos = leverPivot.InverseTransformPoint(handPos);

        float targetAngle = Mathf.Clamp(
            Mathf.Abs(localHandPos.y) * 60f,
            minAngle,
            maxAngle
        );

        Vector3 currentEuler = leverPivot.localEulerAngles;
        currentEuler.x = Mathf.LerpAngle(
            currentEuler.x,
            targetAngle,
            Time.deltaTime * pullSpeed
        );

        leverPivot.localEulerAngles = currentEuler;
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        isGrabbed = true;
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;
        interactor = null;
    }
}
