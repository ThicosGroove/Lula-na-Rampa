using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class Collectable : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Const.PLAYER_TAG))
        {
            ScoreEvents.OnScoreGained(Const.SCORE_PER_COLLECTABLE);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag(Const.OBSTACLE_TAG))
        {
            Destroy(this.gameObject);
        }
    }
}
