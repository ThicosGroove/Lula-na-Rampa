using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFrom_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.Init(gameObject);
        iTween.FadeFrom(gameObject,0,2);
        //iTween.FadeFrom(gameObject, iTween.Hash("alpha", 0, "time", 3, "delay", 4, "looptype", iTween.LoopType.none));
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
