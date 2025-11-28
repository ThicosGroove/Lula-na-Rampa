using UnityEngine;
using GameEvents;
using PlayFab.ClientModels;

public class PlayerGameBehaviour : MonoBehaviour
{

    [SerializeField] private int reward = 0;
    [SerializeField] private int stars = 0;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.OBSTACLE_TAG))
        {
            reward--;
            // Game Over
            
            if (GamePlayManager.Instance.isTraining) { return; }

            GameplayEvents.OnGameOver();
            

            // Teste para treinar IA

        }
        else if (other.CompareTag(Const.REWARD_TAG))
        {
            reward++;
            //ScoreEvents.OnScoreGained(Const.SCORE_PER_COLLECTABLE);
        }
        else if (other.CompareTag(Const.STAR_TAG))
        {
            stars++;
            if (GamePlayManager.Instance.isTraining) { return; }
            
            ScoreEvents.OnScoreGained(Const.SCORE_PER_COLLECTABLE);
            
        }
    }
}
