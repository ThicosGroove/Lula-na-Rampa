using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEvents;
using System.Collections.Generic;

public enum GameStates
{
    PAUSED,
    PREPLAY,
    GAMEOVER,
    MAIN_MENU,
    OPTION_MENU,
    PLAYING,
    WIN
}

public enum CurrentLevelState
{
    LEVEL_1 = 1,
    LEVEL_2 = 2,
    LEVEL_3 = 3,
    LEVEL_4 = 4,
    LEVEL_5
}

// Manager que controla a gameplay e providencia alguns métodos utilitários.
[DefaultExecutionOrder(1)]
public class GamePlayManager : Singleton<GamePlayManager>
{
    [Header("Game State")]
    public GameStates currentGameState = GameStates.MAIN_MENU;
    public static event Action<GameStates> OnGameStateChanged;

    [Header("Game Mode and Testing")]
    public bool isNormalMode;
    public bool playerColliderOn;

    [Header("Score per Level")]
    public int changeToLevel_2;
    public int changeToLevel_3;
    public int changeToLevel_4;
    public int changeToLevel_5;

    [Header("Obstacle Speed per Level")]
    public float obstacleSpeed_Level_1;
    public float obstacleSpeed_Level_2;
    public float obstacleSpeed_Level_3;
    public float obstacleSpeed_Level_4;
    public float obstacleSpeed_Level_5;

    [Header("Obstacle Spawn Delay per Level")]
    public float obstacleDelay_Level_1;
    public float obstacleDelay_Level_2;
    public float obstacleDelay_Level_3;
    public float obstacleDelay_Level_4;
    public float obstacleDelay_Level_5;

    [Header("Player Slide Speed per Level"),]
    public float playerSlideSpeed_Level_1;
    public float playerSlideSpeed_Level_2;
    public float playerSlideSpeed_Level_3;
    public float playerSlideSpeed_Level_4;
    public float playerSlideSpeed_Level_5;

    private CurrentLevelState currentLevel = CurrentLevelState.LEVEL_1;
    public static event Action<CurrentLevelState> OnChangeLevelState;

    [HideInInspector]
    public List<MoveObstacle> objList = new List<MoveObstacle>();

    //protected override void Awake()
    //{
    //    base.Awake();

    //    UtilityEvents.GamePauseToggle += Instance.OnGamePauseToggle;
    //}

    //private void OnApplicationQuit()
    //{

    //    UtilityEvents.GamePauseToggle -= Instance.OnGamePauseToggle;
    //}

    private void Start()
    {
        UpdateGameState(GameStates.PLAYING); // Testing
        UpdateLevel(CurrentLevelState.LEVEL_1);
    }

    public void UpdateGameState(GameStates newState)
    {
        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (currentGameState)
        {
            case GameStates.MAIN_MENU:
                break;
            case GameStates.PREPLAY:
                break;
            case GameStates.PLAYING:
                break;
            case GameStates.OPTION_MENU:
                break;
            case GameStates.PAUSED:
                break;
            case GameStates.GAMEOVER:
                break;
            case GameStates.WIN:
                break;
            default:
                break;
        }
    }

    public void UpdateLevel(CurrentLevelState newLevel) 
    {
        currentLevel = newLevel;
        OnChangeLevelState?.Invoke(newLevel);

        switch (currentLevel)
        {
            case CurrentLevelState.LEVEL_1:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_1));
                break;
            case CurrentLevelState.LEVEL_2:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_2));
                break;
            case CurrentLevelState.LEVEL_3:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_3));
                break;
            case CurrentLevelState.LEVEL_4:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_4));
                break;
            case CurrentLevelState.LEVEL_5:
                ScoreEvents.OnChangeLevel(((int)CurrentLevelState.LEVEL_5));
                break;
            default:
                break;
        }
    }
   

    //void MainMenu()
    //{
    //    GameplayEvents.OnMainMenu();
    //    Time.timeScale = 1;
    //}

    //void PrePlay()
    //{
    //    GameplayEvents.OnPrePlay();
    //}

    //void GameStart()
    //{
    //    GameplayEvents.OnGameStart();
    //    UpdateGameState(GameStates.PLAYING);
    //}

    //void GameOver()
    //{
    //    PlayerEvents.OnPlayerDeath();
    //}

    //void OnGamePauseToggle()
    //{
    //    if (currentGameState == GameStates.PREPLAY
    //    || currentGameState == GameStates.GAMEOVER
    //    || currentGameState == GameStates.MAIN_MENU) return;

    //    if (currentGameState == GameStates.PAUSED)
    //    {
    //        UpdateGameState(GameStates.PLAYING);
    //    }
    //    else
    //    {
    //        UpdateGameState(GameStates.PAUSED);
    //    }

    //    Cursor.lockState = Cursor.lockState == CursorLockMode.Confined ? CursorLockMode.None : CursorLockMode.Confined;
    //    Time.timeScale = 1 - Time.timeScale; // Se timescale for 0, retorna 1, caso seja 1, retorna 0.
    //}
}