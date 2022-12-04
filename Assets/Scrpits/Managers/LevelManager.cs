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

[DefaultExecutionOrder(2)]
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelSO[] levelData;

    private CurrentLevelState currentLevelState = CurrentLevelState.LEVEL_1;

    public int currentLevel;

    public float lerpToNextLevel;

    [Header("Current Variables Apply")]
    public float current_obstacleInitialSpeed;
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

    private void Start()
    {
        if (GamePlayManager.Instance.isNormalMode == true)
        {
            current_obstacleSpeed = normalObstacleSpeed;
            current_obstacleSpawnDelay = normalObstacleDelay;
            current_collectableSpawnDelay = normalObstacleDelay;
            current_playerSlideSpeed = playerNormal_SlideSpeed;
            current_playerJumpSpeed = playerNormal_JumpSpeed;
            current_playerRollingSpeed = playerNormal_RollingSpeed;
        }
        else
        {
            current_obstacleInitialSpeed = levelData[0].obstacle_Initial_Speed;
            current_obstacleSpeed = levelData[0].obstacle_Speed;
            current_obstacleSpawnDelay = levelData[0].obstacle_Spawn_Delay;
            current_collectableSpawnDelay = levelData[0].collectable_Spawn_Delay;
            current_playerSlideSpeed = levelData[0].player_Slide_Speed;
            current_playerJumpSpeed = levelData[0].player_Jump_Speed;
            current_playerRollingSpeed = levelData[0].player_Roll_Speed;
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
                //    current_obstacleSpeed = obstacleSpeed_Level_1;
                //    current_obstacleSpawnDelay = obstacleSpawnDelay_Level_1;
                //    current_collectableSpawnDelay = collectableDelay_Level_1;
                //    current_playerSlideSpeed = playerSlideSpeed_Level_1;
                //    current_playerJumpSpeed = playerJumpSpeed_Level_1;
                //    current_playerRollingSpeed = playerRollingDelay_Level_1;
                //    current_obstacleInitialSpeed = obstacleInitialSpeed_Level_1;
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_1));
                break;
            case 2:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_2));
                while (current_obstacleSpeed <= levelData[1].obstacle_Speed - 0.1f)
                {
                    current_obstacleSpeed = Mathf.Lerp(current_obstacleSpeed, levelData[1].obstacle_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(current_obstacleSpawnDelay, levelData[1].obstacle_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(current_collectableSpawnDelay, levelData[1].collectable_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(current_playerSlideSpeed, levelData[1].player_Slide_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(current_playerJumpSpeed, levelData[1].player_Jump_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(current_playerRollingSpeed, levelData[1].player_Roll_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleInitialSpeed = Mathf.Lerp(current_obstacleInitialSpeed, levelData[1].obstacle_Initial_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    yield return null;
                }
                break;
            case 3:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_3));
                while (current_obstacleSpeed <= levelData[2].obstacle_Speed - 0.1f)
                {
                    current_obstacleSpeed = Mathf.Lerp(current_obstacleSpeed, levelData[2].obstacle_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(current_obstacleSpawnDelay, levelData[2].obstacle_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(current_collectableSpawnDelay, levelData[2].collectable_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(current_playerSlideSpeed, levelData[2].player_Slide_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(current_playerJumpSpeed, levelData[2].player_Jump_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(current_playerRollingSpeed, levelData[2].player_Roll_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleInitialSpeed = Mathf.Lerp(current_obstacleInitialSpeed, levelData[2].obstacle_Initial_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    yield return null;
                }
                break;
            case 4:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_4));
                while (current_obstacleSpeed <= levelData[3].obstacle_Speed - 0.1f)
                {
                    current_obstacleSpeed = Mathf.Lerp(current_obstacleSpeed, levelData[3].obstacle_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(current_obstacleSpawnDelay, levelData[3].obstacle_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(current_collectableSpawnDelay, levelData[3].collectable_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(current_playerSlideSpeed, levelData[3].player_Slide_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(current_playerJumpSpeed, levelData[3].player_Jump_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(current_playerRollingSpeed, levelData[3].player_Roll_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleInitialSpeed = Mathf.Lerp(current_obstacleInitialSpeed, levelData[3].obstacle_Initial_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);

                    yield return null;
                }
                break;
            case 5:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_5));
                while (current_obstacleSpeed <= levelData[4].obstacle_Speed - 0.1f)
                {
                    current_obstacleSpeed = Mathf.Lerp(current_obstacleSpeed, levelData[4].obstacle_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleSpawnDelay = Mathf.Lerp(current_obstacleSpawnDelay, levelData[4].obstacle_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_collectableSpawnDelay = Mathf.Lerp(current_collectableSpawnDelay, levelData[4].collectable_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerSlideSpeed = Mathf.Lerp(current_playerSlideSpeed, levelData[4].player_Slide_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerJumpSpeed = Mathf.Lerp(current_playerJumpSpeed, levelData[4].player_Jump_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_playerRollingSpeed = Mathf.Lerp(current_playerRollingSpeed, levelData[4].player_Roll_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    current_obstacleInitialSpeed = Mathf.Lerp(current_obstacleInitialSpeed, levelData[4].obstacle_Initial_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
                    yield return null;
                }
                break;
            default:
                break;
        }


        yield return null;
    }
}
