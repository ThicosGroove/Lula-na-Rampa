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

    [Header("Play Menu Quit Buttons")]
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject quitButton;

    [Header("Progress Bar")]
    [SerializeField] Image progressBar;
    [SerializeField] Sprite[] progressSprite;

    int totalScore;
    int currentLevel;
    string playerNewName;

    void Start()
    {
        CloseAllPanels();
        OnPlayingPanel.SetActive(true);

        totalScore = 0;
        scoreText.text = "";
        levelText.text = "";

        progressBar.sprite = progressSprite[0];
    }

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += UpdateScoreText;
        ScoreEvents.ScoreGained += UpdateProgressBar;
        ScoreEvents.ChangeLevel += UpdateLevelText;

        GameplayEvents.GameOver += OnGameOverUI;
        GameplayEvents.Win += OnWinUI;
        GameplayEvents.EndGame += OnGameEndButtons;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= UpdateScoreText;
        ScoreEvents.ScoreGained -= UpdateProgressBar;
        ScoreEvents.ChangeLevel -= UpdateLevelText;

        GameplayEvents.GameOver -= OnGameOverUI;
        GameplayEvents.Win -= OnWinUI;
        GameplayEvents.EndGame -= OnGameEndButtons;
    }


    private void UpdateProgressBar(int score)
    {
        // 13 diferentes sprites

    }

    private void UpdateScoreText(int score)
    {
        totalScore += score;

        progressBar.sprite = progressSprite[totalScore];
        scoreText.text = "Estrelas: " + totalScore;
    }

    private void UpdateLevelText(int newLevel)
    {
        currentLevel = newLevel;
        StartCoroutine(LevelTextDelay());
    }

    IEnumerator LevelTextDelay()
    {
        if (currentLevel == 1)
        {
            yield return new WaitForSeconds(8.2f); // Tempo total da cutscene
        }

        GameplayEvents.OnStartNewLevel();

        if (GamePlayManager.Instance.isNormalMode == true)
        {
            levelText.text = "Pegue a Faixa !!";
        }
        else
        {
            levelText.text = "Level " + currentLevel;
        }

        scoreText.text = "Estrelas: " + totalScore;
        levelText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

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
        else if (SaveManager.Instance.LoadFile()._userName != null)
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

        GameOverText.text = "LULA NÃO CONSEGUIU RECEBER A FAIXA";
    }

    private void OnWinUI()
    {
        CloseAllPanels();
        OnWinPanel.SetActive(true);

        WinText.text = "LULÃO RECEBEU A FAIXA!!";
    }

    private void OnGameEndButtons()
    {
        StartCoroutine(TurnOnEndGameButtons());
    }

    IEnumerator TurnOnEndGameButtons()
    {
        yield return new WaitForSeconds(3f);
        playButton.SetActive(true);
        menuButton.SetActive(true);
        quitButton.SetActive(true);
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

        playButton.SetActive(false);
        menuButton.SetActive(false);
        quitButton.SetActive(false);
    }


    #region Buttons
    public void InsertNewName(string name)
    {
        playerNewName = name;
    }

    public void OnClickConfirmNameButton()
    {
        PlayFabGameManager.Instance.OnUpdateDisplayNameRequest(playerNewName);

        InsertNameForScoreBoardPanel.SetActive(false);
        OnGameOverInfinityPanel.SetActive(true);
        ScoreBoardBox.SetActive(true);
    }

    public void OnClickPlayAgainButton()
    {
        SceneManager.LoadScene(Const.GAME_SCENE);
    }

    public void OnClickCancelButton()
    {
        InsertNameForScoreBoardPanel.SetActive(false);
        OnGameOverInfinityPanel.SetActive(true);
        ScoreBoardBox.SetActive(true);
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
        SceneManager.LoadScene(Const.MAIN_MENU_SCENE);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    #endregion Buttons
}
