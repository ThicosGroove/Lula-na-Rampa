using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class CameraBehaviour : MonoBehaviour
{
    Vector3 camPos;
    Vector3 camRot;

    Vector3 receiveFaixaPos = new Vector3(0f, 13f, -30f);
    Vector3 receiveFaixaRot = new Vector3(0f, 1f, 0f);

    private void Start()
    {
        camPos = SaveManager.instance.LoadFile()._cameraPosition;
        camRot = SaveManager.instance.LoadFile()._cameraRotation;

        Debug.LogWarning($"Cam Pos {camPos}  Cam Rot {camRot}");
    }

    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += CanGoToPlace;
        GameplayEvents.ReachPalace += CanReceiveFaixa;
    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= CanGoToPlace;        
        GameplayEvents.ReachPalace -= CanReceiveFaixa;
    }

    void CanGoToPlace()
    {
        Debug.LogWarning("Go To place camera");
        GoToPlace(camPos, camRot);
    }

    void CanReceiveFaixa()
    {
        Debug.LogWarning("Go To place camera Receive Faixa");
        GoToPlace(receiveFaixaPos, receiveFaixaRot);
    }
    private void GoToPlace(Vector3 pos , Vector3 rot)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
        Debug.LogWarning("Chegou");
    }




}
