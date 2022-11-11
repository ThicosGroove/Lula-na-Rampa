using UnityEngine;
using GameEvents;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    int totalScore;

    void Start()
    {
        totalScore = 0;
        scoreText.text = "Estrelas: " + totalScore;
    }

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += UpdateScoreText;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= UpdateScoreText;
    }

    private void UpdateScoreText(int score)
    {
        totalScore += score;

        scoreText.text = "Estrelas: " + totalScore;
    }
}
