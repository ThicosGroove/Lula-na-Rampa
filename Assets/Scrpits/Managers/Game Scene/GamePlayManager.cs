using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEvents;
using System.Collections.Generic;
using System.IO;

public enum GameStates
{
    START_GAME,
    PLAYING,
    PAUSED,
    RESUME,
    GAMEOVER,
    WIN
}

public enum AgentState
{
    DEACTIVATED = 0,
    ACTIVATED = 1
}


[DefaultExecutionOrder(1)]
public class GamePlayManager : Singleton<GamePlayManager>
{
    [Header("Game State")]
    public GameStates currentGameState = GameStates.START_GAME;
    public static event Action<GameStates> OnGameStateChanged;

    [Header("Agent State")]
    public AgentState currentAgentState = AgentState.DEACTIVATED;
    //public bool currentAgentState = false;

    [Header("Game Mode and Testing")]
    public bool isNormalMode;
    public bool isTraining;
    public bool playerColliderOn;
    public bool testStartLevel_5;
    public int winScore;

    [Header("Camera Settings")]
    public Vector3[] cameraPositions; 

    public bool isGamePaused;
    public bool hasReach = false;

    [Header("Object List")]
    public List<GameObject> objList = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        UpdateGameState(GameStates.START_GAME);

        if (File.Exists(Const.GetSaveFilePath()))
        {
            isNormalMode = SaveManager.Instance.LoadFile()._isNormalMode;

            if (isTraining) { currentAgentState = AgentState.ACTIVATED; }
        }

        LevelManager.Instance.UpdateLevel(CurrentLevelState.LEVEL_1);      
    }

    private void Start()
    {
        GameManager.instance.UpdateSceneState(SceneState.GAME);

        currentAgentState = SaveManager.instance.LoadFile()._agentState;

        Debug.Log($"Agent state = {currentAgentState}");
    }

    public void UpdateGameState(GameStates newState)
    {
        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (currentGameState)
        {
            case GameStates.START_GAME:
                StartCoroutine(StartAfterCutscene());
                break;
            case GameStates.PLAYING:

                break;
            case GameStates.PAUSED:
                isGamePaused = true;
                UtilityEvents.OnGamePause();
                break;
            case GameStates.RESUME:
                isGamePaused = false;
                UtilityEvents.OnGameResume();
                UpdateGameState(GameStates.PLAYING);
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

    IEnumerator StartAfterCutscene()
    {
        yield return new WaitForSeconds(Const.CUTSECNE_TIME);

        GameplayEvents.OnStartNewLevel();
        UpdateGameState(GameStates.PLAYING);


    }
}