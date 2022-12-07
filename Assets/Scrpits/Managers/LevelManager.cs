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
    LEVEL_MAX
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
    public float current_caminhaoMulti;

    [Header("Set Score per Level")]
    public int changeToLevel_2;
    public int changeToLevel_3;
    public int changeToLevel_4;
    public int changeToLevel_5;


    private float previousSpeed;

    private void OnEnable()
    {
        UtilityEvents.GamePause += StopMovement;
        UtilityEvents.GameResume += ResumeMovement;
    }

    private void OnDisable()
    {
        UtilityEvents.GamePause -= StopMovement;
        UtilityEvents.GameResume -= ResumeMovement;
    }

    private void Start()
    {
        if (GamePlayManager.Instance.isNormalMode == true)
        {
            current_obstacleInitialSpeed = levelData[0].obstacle_Initial_Speed; ;
            current_obstacleSpeed = levelData[0].obstacle_Speed; ;
            current_obstacleSpawnDelay = levelData[0].obstacle_Spawn_Delay;
            current_collectableSpawnDelay = levelData[0].collectable_Spawn_Delay; ;
            current_playerSlideSpeed = levelData[0].player_Slide_Speed;
            current_playerJumpSpeed = levelData[0].player_Jump_Speed;
            current_playerRollingSpeed = levelData[0].player_Roll_Speed;
            current_caminhaoMulti = levelData[0].speedMulti;

        }
        else
        {
            current_obstacleInitialSpeed = levelData[1].obstacle_Initial_Speed;
            current_obstacleSpeed = levelData[1].obstacle_Speed;
            current_obstacleSpawnDelay = levelData[1].obstacle_Spawn_Delay;
            current_collectableSpawnDelay = levelData[1].collectable_Spawn_Delay;
            current_playerSlideSpeed = levelData[1].player_Slide_Speed;
            current_playerJumpSpeed = levelData[1].player_Jump_Speed;
            current_playerRollingSpeed = levelData[1].player_Roll_Speed;
            current_caminhaoMulti = levelData[1].speedMulti;
        }
    }

    public void UpdateLevel(CurrentLevelState newLevel)
    {
        currentLevelState = newLevel;

        StartCoroutine(SettingUpCurrentLevel((int)currentLevelState));
    }

    private IEnumerator SettingUpCurrentLevel(int level)
    {

        currentLevel = level;
        Debug.LogWarning($"Mudou level {currentLevel}");
        float timeToStart = Time.time;

        ScoreEvents.OnChangeLevel(currentLevel);

        while (current_obstacleSpeed <= levelData[currentLevel + 1].obstacle_Speed - 0.5f)
        {

            Debug.LogWarning($"Está lerpando");

            if (GamePlayManager.Instance.isGamePaused) break;

            current_obstacleInitialSpeed = Mathf.Lerp(current_obstacleInitialSpeed, levelData[currentLevel].obstacle_Initial_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            current_obstacleSpeed = Mathf.Lerp(current_obstacleSpeed, levelData[currentLevel].obstacle_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            current_obstacleSpawnDelay = Mathf.Lerp(current_obstacleSpawnDelay, levelData[currentLevel].obstacle_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            current_collectableSpawnDelay = Mathf.Lerp(current_collectableSpawnDelay, levelData[currentLevel].collectable_Spawn_Delay, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            current_playerSlideSpeed = Mathf.Lerp(current_playerSlideSpeed, levelData[currentLevel].player_Slide_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            current_playerJumpSpeed = Mathf.Lerp(current_playerJumpSpeed, levelData[currentLevel].player_Jump_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            current_playerRollingSpeed = Mathf.Lerp(current_playerRollingSpeed, levelData[currentLevel].player_Roll_Speed, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            current_caminhaoMulti = Mathf.Lerp(current_caminhaoMulti, levelData[currentLevel].speedMulti, (Time.deltaTime - timeToStart) * lerpToNextLevel);
            yield return null;
        }


        yield return null;
    }

    private void StopMovement()
    {
        previousSpeed = current_obstacleSpeed;
        current_obstacleSpeed = 0;
    }

    private void ResumeMovement()
    {
        current_obstacleSpeed = previousSpeed;
    }

}
