using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class CameraBehaviour : MonoBehaviour
{
    Vector3 camPos;

    private void Start()
    {
        camPos = SaveManager.instance.LoadFile()._cameraPosition;
    }

    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += CanGoToPlace;
    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= CanGoToPlace;        
    }

    void CanGoToPlace()
    {        
        GoToPlace(camPos);
    }

    private void GoToPlace(Vector3 newPlace)
    {
        transform.position = newPlace;
    }
}
