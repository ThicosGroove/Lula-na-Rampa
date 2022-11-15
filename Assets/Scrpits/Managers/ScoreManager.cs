using UnityEngine;
using GameEvents;

public class ScoreManager : Singleton<ScoreManager>
{
    public int totalScoreCurrentRun;

    // Start is called before the first frame update
    void Start()
    {
        totalScoreCurrentRun = 0;
    }

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += UpdateScore;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= UpdateScore;
    }

    void UpdateScore(int score)
    {
        totalScoreCurrentRun += score;

        if (GamePlayManager.Instance.isNormalMode)
        {
            VerifyWin();
        }
        else
        {
        }
        VerifyCurrentLevel();
    }

    private void VerifyWin()
    {
        // TEM QUE AJUSTAR O MODO DE JOGO
        // SÓ TEM VITÓRIA NO MODO DE JOGO ESPECÍFICO
        if (totalScoreCurrentRun >= 13)
        {
            // Update GameState WIN
        }
    }

    private void VerifyCurrentLevel()
    {
        if (totalScoreCurrentRun == GamePlayManager.Instance.changeToLevel_2)
        {
            GamePlayManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_2);
        }

        if (totalScoreCurrentRun == GamePlayManager.Instance.changeToLevel_3)
        {
            GamePlayManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_3);
        }

        if (totalScoreCurrentRun == GamePlayManager.Instance.changeToLevel_4)
        {
            GamePlayManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_4);
        }

        if (totalScoreCurrentRun == GamePlayManager.Instance.changeToLevel_5)
        {
            GamePlayManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_5);
        }
    }
}
