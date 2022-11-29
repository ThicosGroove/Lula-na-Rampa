using System;
using System.Collections;
using UnityEngine;
using GameEvents;

public enum CurrentLevelState
{
    LEVEL_1 = 1,
    LEVEL_2 = 2,
    LEVEL_3 = 3,
    LEVEL_4 = 4,
    LEVEL_5
}

public class LevelManager : Singleton<LevelManager>
{
    private CurrentLevelState currentLevelState = CurrentLevelState.LEVEL_1;

    int currentLevel;

    public float lerpToNextLevel;

    [Header("Current Variables Apply")]
    public float current_obstacleSpeed;
    public float current_obstacleSpawnDelay;
    public float current_collectableSpawnDelay;
    public float current_playerSlideSpeed;
    public float current_playerJumpSpeed;
    public float current_playerRollingSpeed;

    [Header("Set Speed And Delay on Normal Mode")]
    public float normalObstacleSpeed;
    public float normalObstacleDelay;
    public float playerNormal_SlideSpeed;
    public float playerNormal_JumpSpeed;
    public float playerNormal_RollingSpeed;

    [Header("Set Score per Level")]
    public int changeToLevel_2;
    public int changeToLevel_3;
    public int changeToLevel_4;
    public int changeToLevel_5;

    [Header("Set Obstacle/Collectable Speed per Level")]
    public float obstacleSpeed_Level_1;
    public float obstacleSpeed_Level_2;
    public float obstacleSpeed_Level_3;
    public float obstacleSpeed_Level_4;
    public float obstacleSpeed_Level_5;

    [Header("Set Obstacle Spawn Delay per Level")]
    public float obstacleSpawnDelay_Level_1;
    public float obstacleSpawnDelay_Level_2;
    public float obstacleSpawnDelay_Level_3;
    public float obstacleSpawnDelay_Level_4;
    public float obstacleSpawnDelay_Level_5;

    [Header("Set Collectable Spawn Delay per Level")]
    public float collectableDelay_Level_1;
    public float collectableDelay_Level_2;
    public float collectableDelay_Level_3;
    public float collectableDelay_Level_4;
    public float collectableDelay_Level_5;

    [Header("Set Player Slide Speed per Level"),]
    public float playerSlideSpeed_Level_1;
    public float playerSlideSpeed_Level_2;
    public float playerSlideSpeed_Level_3;
    public float playerSlideSpeed_Level_4;
    public float playerSlideSpeed_Level_5;

    [Header("Set Player Jump Speed per Level"),]
    public float playerJumpSpeed_Level_1;
    public float playerJumpSpeed_Level_2;
    public float playerJumpSpeed_Level_3;
    public float playerJumpSpeed_Level_4;
    public float playerJumpSpeed_Level_5;

    [Header("Set Player Rolling Delay per Level"),]
    public float playerRollingDelay_Level_1;
    public float playerRollingDelay_Level_2;
    public float playerRollingDelay_Level_3;
    public float playerRollingDelay_Level_4;
    public float playerRollingDelay_Level_5;


    private void Start()
    {
        if (GamePlayManager.Instance.isNormalMode)
        {

        }
    }

    public void UpdateLevel(CurrentLevelState newLevel)
    {
        if (GamePlayManager.Instance.testStartLevel_5) return;

        currentLevelState = newLevel;

        StartCoroutine(SettingUpCurrentLevel((int)currentLevelState));
    }


    private IEnumerator SettingUpCurrentLevel(int level)
    {
        currentLevel = level;
        float timeToStart = Time.time;

        switch (currentLevel)
        {
            case 1:
                Debug.LogWarning("Level 1");
                current_obstacleSpeed = obstacleSpeed_Level_1;
                current_obstacleSpawnDelay = obstacleSpawnDelay_Level_1;
                current_collectableSpawnDelay = collectableDelay_Level_1;
                current_playerSlideSpeed = playerSlideSpeed_Level_1;
                current_playerJumpSpeed = playerJumpSpeed_Level_1;
                current_playerRollingSpeed = playerRollingDelay_Level_1;
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_1));
                break;
            case 2:
                Debug.LogWarning("Level 2");
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_2));
                while (current_obstacleSpeed <= obstacleSpeed_Level_2 - 0.1f)
                {
                    Debug.LogWarning("LERPING 2");
                    current_obstacleSpeed = Mathf.Lerp(obstacleSpeed_Level_1, obstacleSpeed_Level_2, (Time.time - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(obstacleSpawnDelay_Level_1, obstacleSpawnDelay_Level_2, (Time.time - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(collectableDelay_Level_1, collectableDelay_Level_2, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(playerSlideSpeed_Level_1, playerSlideSpeed_Level_2, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(playerJumpSpeed_Level_1, playerJumpSpeed_Level_2, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(playerRollingDelay_Level_1, playerRollingDelay_Level_2, (Time.time - timeToStart) * lerpToNextLevel);
                    yield return null;
                }
                Debug.LogWarning("SAiu lerp level 2");
                break;
            case 3:
                Debug.LogWarning("Level 3");
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_3));
                while (current_obstacleSpeed <= obstacleSpeed_Level_3 - 0.1f)
                {
                    Debug.LogWarning("LERPING 3");
                    current_obstacleSpeed = Mathf.Lerp(obstacleSpeed_Level_2, obstacleSpeed_Level_3, (Time.time - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(obstacleSpawnDelay_Level_2, obstacleSpawnDelay_Level_3, (Time.time - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(collectableDelay_Level_2, collectableDelay_Level_3, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(playerSlideSpeed_Level_2, playerSlideSpeed_Level_3, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(playerJumpSpeed_Level_2, playerJumpSpeed_Level_3, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(playerRollingDelay_Level_2, playerRollingDelay_Level_3, (Time.time - timeToStart) * lerpToNextLevel);
                    yield return null;
                }
                Debug.LogWarning("SAiu lerp level 3");
                break;
            case 4:
                Debug.LogWarning("Level 4");
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_4));
                while (current_obstacleSpeed <= obstacleSpeed_Level_4 - 0.1f)
                {
                    Debug.LogWarning("LERPING 4");
                    current_obstacleSpeed = Mathf.Lerp(obstacleSpeed_Level_3, obstacleSpeed_Level_4, (Time.time - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(obstacleSpawnDelay_Level_3, obstacleSpawnDelay_Level_4, (Time.time - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(collectableDelay_Level_3, collectableDelay_Level_4, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(playerSlideSpeed_Level_3, playerSlideSpeed_Level_4, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(playerJumpSpeed_Level_3, playerJumpSpeed_Level_4, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(playerRollingDelay_Level_3, playerRollingDelay_Level_4, (Time.time - timeToStart) * lerpToNextLevel);
                    yield return null;
                }
                Debug.LogWarning("SAiu lerp level 4");
                break;
            case 5:
                Debug.LogWarning("Level 5");
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_5));
                while (current_obstacleSpeed <= obstacleSpeed_Level_5 - 0.1f)
                {
                    Debug.LogWarning("LERPING 5");
                    current_obstacleSpeed = Mathf.Lerp(obstacleSpeed_Level_4, obstacleSpeed_Level_5, (Time.time - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(obstacleSpawnDelay_Level_4, obstacleSpawnDelay_Level_5, (Time.time - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(collectableDelay_Level_4, collectableDelay_Level_5, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(playerSlideSpeed_Level_4, playerSlideSpeed_Level_5, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(playerJumpSpeed_Level_4, playerJumpSpeed_Level_5, (Time.time - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(playerRollingDelay_Level_4, playerRollingDelay_Level_5, (Time.time - timeToStart) * lerpToNextLevel);
                    yield return null;
                }
                Debug.LogWarning("SAiu lerp level 5");
                break;
            default:
                break;
        }


        yield return null;
    }
}
