using UnityEngine;
using GameEvents;

public abstract class MoveBase : MonoBehaviour
{
    GameObject player;

    protected float minDist = 200f;
    protected float speed;

    protected bool isInReach = false;

    protected virtual void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        speed = LevelManager.Instance.current_obstacleInitialSpeed;
    }

    private void OnEnable()
    {
        ScoreEvents.ChangeLevel += DestroyOnNewLevel;

        GameplayEvents.GameOver += DestroyOnGameOver;
        GameplayEvents.Win += DestroyOnGameOver;
    }

    private void OnDisable()
    {
        ScoreEvents.ChangeLevel -= DestroyOnNewLevel;

        GameplayEvents.GameOver -= DestroyOnGameOver;
        GameplayEvents.Win -= DestroyOnGameOver;
    }

    void Update()
    {
        BasicMovement();
        ReachSlowDownPoint();
        MoveBehaviour();
        DestroyObjOnLeaveScreen();
    }

    void BasicMovement()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

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

    void DestroyObjOnLeaveScreen()
    {
        if (transform.position.z < player.transform.position.z - 50f)
        {
            GamePlayManager.Instance.objList.Remove(this);
            Destroy(this.gameObject);
        }
    }

    void DestroyOnNewLevel(int _)
    {
        //Debug.LogWarning("Nao destrói");
        //GamePlayManager.Instance.objList.Remove(this);
        //Destroy(this.gameObject);
    }

    void DestroyOnGameOver()
    {
        GamePlayManager.Instance.objList.Remove(this);
        Destroy(this.gameObject);
    }
}
