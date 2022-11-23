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
    [SerializeField] float slideSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpSpeed;
    [SerializeField] float rollingDelay;

    [Header("Ground Check parameters")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    [Header("Graphics parameters")] // temporário até obj 3D
    [SerializeField] Transform GFX_transform;  
    [SerializeField] float GFX_ScaleOnRolling = 0.5f;
    [SerializeField] float GFX_PositionOnRolling = -0.5f;
    
    // PRIVATES PARAMETERS
    // position parameters
    float height; 
    int desiredLane;
    Vector3 targetPosition;
    Vector3 targetJumpPosition;

    // collider / roll parameters
    CapsuleCollider coll;
    float colliderHeight = 1f;
    float colliderCenter = -0.5f;
    bool isRolling;
    IEnumerator Rolling;

    int currentLevel;

    #region SetUp 
    private void Awake()
    {
        height = 3;
        state = PlayerState.PLAYING;
        desiredLane = Const.PLAYER_INITIAL_LANE;

        Rolling = RollDelay();
        coll = GetComponent<CapsuleCollider>();
        coll.isTrigger = true;
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
    #endregion SetUp

    #region Movement
    void Update()
    {
        if (state != PlayerState.PLAYING) return;
        Move();
        Jump();
        Roll();
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
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && CheckingGround())
        {
            targetJumpPosition = Vector3.up * jumpHeight;
            //StopCoroutine(Rolling);
            //isRolling = false;
        }

        transform.Translate(targetJumpPosition * jumpSpeed * Time.deltaTime);

        if (transform.position.y >= targetJumpPosition.y)
        {
            targetJumpPosition = Vector3.zero;
        }
    }

    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isRolling && CheckingGround())
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

        Vector3 normalGFX_Scale = Vector3.one;
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
    #endregion Movement

    #region Colliders And Raycasts
    private void OnTriggerEnter(Collider other)
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
    #endregion Colliders And Raycasts

}
