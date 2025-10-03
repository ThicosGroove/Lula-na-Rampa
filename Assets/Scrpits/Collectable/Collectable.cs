using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class Collectable : MonoBehaviour, ICollectable
{
    public void CollectMe()
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.Star);
    }

    public void WrongSpawn()
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.Star);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(Const.OBSTACLE_TAG))
        {
            WrongSpawn();
        }

        if (collision.gameObject.CompareTag(Const.PLAYER_TAG))
        {
            CollectMe();

            ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.Star);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag(Const.OBSTACLE_TAG))
        {
            WrongSpawn();
        }
    }
}
