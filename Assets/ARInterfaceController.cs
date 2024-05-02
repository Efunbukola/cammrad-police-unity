using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * Author: Saboor Salaam
 * 
 * 
 * */

//Facade class script that interacts with AR Setups 
public class ARInterfaceController : MonoBehaviour {

	//int representing type of current AR set up
	public int SET_UP_TYPE = 1;


	//List of all set ups
	[SerializeField]
	public ARSetup[] setups;



	// Use this for initialization
	void Start () {


		//Loop through all set ups
		for (int i = 0; i < setups.Length; i++) {

			//If is the set up we want to use
			if (SET_UP_TYPE == setups [i].SET_UP_TYPE) {

				//Activate root object for that set up
				setups [i].root.SetActive (true);

			} else {

				//Deactivated all other set up root objects
				setups [i].root.SetActive (false);

			}

		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	//Will call next on whatever set up is used
	public void DisplayStep(CAMMRADMainController.CAMMRADWorkflowStep step){
		 
		Debug.Log("");


		//Loop through all set ups
		for (int i = 0; i < setups.Length; i++) {

			//If is the set up we want to use
			if (SET_UP_TYPE == setups [i].SET_UP_TYPE) {

				//Activate root object for that set up
				setups [i].controller.DisplayStep(step);

			} 

		}

	}



	//Base class for each AR set up
	[System.Serializable]
	public class ARSetup {

		public ARSetupController controller;
		public GameObject root;
		public string name;
		public int SET_UP_TYPE;
	}
}
