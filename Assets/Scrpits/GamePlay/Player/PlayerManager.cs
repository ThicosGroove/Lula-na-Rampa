using UnityEngine;
using System.Collections;
using GameEvents;

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
    [SerializeField] private PlayerState state = PlayerState.IDLE;

    private PlayerMovement movement;
    private PlayerGameBehaviour PlayerGameBehaviour;
    private bool isGamePaused = false;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        PlayerGameBehaviour = GetComponent<PlayerGameBehaviour>();
    }

    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += OnGameStart;
        GameplayEvents.GameOver += OnGameOver;
        GameplayEvents.Win += OnPlayerWin;
        UtilityEvents.GamePause += OnGamePause;
        UtilityEvents.GameResume += OnGameResume;
    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= OnGameStart;
        GameplayEvents.GameOver -= OnGameOver;
        GameplayEvents.Win -= OnPlayerWin;
        UtilityEvents.GamePause -= OnGamePause;
        UtilityEvents.GameResume -= OnGameResume;
    }

    private void Update()
    {
        if (state != PlayerState.PLAYING || isGamePaused) return;
    }

    private void OnGameStart()
    {
        state = PlayerState.PLAYING;
        movement.ResetPosition();
    }

    private void OnGameOver()
    {
        state = PlayerState.DEAD;
        movement.StopMovement();
    }

    private void OnPlayerWin()
    {
        state = PlayerState.WIN;
        movement.WinMovement();
    }

    private void OnGamePause()
    {
        isGamePaused = true;
    }

    private void OnGameResume()
    {
        isGamePaused = false;
    }

    public void UpdatePlayerState(PlayerState newState)
    {
        state = newState;
    }
}