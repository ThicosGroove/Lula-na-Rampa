using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTo_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.FadeTo(gameObject,0,2);
        iTween.FadeTo(gameObject, iTween.Hash("alpha",0,"time",3,"delay",1,"looptype",iTween.LoopType.pingPong));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
