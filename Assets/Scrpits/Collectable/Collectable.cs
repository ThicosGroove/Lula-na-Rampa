using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class Collectable : MonoBehaviour, ICollectable
{
    public void CollectMe()
    {
        
        Destroy(this.gameObject);
    }

    public void WrongSpawn()
    {
        Destroy(this.gameObject);
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
            Destroy(this.gameObject);
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
