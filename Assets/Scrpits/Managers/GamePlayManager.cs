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
    public bool testStartLevel_5;
    public int winScore;

 

    [HideInInspector]
    public List<MoveBase> objList = new List<MoveBase>();

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

        if (testStartLevel_5)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_5);
        }
        else
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_1);
        }
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
                GameplayEvents.OnGameOver();
                break;
            case GameStates.WIN:
                GameplayEvents.OnWin();
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