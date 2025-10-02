// 24/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using GameEvents;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class LulaAgent : Agent
{
    private PlayerMovement playerMovement;
    private float reward = 0;
    private int osbtacle = 0;

    private Vector3 initialPos;

    [SerializeField] private RampaBehaviour rampa;

    private RayPerceptionSensorComponent3D[] sensor3D;

    public override void Initialize()
    {
        // Referência ao PlayerController
        playerMovement = GetComponent<PlayerMovement>();
        //initialPos = transform.position;
        Time.timeScale = 1.0f;

        sensor3D = GetComponentsInChildren<RayPerceptionSensorComponent3D>();
        //sensor3D = GetComponentInChildren<RayPerceptionSensorComponent3D>();
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Start Episode");
        //transform.position = initialPos;
        //playerMovement.desiredLane = 1;
    }

    private void CheckRaycastPerception()
    {
        // Get the raw raycast results from the sensor component
        var rayOutputs1 = RayPerceptionSensor.Perceive(sensor3D[0].GetRayPerceptionInput()).RayOutputs;

        for (int i = 0; i < rayOutputs1.Length; i++)
        {
            GameObject goHit = rayOutputs1[i].HitGameObject;
            if (goHit != null)
            {
                // Calculate the hit distance based on the normalized fraction
                float rayHitDistance = rayOutputs1[i].HitFraction * sensor3D[0].RayLength;

                // Log detailed information about the hit object
                Debug.Log($"Sensor 1 Ray {i} hit: {goHit.name} at distance: {rayHitDistance:F2} with tag: {goHit.tag}");
            }
            else
            {
                Debug.Log($" Ray {i} did not hit anything.");
            }
        }

        var rayOutputs2 = RayPerceptionSensor.Perceive(sensor3D[1].GetRayPerceptionInput()).RayOutputs;

        for (int i = 0; i < rayOutputs2.Length; i++)
        {
            GameObject goHit = rayOutputs2[i].HitGameObject;
            if (goHit != null)
            {
                // Calculate the hit distance based on the normalized fraction
                float rayHitDistance = rayOutputs2[i].HitFraction * sensor3D[1].RayLength;

                // Log detailed information about the hit object
                Debug.Log($"Sensor 2 Ray {i} hit: {goHit.name} at distance: {rayHitDistance:F2} with tag: {goHit.tag}");
            }
            else
            {
                Debug.Log($"Ray {i} did not hit anything.");
            }
        }
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

        //CheckRaycastPerception();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

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
                break;
            case 3: // Deslizar
                    playerMovement.Roll();
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

            AddReward(-0.2f);
            reward--;
            osbtacle++;
            // Teste para treinar IA

            rampa.RewardLoss();

            //TrainingEvents.OnRewardLoss();
        }
        else if (other.CompareTag(Const.STAR_TAG))
        {

            AddReward(0.3f);
            reward += 0.3f;
            osbtacle++;

            rampa.GetStar();

            //TrainingEvents.OnGetStar();
        }
        else if (other.CompareTag(Const.REWARD_TAG))
        {
            reward += 0.1f;
            AddReward(0.1f);
            osbtacle++;

            rampa.RewardWin();

            //TrainingEvents.OnRewardWin();
        }

        if (osbtacle <= 10)
        {
            osbtacle = 0;
            EndEpisode();
        }

    }
}