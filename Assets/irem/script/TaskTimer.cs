using UnityEngine;
using TMPro;
using System.Collections;

public class TaskTimer : MonoBehaviour
{
    public float timeLimit = 60f;

    [Header("Siren")]
    public float sirenDelay = 5f;
    public float sirenDuration = 5f;
    public float sirenGap = 1f;
    public AudioSource sirenSource;

    [Header("Flood Warning UI")]
    public GameObject floodWarningPanel;
    public TextMeshProUGUI floodTimerText;

    public WaterRiseController water;

    private float timer;
    private bool sirenStarted = false;
    private bool tasksCompleted = false;

    void Start()
    {
        timer = timeLimit;
        floodWarningPanel.SetActive(false);
    }

    void Update()
    {
        if (tasksCompleted) return;

        timer -= Time.deltaTime;

        // 🔴 5. saniyede siren + TV uyarısı
        if (!sirenStarted && timeLimit - timer >= sirenDelay)
        {
            sirenStarted = true;

            floodWarningPanel.SetActive(true);
            StartCoroutine(PlaySirenTwice());
        }

        // 🔴 TV üstündeki geri sayım
        if (floodWarningPanel.activeSelf)
        {
            floodTimerText.text =
                $"FLOOD INCOMING: {Mathf.CeilToInt(timer)} SECONDS";
        }

        // 🔴 süre biterse su yükselsin
        if (timer <= 0f)
        {
            water.waterActive = true;
        }
    }

    IEnumerator PlaySirenTwice()
    {
        sirenSource.Play();
        yield return new WaitForSeconds(sirenDuration);
        sirenSource.Stop();

        yield return new WaitForSeconds(sirenGap);

        sirenSource.Play();
        yield return new WaitForSeconds(sirenDuration);
        sirenSource.Stop();
    }

    public void CompleteAllTasks()
    {
        tasksCompleted = true;

        if (sirenSource.isPlaying)
            sirenSource.Stop();

        floodWarningPanel.SetActive(false);
        water.waterActive = false;
    }
}
