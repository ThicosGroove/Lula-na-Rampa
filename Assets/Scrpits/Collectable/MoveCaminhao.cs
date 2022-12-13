using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCaminhao : MoveBase
{
    float speedMultiplier;

    protected override void Start()
    {
        base.Start();
        speedMultiplier = LevelManager.Instance.current_caminhaoMulti;
    }

    protected override void MoveBehaviour()
    {
        if (isInReach)
        {
            base.speed *= speedMultiplier;
        }
    }

    protected override void DieBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
