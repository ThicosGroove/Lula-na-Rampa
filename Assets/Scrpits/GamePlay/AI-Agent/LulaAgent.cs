// 24/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class LulaAgent : Agent
{
    private PlayerController playerController;

    public override void Initialize()
    {
        // Referência ao PlayerController
        playerController = GetComponent<PlayerController>();
    }

    public override void OnEpisodeBegin()
    {
        // Reinicia o estado do jogador e o ambiente
        playerController.transform.position = Vector3.zero;
        playerController.UpdatePlayerState(PlayerState.PLAYING);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Coleta informações do ambiente
        sensor.AddObservation(playerController.transform.position.x); // Posição do jogador
        sensor.AddObservation(playerController.CheckingGround() ? 1 : 0); // Está no chão?
        sensor.AddObservation(playerController.desiredLane); // Faixa atual
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        switch (action)
        {
            case 0: // Mover para a esquerda
                if (playerController.desiredLane > 0)
                {
                    playerController.desiredLane--;
                }
                break;
            case 1: // Mover para a direita
                if (playerController.desiredLane < 2)
                {
                    playerController.desiredLane++;
                }
                break;
            case 2: // Pular
                if (playerController.CheckingGround())
                {
                    playerController.Jump();
                }
                break;
            case 3: // Deslizar
                if (playerController.CheckingGround())
                {
                    playerController.Roll();
                }
                break;
        }

        // Atualiza a posição do jogador
        playerController.MoveHandle();
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
    }
}