using GameEvents;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LulaAgent : Agent
{
    [Header("References")]
    private PlayerMovement playerMovement;
    private PlayerInputHandler playerInputHandler;
    private PlayerManager playerManager;
    [SerializeField] private RampaBehaviour rampa;

    [Header("Training Settings")]
    private int obstaclesDodged = 0;
    private const int TARGET_OBSTACLES_TO_WIN = 20; // Meta para completar o episódio com sucesso

    // --- Configuração de Recompensas ---
    // Punições
    private float r_hitObstacle = -5.0f;      // Bater é catastrófico
    private float r_fallDeath = -5.0f;        // Cair é catastrófico
    // Incentivos
    private float r_winTraining = 10.0f;       // Grande prêmio por vencer
    private float r_dodgeObstacle = +1.0f;    // Pequeno incentivo por progresso
    private float r_getStar = +1.0f;          // Incentivo secundário (opcional)
    private float r_survival = +0.0001f;       // Recompensa constante por se manter vivo

    // --- Custos de Energia (Hierarquia de Movimento) ---
    // Incentiva a IA a fazer o movimento "mais barato" possível para resolver o problema
    private float cost_move_side = -0.001f;  // Barato
    private float cost_action_heavy = -0.01f;// Caro (Pular/Rolar)

    public override void Initialize()
    {
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    public override void OnEpisodeBegin()
    {
        // Apenas reseta o contador de progresso da IA.
        // O reset de posição/física deve ser tratado pelo seu GameManager/PlayerMovement
        // quando ele detectar que o jogo reiniciou.
        obstaclesDodged = 0;

        // --- CURRICULUM LEARNING ---
        // Pergunta ao Python: "Qual a dificuldade atual?"
        // Se não estiver treinando (valor padrão), usa 0.
        float difficulty = Academy.Instance.EnvironmentParameters.GetWithDefault("level_difficulty", 0.0f);

        // Aplica a dificuldade (converte float para int)
        SetLevelDifficulty((int)difficulty);
    }

    private void SetLevelDifficulty(int levelIndex)
    {
        // Só aplica se o nível mudar, para não spamar eventos
        if (LevelManager.Instance.currentLevelIndex != levelIndex)
        {
            // Converte o índice int de volta para o Enum (opcional, ou passa int direto)
            CurrentLevelState newLevel = (CurrentLevelState)levelIndex;
            LevelManager.Instance.UpdateLevel(newLevel);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Total Observations: 5
        // Certifique-se que no Inspector: Vector Observation > Space Size = 5

        // 1. Posição do Jogador (3 floats: x, y, z)
        sensor.AddObservation(transform.position);

        // 2. Está no chão? (1 float)
        sensor.AddObservation(playerMovement.isGrounded ? 1 : 0);

        // 3. Em qual faixa estou? (1 float)
        sensor.AddObservation(playerMovement.desiredLane);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Se o input do jogador estiver travado (jogo pausado ou não iniciado), ignora.
        //if (!playerInputHandler.canMove) { return; }

        int action = actions.DiscreteActions[0];
        float currentActionCost = 0f;

        switch (action)
        {
            case 0: // Esquerda
                playerMovement.MoveToLane(-1);
                currentActionCost = cost_move_side;
                break;

            case 1: // Direita
                playerMovement.MoveToLane(+1);
                currentActionCost = cost_move_side;
                break;

            case 2: // Pular
                playerMovement.Jump();
                currentActionCost = cost_action_heavy;
                break;

            case 3: // Deslizar
                playerMovement.Roll();
                currentActionCost = cost_action_heavy;
                break;

            case 4: // Idle (Não fazer nada)
                currentActionCost = 0f; // Grátis (Melhor opção se não houver perigo)
                break;
        }

        // Aplica o custo da ação (penalidade leve) para incentivar a precisão
        AddReward(currentActionCost);
    }

    private void FixedUpdate()
    {
        // Recompensa de Sobrevivência
        // Incentiva o agente a querer continuar jogando.
        // Valor (+0.005) é maior que o custo de pular (-0.002), então ele pulará para sobreviver.
        if (playerInputHandler.canMove)
        {
            AddReward(r_survival);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.OBSTACLE_TAG))
        {
            // Game Over (Batida)
            AddReward(r_hitObstacle);

            if (GamePlayManager.Instance.isTraining) { rampa.RewardLoss(); }

            EndEpisode();
        }
        else if (other.CompareTag("DeathBarrier"))
        {
            // Game Over (Queda no Vazio)
            // Precisamos punir a queda para ele aprender a não se jogar da rampa
            AddReward(r_fallDeath);

            // Avisa o ML-Agents que acabou.
            // O seu script externo de controle deve detectar essa colisão também 
            // e resetar a posição do boneco.
            EndEpisode();
        }
        else if (other.CompareTag(Const.STAR_TAG))
        {
            // Bônus
            AddReward(r_getStar);
            if (GamePlayManager.Instance.isTraining) { rampa.GetStar(); }
        }
        else if (other.CompareTag(Const.REWARD_TAG))
        {
            // Sucesso (Desviou)
            AddReward(r_dodgeObstacle);
            obstaclesDodged++;
        }

        // Condição de Vitória (Curriculum Learning)
        if (obstaclesDodged >= TARGET_OBSTACLES_TO_WIN)
        {
            AddReward(r_winTraining);
            EndEpisode();
        }
    }

    // Controles Manuais para Teste (Debugging)
    // Ative "Heuristic Only" no Behavior Parameters para usar
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 4; // Padrão: Nada

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            discreteActions[0] = 0;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            discreteActions[0] = 1;
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            discreteActions[0] = 2;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            discreteActions[0] = 3;
    }
}