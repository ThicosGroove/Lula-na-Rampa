using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBy_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.MoveBy(gameObject,new Vector3(5,8,0),8);
        iTween.MoveBy(gameObject,iTween.Hash("x",1,"y",0, "speed", 0.3f, "delay",4));
       
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
