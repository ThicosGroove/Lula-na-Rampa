using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCaminhao : MoveBase
{
    float speedMultiplier;

    protected override void Start()
    {
        base.Start();
    }

    protected override void MoveBehaviour()
    {
     
    }

    protected override void DieBehaviour()
    {
        Destroy(this.gameObject);
    }
}
