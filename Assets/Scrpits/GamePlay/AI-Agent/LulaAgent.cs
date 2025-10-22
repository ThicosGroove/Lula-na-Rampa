// 24/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using GameEvents;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LulaAgent : Agent
{
    private PlayerMovement playerMovement;
    private int osbtacle = 0;
    private int obstaclesDodged = 0; // Contador de obstáculos desviados
    private float totalReward = 0f; // Acumula o reward total durante o episódio

    private PlayerInputHandler playerInputHandler;
    private PlayerManager playerManager;

    [Header("Training")]
    [SerializeField] private RampaBehaviour rampa;

    private float r_hitObstacle = -3f;
    private float r_dodgeObstacle = +0.1f;
    private float r_getStar = +0.5f;
    private float r_keepLaneOne = +0.005f;
    private float r_movement = -0.05f;
    private float r_jumpWrong = -0.2f;
    private bool isFaixa = false;


    public override void Initialize()
    {
        // Referência ao PlayerController
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        //initialPos = transform.position;
        Time.timeScale = 1.0f;
    }

    private void FixedUpdate()
    {
        BetterAgentBehaviourReward();
    }

    public override void OnEpisodeBegin()
    {
        osbtacle = 0;
        obstaclesDodged = 0;
        totalReward = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 playerPosition = playerMovement.transform.position;
        int isGrounded = playerMovement.isGrounded ? 1 : 0;
        int currentLane = playerMovement.desiredLane;

        // Coleta informações do ambiente
        sensor.AddObservation(playerPosition); // Posição do jogador
        sensor.AddObservation(isGrounded); // Está no chão?
        sensor.AddObservation(currentLane); // Faixa atual

        CheckRaycastPerception();
    }



    private void CheckRaycastPerception()
    {
        int faixaLayerIndex = LayerMask.NameToLayer(Const.LAYER_FAIXA);
        int faixaLayerMask = 1 << faixaLayerIndex;

        // Obtém todos os sensores RayPerceptionSensorComponent3D nos filhos do agente
        RayPerceptionSensorComponent3D[] sensors = GetComponentsInChildren<RayPerceptionSensorComponent3D>();

        foreach (var sensor in sensors)
        {
            // Avalia os raios lançados pelo sensor
            var rayInput = sensor.GetRayPerceptionInput();
            var rayOutput = RayPerceptionSensor.Perceive(rayInput);

            // Itera sobre os resultados dos raios
            foreach (var rayResult in rayOutput.RayOutputs)
            {
                if (rayResult.HasHit)
                {
                    // Verifica se o objeto atingido está na layer desejada
                    if (((1 << rayResult.HitGameObject.layer) & faixaLayerMask) != 0)
                    {
                        isFaixa = true;
                        Debug.Log(isFaixa);

                        Debug.Log($"Objeto detectado na layer: {LayerMask.LayerToName(rayResult.HitGameObject.layer)} - Objeto: {rayResult.HitGameObject.name}");
                    }
                    else
                    {
                        isFaixa = false;
                        Debug.Log(isFaixa);
                    }
                }
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        if (!playerInputHandler.canMove) { return; }

        switch (action)
        {
            case 0: // Mover para a esquerda
                    playerMovement.MoveToLane(-1);
                break;
            case 1: // Mover para a direita               
                    playerMovement.MoveToLane(+1);
                break;
            case 2: // Pular
                    playerMovement.Jump();
                    AddReward(r_movement);
                    if (isFaixa) { AddReward(r_jumpWrong); }  
                break;
            case 3: // Deslizar
                    playerMovement.Roll();
                    AddReward(r_movement);
                break;
            case 4:
                // Nao se mover
                break;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Controle manual para testes
        var discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.LeftArrow))
            discreteActions[0] = 0; // Esquerda
        else if (Input.GetKey(KeyCode.RightArrow))
            discreteActions[0] = 1; // Direita
        else if (Input.GetKey(KeyCode.UpArrow))
            discreteActions[0] = 2; // Pular
        else if (Input.GetKey(KeyCode.DownArrow))
            discreteActions[0] = 3; // Deslizar
        else
            discreteActions[0] = 4; // nao fazer nada
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.OBSTACLE_TAG))
        {
            // Game Over

            AddReward(r_hitObstacle);
            osbtacle++;
            // Teste para treinar IA

            if (playerManager.isTraining) { rampa.RewardLoss(); }

            //TrainingEvents.OnRewardLoss();
        }
        else if (other.CompareTag(Const.STAR_TAG))
        {

            AddReward(r_getStar);
            osbtacle++;

            if (playerManager.isTraining) { rampa.GetStar(); }
            

            //TrainingEvents.OnGetStar();
        }
        else if (other.CompareTag(Const.REWARD_TAG))
        {
            AddReward(r_dodgeObstacle);
            obstaclesDodged++;
            osbtacle++;

            if (playerManager.isTraining) { rampa.RewardWin(); }

            //TrainingEvents.OnRewardWin();
        }

        if (osbtacle >= 10)
        {
            float multiplier = 1f + (obstaclesDodged * 0.1f); // Exemplo: 10% de bônus por obstáculo desviado
            float finalReward = totalReward * multiplier;
            AddReward(finalReward);
            EndEpisode();
        }
    }

    private void BetterAgentBehaviourReward()
    {
        if (playerMovement.desiredLane == 1)
        {
            AddReward(r_keepLaneOne * Time.fixedDeltaTime);
        }
    }
}