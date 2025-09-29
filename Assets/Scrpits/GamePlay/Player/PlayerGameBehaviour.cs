using UnityEngine;
using GameEvents;
using PlayFab.ClientModels;

public class PlayerGameBehaviour : MonoBehaviour
{

    [SerializeField] private int reward = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.OBSTACLE_TAG))
        {
            // Game Over

            Debug.Log("BATEU");

            // Teste para treinar IA
            reward--;

        }
        else if (other.CompareTag(Const.STAR_TAG))
        {
            reward += 10;
            //ScoreEvents.OnScoreGained(Const.SCORE_PER_COLLECTABLE);
        }

    }
}
