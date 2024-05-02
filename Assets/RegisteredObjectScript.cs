using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Author: Saboor
 * 
 * 
 * 
 * */


//This class is placed on registered objects to ensure that the animation plays even after the object is disabled and enabled 
public class RegisteredObjectScript : MonoBehaviour {


	public string animator_state = "Idle";

	void OnEnable(){

		GetComponent<Animator>().Play (animator_state);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
