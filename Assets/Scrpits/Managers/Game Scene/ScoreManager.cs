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

        if (GamePlayManager.Instance.isNormalMode == true)
        {
            VerifyWin();
        }
        else
        {
            VerifyCurrentLevel();
        }
    }

    private void VerifyWin()
    {
        if (totalScoreCurrentRun == GamePlayManager.Instance.winScore)
        {
            Debug.LogWarning("Win");
            GamePlayManager.Instance.UpdateGameState(GameStates.WIN);
        }
    }

    private void VerifyCurrentLevel()
    {

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_2)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_2);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_3)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_3);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_4)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_4);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_5)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_5);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_6)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_6);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_7)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_7);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_8)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_8);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_9)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_9);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_10)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_10);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_11)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_11);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_12)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_12);
        }

        if (totalScoreCurrentRun == LevelManager.Instance.changeToLevel_13)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_MAX);
        }
    }
}
