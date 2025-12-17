using UnityEngine;

public class Sandbag : MonoBehaviour
{
    public bool isCarried;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform carryPoint)
    {
        isCarried = true;
        rb.isKinematic = true;
        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void SnapToSlot(Transform slotPoint)
    {
        Debug.Log("SNAP CALLED");

        isCarried = false;

        rb.isKinematic = true;

        transform.SetParent(slotPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isCarried = false;
        rb.isKinematic = false;
        transform.SetParent(null);
    }
}
