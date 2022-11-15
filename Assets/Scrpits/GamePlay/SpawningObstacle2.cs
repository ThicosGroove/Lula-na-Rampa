using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

[System.Serializable]
public class ObstacleInfo
{
    public string name; // S� pra mudar o nome do elemento e ficar bonito no editor
    public GameObject prefab;
    public float[] posX;
}

[System.Serializable]
public class CollectableInfo
{
    public GameObject collectablePrefab;
    public float[] posX;
}

public class SpawningObstacle2 : MonoBehaviour
{
    public ObstacleInfo[] obstacles;
    public CollectableInfo collectable;

    private int currentLevel;
    private float currentSpeed;
    private float spawnObstacleDelay;
    private float spawnCollectableDelay = 3f;

    private GameObject newObstacle;
    private GameObject newCollectable;

    private IEnumerator spawnObstacleCoroutine;
    private IEnumerator spawnCollectableCoroutine;


    void Start()
    {
        LevelUp(1);

        spawnObstacleCoroutine = SpawnObstacle();
        StartCoroutine(spawnObstacleCoroutine);

        spawnCollectableCoroutine = SpawnCollectable();
        StartCoroutine(spawnCollectableCoroutine);
    }

    private void OnEnable()
    {
        ScoreEvents.ChangeLevel += LevelUp;
        GameplayEvents.StartNewLevel += StartNewSpawing;
    }

    private void OnDisable()
    {
        ScoreEvents.ChangeLevel -= LevelUp;
        GameplayEvents.StartNewLevel -= StartNewSpawing;
    }

    void LevelUp(int newLevel)
    {
        StopAllCoroutines();

        currentLevel = newLevel;

        switch (currentLevel)
        {
            case 1:
                currentSpeed = GamePlayManager.Instance.obstacleSpeed_Level_1;
                spawnObstacleDelay = GamePlayManager.Instance.obstacleDelay_Level_1;
                break;
            case 2:
                currentSpeed = GamePlayManager.Instance.obstacleSpeed_Level_2;
                spawnObstacleDelay = GamePlayManager.Instance.obstacleDelay_Level_2;
                break;
            case 3:
                currentSpeed = GamePlayManager.Instance.obstacleSpeed_Level_3;
                spawnObstacleDelay = GamePlayManager.Instance.obstacleDelay_Level_3;
                break;
            case 4:
                currentSpeed = GamePlayManager.Instance.obstacleSpeed_Level_4;
                spawnObstacleDelay = GamePlayManager.Instance.obstacleDelay_Level_4;
                break;
            case 5:
                currentSpeed = GamePlayManager.Instance.obstacleSpeed_Level_5;
                spawnObstacleDelay = GamePlayManager.Instance.obstacleDelay_Level_5;
                break;
            default:
                break;
        }
    }

    private void StartNewSpawing()
    {
        StartCoroutine(spawnObstacleCoroutine);
        StartCoroutine(spawnCollectableCoroutine);
    }

    IEnumerator SpawnObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnObstacleDelay);

            //sorteia o objeto
            int obstacle = Random.Range(0, obstacles.Length);

            //sorteia a posi��o entre uma das poss�veis para o objeto
            int randomPosX = Random.Range(0, obstacles[obstacle].posX.Length);

            Vector3 pos = new Vector3(obstacles[obstacle].posX[randomPosX], 0f, 200f);

            newObstacle = Instantiate(obstacles[obstacle].prefab, pos, obstacles[obstacle].prefab.transform.rotation);
            GamePlayManager.Instance.objList.Add(newObstacle.GetComponent<MoveObstacle>());
            newObstacle.GetComponent<MoveObstacle>().speed = currentSpeed;
        }
    }

    IEnumerator SpawnCollectable()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCollectableDelay);

            int randomPosX = Random.Range(0, collectable.posX.Length);

            Vector3 pos = new Vector3(collectable.posX[randomPosX], 0f, 200f);

            newCollectable = Instantiate(collectable.collectablePrefab, pos, Quaternion.identity);
            GamePlayManager.Instance.objList.Add(newCollectable.GetComponent<MoveObstacle>());
            newCollectable.GetComponent<MoveObstacle>().speed = currentSpeed;
        }  
    }
}

