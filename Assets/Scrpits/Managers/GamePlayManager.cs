using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEvents;
using System.Collections.Generic;
using System.IO;

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

    private void Start()
    {
        if (File.Exists(Application.dataPath + Const.SAVE_FILE_PATH))
        {
            isNormalMode = SaveManager.Instance.LoadFile()._isNormalMode;
        }

        UpdateGameState(GameStates.PLAYING); // Testing

        if (testStartLevel_5)
        {
            LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_MAX);
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
}