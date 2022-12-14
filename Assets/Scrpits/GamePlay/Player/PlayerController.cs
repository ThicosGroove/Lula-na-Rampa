using UnityEngine;
using System.Collections;
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
    [Header("Player current State")]
    [SerializeField] PlayerState state;

    [Header("Gameplay parameters")]
    [SerializeField] float jumpHeight;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    [Header("Ground Check parameters")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    [Header("Graphics parameters")]
    [SerializeField] Transform GFX_transform;

    // PRIVATES PARAMETERS 
    InputPlayerControsl input;

    // position parameters
    float height;
    int desiredLane;
    Vector3 targetPosition;
    Vector3 targetJumpPosition;

    // move / jump parameters
    float slideSpeed;
    float jumpSpeed;
    public bool isJump;

    // rotation parameters
    [SerializeField] float rotateBackDelay;
    [SerializeField] float rotateBackSpeed;
    float rotationAngleY = Const.PLAYER_ROTATION_MOVE;
    int isMoving = 0;

    // collider / roll parameters
    CapsuleCollider coll;
    float colliderHeight = 1f;
    float colliderCenter = -0.5f;
    public bool isRolling;
    float rollingDelay;

    int currentLevel;
    bool hasReachPalace = false;
    Vector3 lookToCamera = new Vector3(0, 180f, 0f);

    #region SetUp 
    private void Awake()
    {
        input = new InputPlayerControsl();

        height = 0;
        state = PlayerState.PLAYING;
        desiredLane = Const.PLAYER_INITIAL_LANE;

        coll = GetComponent<CapsuleCollider>();
        coll.isTrigger = true;
    }

    private void Start()
    {
        state = PlayerState.IDLE;

        if (GamePlayManager.Instance.isNormalMode == true)
        {
            slideSpeed = LevelManager.Instance.current_playerSlideSpeed;
            jumpSpeed = LevelManager.Instance.current_playerJumpSpeed;
            rollingDelay = LevelManager.Instance.current_playerRollingSpeed;
        }
    }

    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += GameStart;
        GameplayEvents.Win += OnPlayerWin;
        GameplayEvents.ReachPalace += ReachPalaceRotateRotate;

        input.Enable();
    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= GameStart;
        GameplayEvents.Win -= OnPlayerWin;
        GameplayEvents.ReachPalace -= ReachPalaceRotateRotate;

        input.Disable();
    }

    private void GameStart()
    {
        UpdatePlayerState(PlayerState.PLAYING);
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
    #endregion SetUp

    #region Movement

    #region Mobilie
    public void SwipeDirection(Vector2 direction)
    {
        if (GamePlayManager.Instance.isGamePaused) return;

        if (Vector2.Dot(Vector2.left, direction) > directionThreshold) // Esquerda
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
                return;
            }


            isMoving = 2;
            GFX_Rotation();
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold) // Direita
        {
            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
                return;
            }

            isMoving = 1;
            GFX_Rotation();
        }
        else if (Vector2.Dot(Vector2.up, direction) > directionThreshold) // Pulo
        {
            if (CheckingGround())
            {
                targetJumpPosition = Vector3.up * jumpHeight;
                //isJump = true;
            }

            transform.Translate(targetJumpPosition * jumpSpeed * Time.deltaTime);


            if (transform.position.y >= targetJumpPosition.y && !CheckingGround())
            {
                targetJumpPosition = Vector3.zero;
            }
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold) // Abaixar
        {
            if (!isRolling && CheckingGround())
            {
                isRolling = true;
                StartCoroutine(RollDelay());
            }

            if (Vector2.Dot(Vector2.down, direction) > directionThreshold && !CheckingGround())
            {
                targetJumpPosition = Vector3.zero;
            }

        }
    }
    #endregion Mobile

    void Update()
    {
        if (state == PlayerState.WIN)
        {
            WinMovement();

            if (hasReachPalace)
            {
                WinRotation();
            }
        }


        if (state != PlayerState.PLAYING || GamePlayManager.Instance.isGamePaused) return;
        MoveHandle();
        MoveInput();
        Jump();
        Roll();

        updateSideSpeed();
    }

    void updateSideSpeed()
    {
        slideSpeed = LevelManager.Instance.current_playerSlideSpeed;
        jumpSpeed = LevelManager.Instance.current_playerJumpSpeed;
        rollingDelay = LevelManager.Instance.current_playerRollingSpeed;
    }


    #region Sideways
    public void MoveInput()
    {
        if (input.Movement.Right.triggered)
        {
            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
                return;
            }

            isMoving = 1;
            GFX_Rotation();

        }
        if (input.Movement.Left.triggered)
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
                return;
            }

            isMoving = 2;
            GFX_Rotation();
        }
    }

    private void MoveHandle()
    {
        switch (desiredLane)
        {
            case 0:
                targetPosition = transform.position.x * Vector3.zero + Vector3.left * Const.LANE_DISTANCE + Vector3.up * height;
                break;
            case 1:
                targetPosition = transform.position.x * Vector3.zero + Vector3.up * height;
                break;
            case 2:
                targetPosition = transform.position.x * Vector3.zero + Vector3.right * Const.LANE_DISTANCE + Vector3.up * height;
                break;
            default:
                break;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, slideSpeed * Time.deltaTime);

        if (VerifyPosition(targetPosition))
        {
            isMoving = 0;
            GFX_Rotation();
        }
    }
    #endregion Sideways

    #region Rotation
    void GFX_Rotation()
    {
        if (state != PlayerState.PLAYING) return;

        if (isMoving != 0)
        {
            rotationAngleY = isMoving == 1 ? rotationAngleY : -rotationAngleY;
            GFX_transform.Rotate(0, rotationAngleY, 0);
            rotationAngleY = -rotationAngleY;
        }
        else
        {
            var lookForward = Quaternion.Euler(0, 0, 0);
            GFX_transform.rotation = Quaternion.Slerp(GFX_transform.rotation, lookForward, rotateBackSpeed * Time.deltaTime);
        }
        rotationAngleY = Const.PLAYER_ROTATION_MOVE;
    }

    bool VerifyPosition(Vector3 newTargetPosition)
    {
        float targetMinX = newTargetPosition.x - rotateBackDelay;
        float targetMaxX = newTargetPosition.x + rotateBackDelay;

        bool isDirection = isMoving == 1 ? true : false; // true == Right   false == Left

        if (isDirection && transform.position.x >= targetMinX) return true;

        if (!isDirection && transform.position.x <= targetMaxX) return true;

        return false;
    }

    #endregion Rotation

    #region Jump
    void Jump()
    {
        if (input.Movement.Jump.triggered && CheckingGround())
        {
            targetJumpPosition = Vector3.up * jumpHeight;
        }

        if (transform.position.y > 1)
        {
            isJump = true;
        }

        else
        {
            isJump = false;
        }

        transform.Translate(targetJumpPosition * jumpSpeed * Time.deltaTime);

        if (input.Movement.Roll.triggered && !CheckingGround())
        {
            targetJumpPosition = Vector3.zero;
        }

        if (transform.position.y >= jumpHeight && !CheckingGround())
        {
            targetJumpPosition = Vector3.zero;
        }
    }
    #endregion Jump

    #region Roll
    void Roll()
    {
        if (input.Movement.Roll.triggered && !isRolling && CheckingGround())
        {
            isRolling = true;
            StartCoroutine(RollDelay());
        }
    }

    IEnumerator RollDelay()
    {
        Vector3 normalColliderCenter = coll.center;
        Vector3 colliderCenterOnRolling = new Vector3(0, colliderCenter, 0);

        float normalColliderHeight = coll.height;
        float colliderHeightOnRolling = colliderHeight;

        coll.center = Vector3.Lerp(coll.center, colliderCenterOnRolling, 1f);
        coll.height = Mathf.Lerp(coll.height, colliderHeightOnRolling, 1f);


        yield return new WaitForSeconds(rollingDelay);

        coll.height = Mathf.Lerp(coll.height, normalColliderHeight, 1f);
        coll.center = Vector3.Lerp(coll.center, normalColliderCenter, 1f);

        isRolling = false;
    }
    #endregion Roll

    #endregion Movement

    #region Colliders And Raycasts
    private void OnTriggerEnter(Collider other)
    {
        if (!GamePlayManager.Instance.playerColliderOn) return;

        if (other.gameObject.CompareTag(Const.OBSTACLE_TAG))
        {
            StartCoroutine(DieBehaviour());         
        }
    }

    IEnumerator DieBehaviour()
    {
        yield return new WaitForSeconds(0.1f);
        UpdatePlayerState(PlayerState.DEAD);

        GamePlayManager.Instance.UpdateGameState(GameStates.GAMEOVER);
    }

    bool CheckingGround()
    {
        bool ray = Physics.CheckSphere(groundCheck.position, 1.5f, groundMask);

        slideSpeed = jumpSpeed;
        return ray;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(groundCheck.position, 1.5f);
    }

    #endregion Colliders And Raycasts

    #region Win Behaviour

    void OnPlayerWin()
    {
        desiredLane = 1;
        StartCoroutine(WinBehaviour());
    }

    IEnumerator WinBehaviour()
    {
        yield return new WaitForSeconds(0.5f);
        UpdatePlayerState(PlayerState.WIN);
    }

    void WinMovement()
    {
        Debug.LogWarning("MOVIMENTAAAA");
        desiredLane = 1;
        targetPosition = transform.position.x * Vector3.zero + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, targetPosition, slideSpeed * Time.deltaTime);

        if (VerifyPosition(targetPosition))
        {
            isMoving = 0;
            GFX_Rotation();
        }
    }

    void ReachPalaceRotateRotate()
    {
        hasReachPalace = true;       
    }

    void WinRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(lookToCamera), 0.01f);

        if (transform.rotation.y <= lookToCamera.y - 1f)
        {
            Debug.LogWarning("Cai Faixa");
            GameplayEvents.OnDropFaixa();
        }
    }
    #endregion WinBehaviour
}
