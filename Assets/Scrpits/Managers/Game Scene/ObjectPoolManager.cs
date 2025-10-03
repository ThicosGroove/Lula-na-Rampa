//// 02/10/2025 AI-Tag
//// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private bool _addToDontDestroyOnLoad = false;

    private GameObject _emptyHolder;

    private static GameObject _obstacleEmpty;
    private static GameObject _starEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        Obstacle,
        Star
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _emptyHolder = new GameObject("Object Pools");

        _obstacleEmpty = new GameObject("Obstacle Pool");
        _obstacleEmpty.transform.SetParent(_emptyHolder.transform);

        _starEmpty = new GameObject("Star Pool");
        _starEmpty.transform.SetParent(_emptyHolder.transform);

        if (_addToDontDestroyOnLoad)
            DontDestroyOnLoad(_obstacleEmpty.transform.root);


    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.Obstacle)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
            );

        _objectPools.Add(prefab, pool);
    }

    private static void CreatePool(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.Obstacle)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, parent, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
            );

        _objectPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.Obstacle)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, pos, rot);

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    private static GameObject CreateObject(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.Obstacle)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, parent);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = rot;
        obj.transform.localScale = Vector3.one;

        prefab.SetActive(true);


        return obj;
    }

    private static void OnGetObject(GameObject obj)
    {
        // Logic for when getting the obj from pool
    }

    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private static void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }  
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Obstacle:
                return _obstacleEmpty;
            case PoolType.Star:
                return _starEmpty;
            default:
                return null;
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.Obstacle) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (!GamePlayManager.Instance.objList.Contains(obj))
            {
                GamePlayManager.Instance.objList.Add(obj);
            }

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesnt have component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }

    public  static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.Obstacle) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.Obstacle)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }

    public static T SpawnObject<T>(T typePrefab, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.Obstacle) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, parent, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.Obstacle)
    {
        return SpawnObject<GameObject>(objectToSpawn, parent, spawnRotation, poolType);
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.Obstacle) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, parent, spawnRotation, poolType);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = spawnRotation;
            obj.SetActive(true);

            if (!GamePlayManager.Instance.objList.Contains(obj))
            {
                GamePlayManager.Instance.objList.Add(obj);
            }

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesnt have component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }


    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.Obstacle)
    {
        if (!obj.activeSelf)
        {
            Debug.LogWarning($"Tentando retornar um objeto já desativado: {obj.name}");
            return;
        }

        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);

            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return an object that is not pooled: " + obj.name);
        }

    }
}