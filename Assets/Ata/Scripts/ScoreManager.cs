using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public Color positiveColor = Color.green;
    public Color zeroColor = Color.cyan;   // 0 için mavi/cyan daha temiz
    public Color negativeColor = Color.red;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = $"SCORE: {score}";

        if (score > 0)
            scoreText.color = positiveColor;
        else if (score < 0)
            scoreText.color = negativeColor;
        else
            scoreText.color = zeroColor;
    }
}
