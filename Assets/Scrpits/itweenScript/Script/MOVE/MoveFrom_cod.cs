using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFrom_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        iTween.MoveFrom(gameObject,Vector3.zero,8);
        //iTween.MoveFrom(gameObject,iTween.Hash("x",10,"y",20,"speed",8,"delay",2));
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
