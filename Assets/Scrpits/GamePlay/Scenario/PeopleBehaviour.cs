using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleBehaviour : MonoBehaviour
{

    void Start()
    {
        float rndSpeed = Random.Range(3, 15);
        float rndHeight = Random.Range(3, 10);

        iTween.Init(gameObject);
        iTween.MoveBy(gameObject, iTween.Hash("y", rndHeight, "speed", rndSpeed, "looptype", iTween.LoopType.pingPong));
    }
}
