using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

[System.Serializable]
public class ObstacleInfo
{
    public string name; // Só pra mudar o nome do elemento e ficar bonito no editor
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
        spawnObstacleCoroutine = SpawnObstacle();
        //StartCoroutine(spawnObstacleCoroutine);

        spawnCollectableCoroutine = SpawnCollectable();
        //StartCoroutine(spawnCollectableCoroutine);

        if (!GamePlayManager.Instance.isNormalMode)
        {
            LevelUp(1);
        }
        else
        {
            LevelUp(3);
        }      
    }

    private void OnEnable()
    {
        ScoreEvents.ChangeLevel += LevelUp;

        GameplayEvents.StartNewLevel += StartNewSpawing;
        GameplayEvents.GameOver += StopAllTheCoroutines;
        GameplayEvents.Win += StopAllTheCoroutines;
    }

    private void OnDisable()
    {
        ScoreEvents.ChangeLevel -= LevelUp;

        GameplayEvents.StartNewLevel -= StartNewSpawing;
        GameplayEvents.GameOver -= StopAllTheCoroutines;
        GameplayEvents.Win -= StopAllTheCoroutines;
    }

    void LevelUp(int newLevel)
    {
        //if (GamePlayManager.Instance.isNormalMode) return;

        StopAllCoroutines();

        currentLevel = newLevel;

        switch (currentLevel)
        {
            case 1:
                currentSpeed = LevelManager.Instance.obstacleSpeed_Level_1;
                spawnObstacleDelay = LevelManager.Instance.obstacleSpawnDelay_Level_1;
                break;
            case 2:
                currentSpeed = LevelManager.Instance.obstacleSpeed_Level_2;
                spawnObstacleDelay = LevelManager.Instance.obstacleSpawnDelay_Level_2;
                break;
            case 3:
                currentSpeed = LevelManager.Instance.obstacleSpeed_Level_3;
                spawnObstacleDelay = LevelManager.Instance.obstacleSpawnDelay_Level_3;
                break;
            case 4:
                currentSpeed = LevelManager.Instance.obstacleSpeed_Level_4;
                spawnObstacleDelay = LevelManager.Instance.obstacleSpawnDelay_Level_4;
                break;
            case 5:
                currentSpeed = LevelManager.Instance.obstacleSpeed_Level_5;
                spawnObstacleDelay = LevelManager.Instance.obstacleSpawnDelay_Level_5;
                break;
            default:
                break;
        }
    }

    private void StartNewSpawing()
    {
        if (GamePlayManager.Instance.isNormalMode)
        {
            currentSpeed = LevelManager.Instance.normalSpeed;
            spawnCollectableDelay = LevelManager.Instance.normalDelay;
        }

        StartCoroutine(spawnObstacleCoroutine);
        StartCoroutine(spawnCollectableCoroutine);
    }

    private void StopAllTheCoroutines()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnObstacleDelay);

            //sorteia o objeto
            int obstacle = Random.Range(0, obstacles.Length);

            //sorteia a posição entre uma das possíveis para o objeto
            int randomPosX = Random.Range(0, obstacles[obstacle].posX.Length);

            Vector3 pos = new Vector3(obstacles[obstacle].posX[randomPosX], 0f, 1000f);

            newObstacle = Instantiate(obstacles[obstacle].prefab, pos, obstacles[obstacle].prefab.transform.rotation);
            GamePlayManager.Instance.objList.Add(newObstacle.GetComponent<MoveObstacle>());
        }
    }

    IEnumerator SpawnCollectable()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCollectableDelay);

            int randomPosX = Random.Range(0, collectable.posX.Length);

            Vector3 pos = new Vector3(collectable.posX[randomPosX], 3f, 200f);

            newCollectable = Instantiate(collectable.collectablePrefab, pos, Quaternion.identity);
            GamePlayManager.Instance.objList.Add(newCollectable.GetComponent<MoveCollectable>());
        }
    }
}

