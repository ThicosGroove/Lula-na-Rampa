using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        iTween.MoveTo(gameObject, Vector3.zero, 5);
        //iTween.MoveTo(gameObject, iTween.Hash("y", 0,"speed",5));
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
