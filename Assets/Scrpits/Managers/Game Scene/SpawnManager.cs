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
    [Header("Settings")]
    public ObstacleInfo[] obstacles;
    public CollectableInfo collectable;

    [Header("Debug View (Read Only)")]
    [SerializeField] private float currentSpawnDistance; // Apenas para você ver no Inspector se está funcionando

    private float distanceAccumulator = 0f;
    private float currentWorldSpeed;
    private Vector3 thisPos;
    private bool isGameActive = true;

    void Start()
    {
        thisPos = GetComponent<Transform>().position;
        distanceAccumulator = 0f;
    }

    private void OnEnable()
    {
        GameplayEvents.GameOver += StopSpawning;
        GameplayEvents.Win += StopSpawning;
        UtilityEvents.GamePause += PauseSpawning;
        UtilityEvents.GameResume += ResumeSpawning;
    }

    private void OnDisable()
    {
        GameplayEvents.GameOver -= StopSpawning;
        GameplayEvents.Win -= StopSpawning;
        UtilityEvents.GamePause -= StopSpawning;
        UtilityEvents.GameResume -= ResumeSpawning;
    }

    private void Update()
    {
        if (!isGameActive) return;
        if (LevelManager.Instance == null) return;

        // 1. Ler Velocidade do Manager
        currentWorldSpeed = LevelManager.Instance.current_obstacleSpeed;

        if (currentWorldSpeed <= 0) return;

        // 2. Ler Distância do Manager (A Fonte da Verdade é o SO via Manager)
        currentSpawnDistance = LevelManager.Instance.current_obstacleSpawnDistance;

        // Segurança: Evita spawn infinito se você esquecer de configurar o SO (distância 0)
        if (currentSpawnDistance <= 5f) currentSpawnDistance = 20f;

        // 3. Acumular e Spawnar
        distanceAccumulator += currentWorldSpeed * Time.deltaTime;

        if (distanceAccumulator >= currentSpawnDistance)
        {
            SpawnLogic();
            distanceAccumulator -= currentSpawnDistance;
        }
    }

    private void SpawnLogic()
    {
        if (obstacles.Length == 0) return;

        int obstacleIndex = Random.Range(0, obstacles.Length);

        if (obstacles[obstacleIndex].posX.Length == 0) return;

        int randomPosObstacleX = Random.Range(0, obstacles[obstacleIndex].posX.Length);
        Vector3 posObstacle = new Vector3(obstacles[obstacleIndex].posX[randomPosObstacleX] + thisPos.x, 0f, thisPos.z);

        // Lógica do Coletável
        if (collectable.posX.Length > 0)
        {
            int randomPosCollectableX = Random.Range(0, collectable.posX.Length);
            Vector3 posCollectable = new Vector3(collectable.posX[randomPosCollectableX] + thisPos.x, 0f, thisPos.z);

            GameObject newCollectable = ObjectPoolManager.SpawnObject(
                collectable.collectablePrefab,
                posCollectable,
                Quaternion.identity,
                ObjectPoolManager.PoolType.Star
            );
            if (newCollectable != null)
                newCollectable.transform.SetParent(this.transform);
        }

        // Spawn do Obstáculo
        GameObject newObstacle = ObjectPoolManager.SpawnObject(
            obstacles[obstacleIndex].prefab,
            posObstacle,
            obstacles[obstacleIndex].prefab.transform.rotation,
            ObjectPoolManager.PoolType.Obstacle
        );

        if (newObstacle != null)
            newObstacle.transform.SetParent(this.transform);
    }

    private void StopSpawning() { isGameActive = false; }
    private void PauseSpawning() { isGameActive = false; }
    private void ResumeSpawning() { isGameActive = true; }
}