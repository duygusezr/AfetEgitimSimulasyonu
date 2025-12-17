using UnityEngine;

public class TaskTimer : MonoBehaviour
{
    public float timeLimit = 60f;
    private float timer;

    public WaterRiseController water;
    private bool tasksCompleted = false;

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        if (tasksCompleted) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            water.waterActive = true;
        }
    }

    public void CompleteAllTasks()
    {
        tasksCompleted = true;
        water.waterActive = false;
    }
}
