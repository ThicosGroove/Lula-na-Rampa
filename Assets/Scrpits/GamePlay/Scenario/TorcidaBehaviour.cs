using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class TorcidaBehaviour : MonoBehaviour
{
    Camera mainCamera;
 
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(mainCamera.transform);     
    }    
}
