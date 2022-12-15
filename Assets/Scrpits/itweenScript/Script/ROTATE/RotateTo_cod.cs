using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTo_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.RotateTo(gameObject,new Vector3(0,0,45),5);
        iTween.RotateTo(gameObject,iTween.Hash("z",45,"time",5,"delay",2,"looptype",iTween.LoopType.pingPong));

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
