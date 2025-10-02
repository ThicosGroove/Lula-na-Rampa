using UnityEngine;
using GameEvents;
using PlayFab.ClientModels;

public class PlayerGameBehaviour : MonoBehaviour
{

    [SerializeField] private int reward = 0;
    [SerializeField] private int stars = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.OBSTACLE_TAG))
        {
            // Game Over

            // Teste para treinar IA
            reward--;

        }
        else if (other.CompareTag(Const.REWARD_TAG))
        {
            reward++;
            //ScoreEvents.OnScoreGained(Const.SCORE_PER_COLLECTABLE);
        }
        else if (other.CompareTag(Const.STAR_TAG))
        {
            stars++;
            //ScoreEvents.OnScoreGained(Const.SCORE_PER_COLLECTABLE);
        }
    }
}
