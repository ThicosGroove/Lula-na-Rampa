﻿using UnityEngine;
using GameEvents;

enum PlayerState
{
    IDLE,
    JUMP,
    SLIDING,
    PLAYING,
    DEAD,
    WIN
}

// CRIAR UM PLAYER ANIMATION HANDLER P LIDAR COM AS ANIMAÇÕES
// MUDAR OS BOTOES
public class PlayerController : MonoBehaviour
{
    [SerializeField] float laneDistance;
    [SerializeField] float slideSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpSpeed;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    float height;
    int desiredLane;
    Vector3 targetPosition;
    Vector3 targetJumpPosition;
    Vector3 jumpPosition;

    int currentLevel;

    [SerializeField] PlayerState state;

    private void Awake()
    {
        height = 3;
        state = PlayerState.PLAYING;
        desiredLane = Const.PLAYER_INITIAL_LANE;
    }

    private void Start()
    {
        if (GamePlayManager.Instance.isNormalMode)
        {
            slideSpeed = GamePlayManager.Instance.normalSpeed;
        }
    }

    private void OnEnable()
    {
        ScoreEvents.ChangeLevel += updateSideSpeed;

        GameplayEvents.Win += OnPlayerWin;
    }

    private void OnDisable()
    {
        ScoreEvents.ChangeLevel -= updateSideSpeed;

        GameplayEvents.Win -= OnPlayerWin;
    }

    private void UpdatePlayerState(PlayerState newState)
    {
        state = newState;

        if (state != PlayerState.PLAYING)
        {
            slideSpeed = 0;
            jumpSpeed = 0;
        }
    }

    void Update()
    {
        if (state != PlayerState.PLAYING) return;
        Move();
        Jump();
        //CheckingGround();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        switch (desiredLane)
        {
            case 0:
                targetPosition = transform.position.x * Vector3.zero + Vector3.left * laneDistance + Vector3.up * height;
                break;
            case 1:
                targetPosition = transform.position.x * Vector3.zero + Vector3.up * height;
                break;
            case 2:
                targetPosition = transform.position.x * Vector3.zero + Vector3.right * laneDistance + Vector3.up * height;
                break;
            default:
                break;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, slideSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && CheckingGround())
        {
            targetJumpPosition = Vector3.up * jumpHeight;
        }

        transform.Translate(targetJumpPosition * jumpSpeed * Time.deltaTime);

        if (transform.position.y >= targetJumpPosition.y)
        {
            targetJumpPosition = Vector3.zero;
        }
    }


    void updateSideSpeed(int newLevel)
    {
        currentLevel = newLevel;

        switch (currentLevel)
        {
            case 1:
                slideSpeed = GamePlayManager.Instance.playerSlideSpeed_Level_1;
                break;
            case 2:
                slideSpeed = GamePlayManager.Instance.playerSlideSpeed_Level_2;
                break;
            case 3:
                slideSpeed = GamePlayManager.Instance.playerSlideSpeed_Level_3;
                break;
            case 4:
                slideSpeed = GamePlayManager.Instance.playerSlideSpeed_Level_4;
                break;
            case 5:
                slideSpeed = GamePlayManager.Instance.playerSlideSpeed_Level_5;
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!GamePlayManager.Instance.playerColliderOn) return;

        if (other.gameObject.CompareTag(Const.OBSTACLE_TAG))
        {
            UpdatePlayerState(PlayerState.DEAD);

            Debug.Log("Game Over");
            GamePlayManager.Instance.UpdateGameState(GameStates.GAMEOVER);
        }
    }

    bool CheckingGround()
    {
        bool ray = Physics.CheckSphere(groundCheck.position, 1f, groundMask);

        slideSpeed = jumpSpeed;
        return ray;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(groundCheck.position, 1f);
    }

    void OnPlayerWin()
    {
        UpdatePlayerState(PlayerState.WIN);
    }
}
