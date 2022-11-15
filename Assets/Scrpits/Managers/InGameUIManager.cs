using UnityEngine;
using System.Collections;
using GameEvents;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text levelText;

    int totalScore;
    int currentLevel;

    void Start()
    {
        totalScore = 0;
        scoreText.text = "Estrelas: " + totalScore;
    }

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += UpdateScoreText;
        ScoreEvents.ChangeLevel += UpdateLevelText;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= UpdateScoreText;
        ScoreEvents.ChangeLevel -= UpdateLevelText;
    }

    private void UpdateScoreText(int score)
    {
        totalScore += score;

        scoreText.text = "Estrelas: " + totalScore;
    }

    private void UpdateLevelText(int newLevel)
    {
        currentLevel = newLevel;
        StartCoroutine(LevelTextDelay());

    }

    IEnumerator LevelTextDelay()
    {        
        levelText.text = "Level " + currentLevel;
        levelText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        levelText.gameObject.SetActive(false);
        GameplayEvents.OnStartNewLevel();
    }

}
