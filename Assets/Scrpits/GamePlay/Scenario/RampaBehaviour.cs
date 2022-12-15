using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class RampaBehaviour : MonoBehaviour
{
	public float vel = 0.1f;
	public Renderer quad;
	bool canMove = false;

    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += StartMoving;
        GameplayEvents.Win += WinMovement;
        GameplayEvents.ReachPalace += StopMoving;

    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= StartMoving;      
        GameplayEvents.Win += WinMovement;
        GameplayEvents.ReachPalace -= StopMoving;
    }

    void StartMoving()
    {
        canMove = true;
    }

    void StopMoving()
    {
        canMove = false;
    }

    void WinMovement()
    {
        vel *= 2f;
    }

    void Update()
	{
        if (canMove)
        {
            Vector2 offset = new Vector2(0, vel * Time.deltaTime);
            quad.material.mainTextureOffset += offset;
        }		
	}
}