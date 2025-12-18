using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VR_ElectricLever : MonoBehaviour
{
    public float offAngle = -60f;

    [Header("Optional Fire Logic")]
    public bool useFireLock = false; // 🔥 sadece elektrik yangını olan sahnede aç
    
    XRGrabInteractable grab;
    bool isOff = false;
    Vector3 initialRotation;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);

        initialRotation = transform.localEulerAngles;
    }

    void OnDestroy()
    {
        if (grab != null)
            grab.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (isOff) return;

        isOff = true;

        // Kolu indir
        transform.Rotate(offAngle, 0f, 0f, Space.Self);

        // Normal sahnede tek kullanımlık
        if (!useFireLock)
            grab.enabled = false;

        Debug.Log("Elektrik kesildi");
    }

    // 🔁 Elektrik yangını söndüğünde çağrılır
    public void ResetLever()
    {
        if (!useFireLock) return;

        transform.localEulerAngles = initialRotation;
        isOff = false;
        grab.enabled = true;
    }

    public bool IsPowerOff()
    {
        float x = transform.localEulerAngles.x;
        if (x > 180f) x -= 360f;
        return Mathf.Abs(x - offAngle) < 5f;
    }
}
