using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFrom_cod : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        iTween.Init(gameObject);
        //iTween.ColorFrom(gameObject,Color.grey,3);
        //iTween.ColorFrom(gameObject,iTween.Hash("r",0.5f,"g",0.5f,"b",1f,"time",3,"delay",2,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.easeInCubic));
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public Vector3[] caminho;


    private void OnDrawGizmos()
    {
        iTween.DrawLineGizmos(caminho,Color.red);
    }
}
