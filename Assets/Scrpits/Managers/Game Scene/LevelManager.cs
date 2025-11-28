using System;
using System.Collections;
using UnityEngine;
using GameEvents;

public enum CurrentLevelState
{
    LEVEL_1 = 0,
    LEVEL_2 = 1,
    LEVEL_3 = 2,
    LEVEL_4 = 3,
    LEVEL_5 = 4,
    LEVEL_6 = 5,
    LEVEL_7 = 6,
    LEVEL_8 = 7,
    LEVEL_9 = 8,
    LEVEL_10 = 9,
    LEVEL_11 = 10,
    LEVEL_12 = 11,
    LEVEL_MAX = 12
}

[DefaultExecutionOrder(-50)]
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelSO[] levelData;

    private CurrentLevelState currentLevelState = CurrentLevelState.LEVEL_1;
    public int currentLevelIndex;

    [Header("Current Variables Apply")]
    public float current_obstacleSpeed;

    // NOVA VARIÁVEL CENTRALIZADA
    public float current_obstacleSpawnDistance;

    public float current_playerSlideSpeed;
    public float current_playerJumpHeight;
    public float current_playerRollingSpeed;

    [Header("Set Score per Level")]
    public int changeToLevel_2;
    public int changeToLevel_3;
    public int changeToLevel_4;
    public int changeToLevel_5;
    public int changeToLevel_6;
    public int changeToLevel_7;
    public int changeToLevel_8;
    public int changeToLevel_9;
    public int changeToLevel_10;
    public int changeToLevel_11;
    public int changeToLevel_12;
    public int changeToLevel_13;

    private float previousSpeed;
    private bool isPaused = false;

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
        if (GamePlayManager.Instance != null && GamePlayManager.Instance.isNormalMode)
        {
            ApplyLevelData(0);
        }
        else
        {
            ApplyLevelData(1);
        }
    }

    public void UpdateLevel(CurrentLevelState newLevel)
    {
        currentLevelState = newLevel;
        int levelIndex = (int)currentLevelState;
        StartCoroutine(SettingUpCurrentLevel(levelIndex));
    }

    private IEnumerator SettingUpCurrentLevel(int levelIndex)
    {
        ApplyLevelData(levelIndex);
        ScoreEvents.OnChangeLevel(levelIndex);
        yield return null;
    }

    private void ApplyLevelData(int index)
    {
        if (index >= levelData.Length) index = levelData.Length - 1;

        currentLevelIndex = index;

        current_obstacleSpeed = levelData[index].obstacle_Speed;
        current_obstacleSpawnDistance = levelData[index].obstacle_Spawn_Distance;
        current_playerSlideSpeed = levelData[index].player_Slide_Speed;
        current_playerJumpHeight = levelData[index].player_Jump_Height;
        current_playerRollingSpeed = levelData[index].player_Roll_Speed;
    }

    // Métodos auxiliares
    public LevelSO GetLevelData(int index)
    {
        if (index >= 0 && index < levelData.Length) return levelData[index];
        return levelData[levelData.Length - 1];
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    private void StopMovement()
    {
        if (isPaused) return;
        previousSpeed = current_obstacleSpeed;
        current_obstacleSpeed = 0;
        isPaused = true;
    }

    private void ResumeMovement()
    {
        if (!isPaused) return;
        current_obstacleSpeed = previousSpeed;
        isPaused = false;
    }
}