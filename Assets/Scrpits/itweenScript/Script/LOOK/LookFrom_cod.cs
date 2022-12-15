using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFrom_cod : MonoBehaviour {

    public Transform posAlvo;

	// Use this for initialization
	void Start () {

        iTween.Init(gameObject);
        // iTween.LookFrom(gameObject,new Vector3(posAlvo.position.x,posAlvo.position.y,posAlvo.position.z),4);
        // iTween.LookFrom(gameObject, iTween.Hash("looktarget", new Vector3(posAlvo.position.x, posAlvo.position.y, posAlvo.position.z),"delay",3,"time",4));

        //iTween.LookTo(gameObject, new Vector3(posAlvo.position.x, posAlvo.position.y, posAlvo.position.z),3);
        //iTween.LookTo(gameObject, iTween.Hash("looktarget", new Vector3(posAlvo.position.x, posAlvo.position.y, posAlvo.position.z), "delay", 3, "time", 4,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.easeInOutElastic));
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
