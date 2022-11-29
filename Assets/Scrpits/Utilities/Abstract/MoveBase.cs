using UnityEngine;
using GameEvents;

public abstract class MoveBase : MonoBehaviour
{
    GameObject player;

    [SerializeField] float Initialspeed;

    float speed;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        speed = Initialspeed;
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
        if (transform.position.z < 200f)
        {
            speed = GamePlayManager.Instance.no 
        }
    }

    protected abstract void MoveBehaviour();

    void DestroyObjOnLeaveScreen()
    {
        if (transform.position.z < player.transform.position.z - 10)
        {
            GamePlayManager.Instance.objList.Remove(this);
            Destroy(this.gameObject);
        }
    }

    void DestroyOnNewLevel(int _)
    {
        GamePlayManager.Instance.objList.Remove(this);
        Destroy(this.gameObject);
    }

    void DestroyOnGameOver()
    {
        GamePlayManager.Instance.objList.Remove(this);
        Destroy(this.gameObject);
    }
}
