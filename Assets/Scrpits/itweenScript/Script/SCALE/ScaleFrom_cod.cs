﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFrom_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.ScaleFrom(gameObject,new Vector3(3,0.5f,1),5);
        //iTween.ScaleFrom(gameObject, iTween.Hash("x", 3, "y", 4, "delay", 2,"looptype",iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInBounce));


    }



    // Update is called once per frame
    void Update () {
		
	}
}