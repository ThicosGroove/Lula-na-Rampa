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

    [Header("Graphics parameters")] // temporário até obj 3D
    [SerializeField] Transform GFX_transform;
    [SerializeField] float GFX_ScaleOnRolling = 0.5f;
    [SerializeField] float GFX_PositionOnRolling = -0.5f;

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
    bool canJump;

    // rotation parameters
    [SerializeField] float rotateBackDelay;
    [SerializeField] float rotateBackSpeed;
    float rotationAngleY = Const.PLAYER_ROTATION_MOVE;
    int isMoving = 0;

    // collider / roll parameters
    CapsuleCollider coll;
    float colliderHeight = 1f;
    float colliderCenter = -0.5f;
    bool isRolling;
    float rollingDelay;

    int currentLevel;

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
        if (GamePlayManager.Instance.isNormalMode == true)
        {
            slideSpeed = LevelManager.Instance.current_playerSlideSpeed;
            jumpSpeed = LevelManager.Instance.current_playerJumpSpeed;
            rollingDelay = LevelManager.Instance.current_playerRollingSpeed;
        }
    }

    private void OnEnable()
    {
        GameplayEvents.Win += OnPlayerWin;

        input.Enable();
    }

    private void OnDisable()
    {
        GameplayEvents.Win -= OnPlayerWin;

        input.Disable();
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
            }

            transform.Translate(targetJumpPosition * jumpSpeed * Time.deltaTime);

            if (transform.position.y >= targetJumpPosition.y)
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
        }
    }
    #endregion Mobile

    void Update()
    {
        if (state != PlayerState.PLAYING || GamePlayManager.Instance.isGamePaused) return;
        MoveInput();
        Jump();
        Roll();

        MoveHandle();
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

        transform.Translate(targetJumpPosition * jumpSpeed * Time.deltaTime);

        if (transform.position.y >= targetJumpPosition.y)
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

        //Temporário
        Vector3 normalGFX_Position = Vector3.zero;
        Vector3 newGXF_Position = new Vector3(0, GFX_PositionOnRolling, 0);

        Vector3 normalGFX_Scale = GFX_transform.localScale;
        Vector3 newGFX_Scale = new Vector3(1, GFX_ScaleOnRolling, 1);

        GFX_transform.localPosition = Vector3.Lerp(normalGFX_Position, newGXF_Position, 1f);
        GFX_transform.localScale = Vector3.Lerp(normalGFX_Scale, newGFX_Scale, 1f);
        //

        yield return new WaitForSeconds(rollingDelay);

        coll.height = Mathf.Lerp(coll.height, normalColliderHeight, 1f);
        coll.center = Vector3.Lerp(coll.center, normalColliderCenter, 1f);

        // Temp
        GFX_transform.localPosition = Vector3.Lerp(newGXF_Position, normalGFX_Position, 1f);
        GFX_transform.localScale = Vector3.Lerp(newGFX_Scale, normalGFX_Scale, 1f);
        //

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
            UpdatePlayerState(PlayerState.DEAD);

            GamePlayManager.Instance.UpdateGameState(GameStates.GAMEOVER);
        }
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

    void OnPlayerWin()
    {
        UpdatePlayerState(PlayerState.WIN);

        // desabilita os inputs
        // diminui a velocidade
        // vai para a lane do meio
    }
}
