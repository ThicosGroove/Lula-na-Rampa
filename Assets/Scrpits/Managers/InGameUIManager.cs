using UnityEngine;
using System.Collections;
using GameEvents;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [Header("Playing Panel")]
    [SerializeField] GameObject OnPlayingPanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text levelText;

    [Header("Pause Panel")]
    [SerializeField] GameObject OnPausePanel;

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


    private void UpdateProgressBar()
    {
        // 13 diferentes sprites
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
        if (GamePlayManager.Instance.isNormalMode == true)
        {
            levelText.text = "Pegue a Faixa !!";
        }
        else
        {
            levelText.text = "Level " + currentLevel;
        }

        levelText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        GameplayEvents.OnStartNewLevel();
        levelText.gameObject.SetActive(false);
    }

    private void OnGameOverUI()
    {
        CloseAllPanels();
        OnPlayingPanel.SetActive(true);

        if (GamePlayManager.Instance.isNormalMode == true)
        {
            OnGameOverNormalPanel.SetActive(true);
        }
        else if (SaveManager.Instance.LoadFile()._playerID != null)
        {
            OnGameOverInfinityPanel.SetActive(true);
            ScoreBoardBox.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Abriu confirm name panel");
            OnGameOverInfinityPanel.SetActive(true);
            InsertNameForScoreBoardPanel.SetActive(true);
        }

        GameOverText.text = "Perdeu!!";
    }

    private void OnWinUI()
    {
        CloseAllPanels();
        OnWinPanel.SetActive(true);

        WinText.text = "Você Ganhou!!";
    }

    private void CloseAllPanels()
    {
        OnPlayingPanel.SetActive(false);
        OnPausePanel.SetActive(false);
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

    public void OnClickPlayAgainButton()
    {
        SceneManager.LoadScene(1); // TEMPORÁRIO
    }

    public void OnClickCancelButton()
    {
        // Deletar o jogador lá no sistema
        // mostro o score board de qualquer forma??
        // acho que sim, pra instigar a vontade do jogador
    }

    public void OnClickPauseButton()
    {
        if (!GamePlayManager.Instance.isGamePaused)
        {
            GamePlayManager.Instance.UpdateGameState(GameStates.PAUSED);
            OnPausePanel.SetActive(true);
        }
        else
        {
            GamePlayManager.Instance.UpdateGameState(GameStates.RESUME);
            OnPausePanel.SetActive(false);
        }
    }

    public void OnClickReturnToMenuButton()
    {
        SceneManager.LoadScene(Const.HOME_SCENE);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    #endregion Buttons
}
