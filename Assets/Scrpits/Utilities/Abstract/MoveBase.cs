using UnityEngine;
using GameEvents;
using UnityEngine.Audio;

public abstract class MoveBase : MonoBehaviour
{
    GameObject player;

    protected float minDist = 200f;
    protected float speed;

    protected bool isInReach = false;

    private float previousSpeed;
    private bool hasReach;

    //AudioSource audioSource;

    protected virtual void Start()
    {
        player = FindFirstObjectByType<PlayerManager>().gameObject;
        speed = LevelManager.Instance.current_obstacleSpeed;

        //audioSource = GetComponent<AudioSource>();
        //audioSource.gameObject.SetActive(false);

        hasReach = GamePlayManager.Instance.hasReach;
    }

    private void OnEnable()
    {
        ScoreEvents.ChangeLevel += DestroyOnNewLevel;

        GameplayEvents.GameOver += DestroyOnGameOver;
        GameplayEvents.Win += DestroyOnGameOver;

        UtilityEvents.GamePause += StopMovement;
        UtilityEvents.GameResume += ResumeMovement;
    }

    private void OnDisable()
    {
        ScoreEvents.ChangeLevel -= DestroyOnNewLevel;

        GameplayEvents.GameOver -= DestroyOnGameOver;
        GameplayEvents.Win -= DestroyOnGameOver;

        UtilityEvents.GamePause -= StopMovement;
        UtilityEvents.GameResume -= ResumeMovement;
    }

    void Update()
    {
        BasicMovement();
        UpdateSpeed();
        ReachSlowDownPoint();
        MoveBehaviour();
        ReturnToPoolOnLeaveScreen();
    }

    void BasicMovement()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    void UpdateSpeed()
    {
        speed = LevelManager.Instance.current_obstacleSpeed;
    }

    void ReachSlowDownPoint()
    {
        if (transform.position.z < minDist)
        {
            isInReach = true;
            speed = LevelManager.Instance.current_obstacleSpeed;
        }
    }

    protected abstract void MoveBehaviour();
    protected abstract void DieBehaviour();

    void ReturnToPoolOnLeaveScreen()
    {
        if (transform.position.z < player.transform.position.z - 50f)
        {
            ReturnToPool();
        }
    }

    void DestroyOnNewLevel(int _)
    {
        //Debug.LogWarning("Nao destrói");
    }

    void DestroyOnGameOver()
    {
        ReturnToPool();
    }

    void StopMovement()
    {
        previousSpeed = speed;
        speed = 0;
    }

    void ResumeMovement()
    {
        speed = previousSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Const.PLAYER_TAG))
        {
            //Play Audio
            ReturnToPool();
        }

        else if (this.CompareTag(Const.STAR_TAG) && other.CompareTag(Const.OBSTACLE_TAG))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (this.CompareTag(Const.STAR_TAG))
        {
            ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.Star);
        }
        else if (this.CompareTag(Const.OBSTACLE_TAG))
        {
            ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.Obstacle);
        }
    }
}
