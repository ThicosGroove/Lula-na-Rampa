using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBy_cod : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
       // iTween.ScaleBy(gameObject,Vector3.up,2);
        iTween.ScaleBy(gameObject, iTween.Hash("x",2,"delay",2,"time",5,"oncomplete","Ola"));

    }
	
    void Ola()
    {
        print("Ola pessoal");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
