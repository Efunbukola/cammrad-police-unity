using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechProcessing : MonoBehaviour {

	private int current_state;
	public const int SELECTING_TASK = 0, NAVIGATING_CURRENT_TASK = 1;
	public GameObject MainController, CAMMRADPM;

	// Use this for initialization
	void OnEnable() {
		
		current_state = SELECTING_TASK;
	}
	
	// Update is called once per frame
	void Update () {



	}

	public void OnSelectTaskOne(){

		if (current_state != SELECTING_TASK)
			return;

		MainController.GetComponent<CAMMRADMainController> ().setCurrentTask("0");
		current_state = NAVIGATING_CURRENT_TASK;

	}
		

	public void OnSelectTaskTwo(){

		if (current_state != SELECTING_TASK)
			return;

		MainController.GetComponent<CAMMRADMainController> ().setCurrentTask("1");
		current_state = NAVIGATING_CURRENT_TASK;

	}


	public void OnSelectTaskThree(){

		if (current_state != SELECTING_TASK)
			return;

		MainController.GetComponent<CAMMRADMainController> ().setCurrentTask("2");
		current_state = NAVIGATING_CURRENT_TASK;
	}

	public void OnSelectTaskFour(){

		if (current_state != SELECTING_TASK)
			return;

		MainController.GetComponent<CAMMRADMainController> ().setCurrentTask("3");
		current_state = NAVIGATING_CURRENT_TASK;
	}
		

	public void OnNext(){

		if (current_state != NAVIGATING_CURRENT_TASK)
			return;

		CAMMRADPM.GetComponent<CAMMRADPMController>().next();
	}

	public void OnBack(){

		if (current_state != NAVIGATING_CURRENT_TASK)
			return;

		CAMMRADPM.GetComponent<CAMMRADPMController>().back();

	}

	public void OnFinishTask(){

		if (current_state != NAVIGATING_CURRENT_TASK)
			return;

		MainController.GetComponent<CAMMRADMainController>().finishTask();

	}



}
