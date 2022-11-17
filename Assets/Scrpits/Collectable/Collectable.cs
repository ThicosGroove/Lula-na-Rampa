using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class Collectable : MonoBehaviour, ICollectable
{
    public void CollectMe()
    {
        ScoreEvents.OnScoreGained(Const.SCORE_PER_COLLECTABLE);
        Destroy(this.gameObject);
    }

    public void WrongSpawn()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Const.OBSTACLE_TAG))
        {
            WrongSpawn();
        }

        if (collision.gameObject.CompareTag(Const.PLAYER_TAG))
        {
            CollectMe();
        }
    }
}
