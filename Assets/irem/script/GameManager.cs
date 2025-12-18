using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Task Settings")]
    public int totalTaskCount = 3;
    private int completedTaskCount = 0;

    [Header("UI")]
    public TextMeshProUGUI taskProgressText;

    [Header("References")]
    public TaskTimer taskTimer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateTaskUI();
    }

    public void CompleteTask()
    {
        completedTaskCount++;

        Debug.Log("Görev tamamlandý: " + completedTaskCount + "/" + totalTaskCount);

        UpdateTaskUI();

        if (completedTaskCount >= totalTaskCount)
        {
            taskTimer.CompleteAllTasks();
        }
    }

    void UpdateTaskUI()
    {
        if (taskProgressText != null)
        {
            taskProgressText.text =
                $"Görev {completedTaskCount} / {totalTaskCount} tamamlandý";
        }
    }
}
