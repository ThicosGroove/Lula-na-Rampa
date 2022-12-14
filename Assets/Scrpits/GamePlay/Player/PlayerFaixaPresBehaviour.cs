using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class PlayerFaixaPresBehaviour : MonoBehaviour
{
    [SerializeField] GameObject faixa_FBX;
    [SerializeField] Transform faixa_Pos;
    [SerializeField] float time;

    bool canMove = false;

    private void OnEnable()
    {
        GameplayEvents.DropFaixa += WinBehaviour;
    }

    private void OnDisable()
    {
        GameplayEvents.DropFaixa -= WinBehaviour;
        
    }

    void WinBehaviour()
    {
        faixa_FBX.SetActive(true);
        canMove = true;
    }

    private void Start()
    {
        faixa_FBX.SetActive(false);
    }

    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.Lerp(transform.position, faixa_Pos.position, time);        
        }
    }
}
