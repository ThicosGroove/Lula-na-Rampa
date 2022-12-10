using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

[System.Serializable]
public class ObstacleInfo
{
    public string name;
    public GameObject prefab;
    public float[] posX;
}

[System.Serializable]
public class CollectableInfo
{
    public GameObject collectablePrefab;
    public float[] posX;
}

[DefaultExecutionOrder(10)]
public class SpawnManager : MonoBehaviour
{
    public ObstacleInfo[] obstacles;
    public CollectableInfo collectable;

    [SerializeField] float objSpawnDistance = 1000f;

    private float spawnObstacleDelay;
    private float spawnCollectableDelay;

    private GameObject newObstacle;
    private GameObject newCollectable;

    private float myTimerObstacle, myTimerCollectable;
    private float stopTimerObstacle, stopTimerCollectable;
    private float newtimerObstacle, newTimerCollectable;

    void Start()
    {
        spawnObstacleDelay = LevelManager.Instance.current_obstacleSpawnDelay;
        spawnCollectableDelay = LevelManager.Instance.current_collectableSpawnDelay;

        StartCoroutine(SpawnObstacle());

        myTimerObstacle = spawnObstacleDelay;
        myTimerCollectable = spawnCollectableDelay;
    }

    private void OnEnable()
    {
        GameplayEvents.GameOver += StopAllTheCoroutines;
        GameplayEvents.Win += StopAllTheCoroutines;

        UtilityEvents.GamePause += StopAllTheCoroutines;
        UtilityEvents.GameResume += ResumeAllCoroutines;
    }

    private void OnDisable()
    {
        GameplayEvents.GameOver -= StopAllTheCoroutines;
        GameplayEvents.Win -= StopAllTheCoroutines;

        UtilityEvents.GamePause -= StopAllTheCoroutines;
        UtilityEvents.GameResume -= ResumeAllCoroutines;
    }

    private void Update()
    {
        SetSpawnTimer();

        if (GamePlayManager.Instance.isNormalMode == true) return;
        LevelUp();
    }

    void LevelUp()
    {
        spawnObstacleDelay = LevelManager.Instance.current_obstacleSpawnDelay;
        spawnCollectableDelay = LevelManager.Instance.current_collectableSpawnDelay;
    }

    private void StopAllTheCoroutines()
    {
        StopAllCoroutines();

        stopTimerObstacle = myTimerObstacle;
        newtimerObstacle = spawnObstacleDelay - stopTimerObstacle;

        //stopTimerCollectable = myTimerCollectable;
        //newTimerCollectable = spawnCollectableDelay - stopTimerCollectable;
    }

    private void ResumeAllCoroutines()
    {
        StartCoroutine(SpawnObstacle());
    }

    IEnumerator SpawnObstacle()
    {
        yield return new WaitForSeconds(newtimerObstacle);

        int obstacle = Random.Range(0, obstacles.Length);
        int randomPosObstacleX = Random.Range(0, obstacles[obstacle].posX.Length);
        Vector3 posObstacle = new Vector3(obstacles[obstacle].posX[randomPosObstacleX], 0f, objSpawnDistance);

        int randomPosCollectableX = Random.Range(0, collectable.posX.Length);
        Vector3 posCollectable = new Vector3(collectable.posX[randomPosCollectableX], 0f, objSpawnDistance);


        newObstacle = Instantiate(obstacles[obstacle].prefab, posObstacle, obstacles[obstacle].prefab.transform.rotation);
        newCollectable = Instantiate(collectable.collectablePrefab, posCollectable, Quaternion.identity);

        GamePlayManager.Instance.objList.Add(newObstacle.GetComponent<MoveObstacle>());
        GamePlayManager.Instance.objList.Add(newCollectable.GetComponent<MoveCollectable>());


        yield return new WaitForSecondsRealtime(spawnObstacleDelay);

        StartCoroutine(SpawnObstacle());
    }

    //IEnumerator SpawnCollectable()
    //{
    //    yield return new WaitForSeconds(newtimerObstacle);

    //    int randomPosCollectableX = Random.Range(0, collectable.posX.Length);
    //    Vector3 posCollectable = new Vector3(collectable.posX[randomPosCollectableX], 0f, objSpawnDistance);

    //    newCollectable = Instantiate(collectable.collectablePrefab, posCollectable, Quaternion.identity);
    //    GamePlayManager.Instance.objList.Add(newCollectable.GetComponent<MoveCollectable>());
    //    yield return new WaitForSeconds(spawnCollectableDelay);

    //    StartCoroutine(SpawnCollectable());
    //}


    private void SetSpawnTimer()
    {
        myTimerObstacle -= Time.deltaTime;
        //myTimerCollectable -= Time.deltaTime;

        if (myTimerObstacle <= 0)
        {
            myTimerObstacle = spawnObstacleDelay;
        }

        //if (myTimerCollectable <= 0)
        //{
        //    myTimerCollectable = spawnObstacleDelay;
        //}
    }
}

