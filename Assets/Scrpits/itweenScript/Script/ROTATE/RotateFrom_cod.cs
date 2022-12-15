using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFrom_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.RotateFrom(gameObject,new Vector3(0,0,180),8);
        iTween.RotateFrom(gameObject, iTween.Hash("z",180,"time",8,"delay",2,"looptype",iTween.LoopType.loop,"easetype",iTween.EaseType.easeInOutElastic));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
