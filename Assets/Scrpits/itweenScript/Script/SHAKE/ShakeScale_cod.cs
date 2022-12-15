using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScale_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.ShakeScale(gameObject,new Vector3(1,1,0),4);
        iTween.ShakeScale(gameObject,iTween.Hash("x",0.5f,"y",0.1f,"delay",3,"time",4));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
