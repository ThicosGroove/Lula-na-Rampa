using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCaminhao : MoveBase
{
    [Header("Caminhão parameters")]
    [SerializeField] float speedMultiplier;

    protected override void MoveBehaviour()
    {
        if (isInReach)
        {
            base.speed *= speedMultiplier;
        }
    }
}
