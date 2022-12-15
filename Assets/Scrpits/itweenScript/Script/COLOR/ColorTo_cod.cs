using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTo_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.ColorTo(gameObject,new Color(0,0,1),3);
        iTween.ColorTo(gameObject,iTween.Hash("color",Color.green,"time",4,"delay",5,"includechildren",false));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
