using UnityEngine;
using System.Collections;
using GameEvents;
using UnityEngine.Rendering;
using Unity.MLAgents.Policies;
using Unity.MLAgents;

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
[DefaultExecutionOrder(2)]
public class PlayerManager : MonoBehaviour
{
    [Header("Player State")]
    [SerializeField] public PlayerState state = PlayerState.IDLE;

    [Header("Training")]
    [SerializeField] public bool isTraining = true;

    private PlayerMovement movement;
    private PlayerGameBehaviour PlayerGameBehaviour;
    private bool isGamePaused = false;
    private PlayerInputHandler playerInputHandler;

    private BehaviorParameters BehaviorParameters;
    private LulaAgent lulaAgent;
    private DecisionRequester DecisionRequester ;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        PlayerGameBehaviour = GetComponent<PlayerGameBehaviour>();
        playerInputHandler = GetComponent<PlayerInputHandler>();

        BehaviorParameters = GetComponent<BehaviorParameters>();
        lulaAgent = GetComponent<LulaAgent>();
        DecisionRequester = GetComponent<DecisionRequester>();
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

    private void Start()
    {
        AgentVerify();
    }

    private void Update()
    {
        if (state != PlayerState.PLAYING || isGamePaused) return;
    }

    private void AgentVerify()
    {
        if (isTraining == true) return;

        //if (GamePlayManager.Instance.currentAgentState == false)
        if (GamePlayManager.Instance.currentAgentState == AgentState.DEACTIVATED)
        {

            Debug.Log("Agent está DESATIVADO");
            BehaviorParameters.enabled = false;
            lulaAgent.enabled = false;
            DecisionRequester.enabled = false; 
        }
        else
        {
            
            Debug.Log("Agent está ATIVADO");

            
        }
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
}