// 24/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class LulaAgent : Agent
{
    private PlayerMovement playerMovement;
    private float reward = 0;

    public override void Initialize()
    {
        // Referência ao PlayerController
        playerMovement = GetComponent<PlayerMovement>();
        Time.timeScale = 1.0f;
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Start Episode");
        transform.position = Vector3.zero;
        playerMovement.desiredLane = 1;
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
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        Debug.Log($"Action Received: {action}");
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
        //else
        //    discreteActions[0] = 4; // nao fazer nada
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.OBSTACLE_TAG))
        {
            // Game Over

            Debug.Log("BATEU");

            AddReward(-0.1f);
            reward--;
            // Teste para treinar IA
        }
        else if (other.CompareTag(Const.STAR_TAG))
        {

            AddReward(0.3f);
            reward += 0.1f;
        }

        if (reward < 0)
        {
            EndEpisode();
        }

    }
}