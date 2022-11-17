using UnityEngine;
using System.Collections;
using GameEvents;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
    [Header("Playing Panel")]
    [SerializeField] GameObject OnPlayingPanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text levelText;

    [Header("Game Over Infinity Panel")]
    [SerializeField] GameObject OnGameOverInfinityPanel;

    [Header("Game Over Normal Panel")]
    [SerializeField] GameObject OnGameOverNormalPanel;
    [SerializeField] TMP_Text GameOverText;

    [Header("WinPanel")]
    [SerializeField] GameObject OnWinPanel;
    [SerializeField] TMP_Text WinText;

    int totalScore;
    int currentLevel;

    void Start()
    {
        CloseAllPanels();
        OnPlayingPanel.SetActive(true);

        totalScore = 0;
        scoreText.text = "Estrelas: " + totalScore;
    }

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += UpdateScoreText;
        ScoreEvents.ChangeLevel += UpdateLevelText;

        GameplayEvents.GameOver += OnGameOverUI;
        GameplayEvents.Win += OnWinUI;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= UpdateScoreText;
        ScoreEvents.ChangeLevel -= UpdateLevelText;

        GameplayEvents.GameOver -= OnGameOverUI;
        GameplayEvents.Win -= OnWinUI;
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

    private void OnGameOverUI()
    {
        CloseAllPanels();

        if (GamePlayManager.Instance.isNormalMode)
        {
            OnGameOverNormalPanel.SetActive(true);
        }
        else
        {
            OnGameOverInfinityPanel.SetActive(true);
        }

        GameOverText.text = "Game Over!!";
    }


    private void OnWinUI()
    {
        CloseAllPanels();
        OnWinPanel.SetActive(true);

        WinText.text = "You Win!!";
    }



    private void CloseAllPanels()
    {
        OnPlayingPanel.SetActive(false);
        OnGameOverInfinityPanel.SetActive(false);
        OnGameOverNormalPanel.SetActive(false);
        OnWinPanel.SetActive(false);
    }

}
