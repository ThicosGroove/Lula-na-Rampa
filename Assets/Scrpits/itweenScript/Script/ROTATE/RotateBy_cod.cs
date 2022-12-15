using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBy_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.RotateBy(gameObject, new Vector3(0, 0, 0.1f), 8);
        iTween.RotateBy(gameObject,iTween.Hash("z",1,"time",2,"delay",2,"looptype",iTween.LoopType.pingPong));

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
