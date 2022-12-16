using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehaviour : MonoBehaviour
{
    void Start()
    {
        iTween.Init(gameObject);


        float moveRndSpeed = Random.Range(3, 15);
        float moveRndHeight = Random.Range(3, 10);

        iTween.MoveBy(gameObject, iTween.Hash(
            "y", moveRndSpeed,
            "speed", moveRndHeight,
            "looptype",
            iTween.LoopType.pingPong));

        float rotateRndSpeed = Random.Range(50, 150);
        float rotateRndY = Random.Range(0.25f, 2f);
        float rotateRndX = Random.Range(0, 0.25f);

        iTween.RotateBy(gameObject, iTween.Hash(
            "y", rotateRndY,
            "x", rotateRndX,
            "speed", rotateRndSpeed,
            "easetype", iTween.EaseType.easeInOutCubic,
            "looptype", iTween.LoopType.pingPong));
    }

 
}
