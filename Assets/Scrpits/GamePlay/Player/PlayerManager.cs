using UnityEngine;
using System.Collections;
using GameEvents;
using UnityEngine.Rendering;

public enum PlayerState
{
    IDLE,
    PLAYING,
    DEAD,
    WIN
}

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerGameBehaviour))]
public class PlayerManager : MonoBehaviour
{
    [Header("Player State")]
    [SerializeField] public PlayerState state = PlayerState.IDLE;

    [Header("Training")]
    [SerializeField] public bool isTraining = false;

    private PlayerMovement movement;
    private PlayerGameBehaviour PlayerGameBehaviour;
    private bool isGamePaused = false;
    private PlayerInputHandler playerInputHandler;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        PlayerGameBehaviour = GetComponent<PlayerGameBehaviour>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += OnGameStart;
        GameplayEvents.GameOver += OnGameOver;
        GameplayEvents.Win += OnPlayerWin;
        UtilityEvents.GamePause += OnGamePause;
        UtilityEvents.GameResume += OnGameResume;
        TrainingEvents.RewardLoss += TrainingRewardLoss;
    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= OnGameStart;
        GameplayEvents.GameOver -= OnGameOver;
        GameplayEvents.Win -= OnPlayerWin;
        UtilityEvents.GamePause -= OnGamePause;
        UtilityEvents.GameResume -= OnGameResume;
        TrainingEvents.RewardLoss -= TrainingRewardLoss;
    }

    private void Update()
    {
        if (state != PlayerState.PLAYING || isGamePaused) return;
    }

    private void OnGameStart()
    {
        state = PlayerState.PLAYING;
        playerInputHandler.canMove = true;
        movement.ResetPosition();
    }

    private void OnGameOver()
    {
        state = PlayerState.DEAD;
        playerInputHandler.canMove = false;
        movement.StopMovement();
    }

    private void OnPlayerWin()
    {
        state = PlayerState.WIN;
        playerInputHandler.canMove = false;
        movement.WinMovement();
    }

    private void OnGamePause()
    {
        isGamePaused = true;
        playerInputHandler.canMove = false;
    }

    private void OnGameResume()
    {
        isGamePaused = false;
        playerInputHandler.canMove = true;
    }

    public void UpdatePlayerState(PlayerState newState)
    {
        state = newState;
    }

    private void TrainingRewardLoss()
    {

    }
}