using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class MoveObstacle : MonoBehaviour
{
    GameObject player;

    [HideInInspector]
    public float speed;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
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
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        DestroyObstacle();
    }

    void DestroyObstacle()
    {
        if(transform.position.z < player.transform.position.z - 10)
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