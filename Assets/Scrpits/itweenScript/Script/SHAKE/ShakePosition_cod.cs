using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePosition_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.ShakePosition(gameObject, new Vector3(1, 1, 0), 4);
        iTween.ShakePosition(gameObject, iTween.Hash("x",0.5f,"y",0.2f,"delay",1,"time",4));
    }


        
	
	// Update is called once per frame
	void Update () {
		
	}
}
