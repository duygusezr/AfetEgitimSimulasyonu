using UnityEngine;

public class SandbagSlot : MonoBehaviour
{
    public Transform snapPoint; // torbanýn duracaðý tam nokta

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Sandbag")) return;

        Sandbag bag = other.GetComponent<Sandbag>();
        if (bag == null) return;

        bag.SnapToSlot(snapPoint);
    }
}
