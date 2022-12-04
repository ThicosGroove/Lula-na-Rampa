using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCaminhao : MoveBase
{
    [SerializeField] LevelSO[] levelData;

    [SerializeField] float speedMultiplier;

    protected override void Start()
    {
        base.Start();
        speedMultiplier = levelData[LevelManager.Instance.currentLevel - 1].speedMulti;
    }

    protected override void MoveBehaviour()
    {
        if (isInReach)
        {
            base.speed *= speedMultiplier;
        }
    }
}
