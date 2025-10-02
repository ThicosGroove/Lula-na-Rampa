// 02/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public void CreatePool(string poolKey, GameObject prefab, int initialSize)
    {
        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary[poolKey] = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                poolDictionary[poolKey].Enqueue(obj);
            }
        }
    }

    public GameObject GetFromPool(string poolKey, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(poolKey) && poolDictionary[poolKey].Count > 0)
        {
            GameObject obj = poolDictionary[poolKey].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        Debug.LogWarning($"Pool with key {poolKey} is empty or does not exist!");
        return null;
    }

    public void ReturnToPool(string poolKey, GameObject obj)
    {
        obj.SetActive(false);
        if (poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary[poolKey].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"Pool with key {poolKey} does not exist!");
        }
    }
}