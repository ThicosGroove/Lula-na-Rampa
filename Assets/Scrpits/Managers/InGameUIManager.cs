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
    [SerializeField] GameObject InsertNameForScoreBoardPanel;
    [SerializeField] GameObject ScoreBoardBox;

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
        if (GamePlayManager.Instance.isNormalMode)
        {
            levelText.text = "Pegue a Faixa !!";
        }
        else
        {
            levelText.text = "Level " + currentLevel;
        }

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
        else if (PlayerPrefs.HasKey(Const.PLAYER_ID))
        {
            OnGameOverInfinityPanel.SetActive(true);
            ScoreBoardBox.SetActive(true);
        }
        else
        {
            InsertNameForScoreBoardPanel.SetActive(true);
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
        InsertNameForScoreBoardPanel.SetActive(false);
        ScoreBoardBox.SetActive(false);
        OnGameOverNormalPanel.SetActive(false);
        OnWinPanel.SetActive(false);
    }


    #region Buttons
    public void InsertNewName(string name)
    {
        // fazer uma chamada de evento para o Play Fab Game Manager
        // On Display Name Update
        PlayFabGameManager.Instance.OnUpdateDisplayNameRequest(name);
    }

    public void OnClickConfirmNameButton()
    {
        // Atualizar o nome do jogador lá no sistema
        OnGameOverInfinityPanel.SetActive(true);
        ScoreBoardBox.SetActive(true);
    }

    public void OnClickCancelButton()
    {
        // Deletar o jogador lá no sistema
        // mostro o score board de qualquer forma??
        // acho que sim, pra instigar a vontade do jogador
    }

    #endregion Buttons
}
