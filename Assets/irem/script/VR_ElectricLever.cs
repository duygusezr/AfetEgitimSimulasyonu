using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VR_ElectricLever : MonoBehaviour
{
    public float offAngle = -60f;

    private XRGrabInteractable grab;
    private bool isOff = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        if (grab != null)
            grab.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (isOff) return;

        isOff = true;

        // 🔴 EN BASİT, EN GARANTİ
        transform.Rotate(offAngle, 0f, 0f, Space.Self);

        grab.enabled = false;

        GameManager.Instance.CompleteTask();

        Debug.Log("Elektrik kesildi");
    }
}
