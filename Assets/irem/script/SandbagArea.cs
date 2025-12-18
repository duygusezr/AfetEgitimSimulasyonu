using UnityEngine;

public class SandbagArea : MonoBehaviour
{
    public int totalSlotCount;
    private int filledSlotCount = 0;

    private bool taskCompleted = false;

    public void OnSlotFilled()
    {
        if (taskCompleted) return;

        filledSlotCount++;
        Debug.Log("Slot doldu: " + filledSlotCount + "/" + totalSlotCount);

        if (filledSlotCount >= totalSlotCount)
        {
            taskCompleted = true;
            Debug.Log("TÜM SLOTLAR DOLDU – GÖREV TAMAMLANDI");

            GameManager.Instance.CompleteTask();
        }
    }
}
