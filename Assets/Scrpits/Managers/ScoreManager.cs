using UnityEngine;
using GameEvents;

public class ScoreManager : Singleton<ScoreManager>
{
    public int totalScoreCurrentRun;

    // Start is called before the first frame update
    void Start()
    {
        totalScoreCurrentRun = 0;
    }

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += UpdateScore;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= UpdateScore;       
    }

    void UpdateScore(int score)
    {
        totalScoreCurrentRun += score;
    }

    private void VerifyWin()
    {
        // TEM QUE AJUSTAR O MODO DE JOGO
        // SÓ TEM VITÓRIA NO MODO DE JOGO ESPECÍFICO
        if (totalScoreCurrentRun >= 13)
        {
            // Update GameState WIN
        }
    }
}
