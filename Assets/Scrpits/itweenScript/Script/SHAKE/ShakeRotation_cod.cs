using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeRotation_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        //iTween.ShakeRotation(gameObject, new Vector3(0,0,3), 4);
        iTween.ShakeRotation(gameObject, iTween.Hash("z",5,"delay",2,"time",4,"oncomplete","metodo","oncompleteparams","Ola Mundo"));

    }

    void metodo(string s)
    {
        print(s);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
