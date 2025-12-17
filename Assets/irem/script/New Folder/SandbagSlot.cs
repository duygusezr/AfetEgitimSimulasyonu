using UnityEngine;

public class SandbagSlot : MonoBehaviour
{
    public Transform snapPoint; // torbanýn duracaðý tam nokta
    public SandbagArea area;

    private bool slotFilled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (slotFilled) return;
        if (!other.CompareTag("Sandbag")) return;

        Sandbag bag = other.GetComponent<Sandbag>();
        if (bag == null) return;

        bag.SnapToSlot(snapPoint);

        slotFilled = true;
        area.OnSlotFilled();
    }
}
