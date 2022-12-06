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

    private float timeLeftSpawnObstacle;
    private float timeLeftSpawnCollectable;

    private void Awake()
    {
    }

    void Start()
    {
        if (GamePlayManager.Instance.isNormalMode == true)
        {
            spawnObstacleDelay = LevelManager.Instance.normalObstacleDelay;
            spawnCollectableDelay = LevelManager.Instance.normalObstacleDelay;
        }
        else
        {
            spawnObstacleDelay = LevelManager.Instance.current_obstacleSpawnDelay;
            spawnCollectableDelay = LevelManager.Instance.current_collectableSpawnDelay;
        }

        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnCollectable());
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
    }

    private void ResumeAllCoroutines()
    {
        //Remaining timer

        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnCollectable());
    }

    IEnumerator SpawnObstacle()
    {
        //sorteia o objeto
        int obstacle = Random.Range(0, obstacles.Length);

        //sorteia a posição entre uma das possíveis para o objeto
        int randomPosX = Random.Range(0, obstacles[obstacle].posX.Length);

        Vector3 pos = new Vector3(obstacles[obstacle].posX[randomPosX], 0f, objSpawnDistance);

        newObstacle = Instantiate(obstacles[obstacle].prefab, pos, obstacles[obstacle].prefab.transform.rotation);

        GamePlayManager.Instance.objList.Add(newObstacle.GetComponent<MoveObstacle>());

        timeLeftSpawnObstacle += Time.time;

        yield return new WaitForSeconds(spawnObstacleDelay);
        StartCoroutine(SpawnObstacle());
    }

    IEnumerator SpawnCollectable()
    {
        timeLeftSpawnCollectable = spawnCollectableDelay;

        int randomPosX = Random.Range(0, collectable.posX.Length);

        Vector3 pos = new Vector3(collectable.posX[randomPosX], 0f, objSpawnDistance);

        newCollectable = Instantiate(collectable.collectablePrefab, pos, Quaternion.identity);
        GamePlayManager.Instance.objList.Add(newCollectable.GetComponent<MoveCollectable>());


        while (timeLeftSpawnCollectable > 0f)
        {

            timeLeftSpawnCollectable -= Time.deltaTime;
            yield return null;
        }

        spawnObstacleDelay = VerifyingTimeLeftSpanwCollectable();

        //yield return new WaitForSeconds(spawnCollectableDelay);
        StartCoroutine(SpawnCollectable());
    }


    private float VerifyingTimeLeftSpanwObstacle()
    {
        if (timeLeftSpawnObstacle != 0) return timeLeftSpawnObstacle;

        return spawnObstacleDelay;
    }

    private float VerifyingTimeLeftSpanwCollectable()
    {
        if (timeLeftSpawnCollectable != 0) return timeLeftSpawnCollectable;

        return spawnCollectableDelay;
    }
}

