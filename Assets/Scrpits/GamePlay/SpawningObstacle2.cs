using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float spawnObstacleDelay = 1.4f;
    public float spawnCollectableDelay = 3f;

    private GameObject newObstacle;
    private GameObject newCollectable;

    void Start()
    {
        //adicionei um delay no primeiro parâmetro pra não começar já com objeto na cara
        InvokeRepeating("SpawnObstacle", 1f, spawnObstacleDelay); 
        InvokeRepeating("SpawnCollectable", 1f, spawnCollectableDelay); 
    }

    void SpawnObstacle()
    {
        //sorteia o objeto
        int obstacle = Random.Range(0, obstacles.Length);

        //sorteia a posição entre uma das possíveis para o objeto
        int randomPosX = Random.Range(0, obstacles[obstacle].posX.Length);

        Vector3 pos = new Vector3(obstacles[obstacle].posX[randomPosX], 0f, 100f);

        newObstacle = Instantiate(obstacles[obstacle].prefab, pos, obstacles[obstacle].prefab.transform.rotation);
    }

    void SpawnCollectable()
    {
        int randomPosX = Random.Range(0, collectable.posX.Length);

        Vector3 pos = new Vector3(collectable.posX[randomPosX], 0f, 100f);

        newCollectable = Instantiate(collectable.collectablePrefab, pos, Quaternion.identity);
    }
}

