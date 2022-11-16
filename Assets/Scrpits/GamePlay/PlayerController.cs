using UnityEngine;
using GameEvents;

enum PlayerState
{
    IDLE,
    PLAYING,
    DEAD,
    WIN
}

// CRIAR UM PLAYER ANIMATION HANDLER P LIDAR COM AS ANIMAÇÕES
public class PlayerController : MonoBehaviour
{
    public float laneDistance;
    public float slideSpeed;

    float height;
    int desiredLane;

    int currentLevel;

    [SerializeField] PlayerState state;

    private void Awake()
    {
        height = transform.localScale.y / 2;
        state = PlayerState.PLAYING;
        desiredLane = 1;
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
        }
    }

    void Update()
    {
        if (state != PlayerState.PLAYING) return;

        Move();
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

        Vector3 targetPosition = transform.position.x * Vector3.zero + new Vector3(0, height, 0);

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, slideSpeed * Time.deltaTime);
    }

    void OnPlayerWin()
    {
        state = PlayerState.WIN;
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
}
