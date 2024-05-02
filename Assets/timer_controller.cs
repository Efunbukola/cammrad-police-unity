using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class timer_controller : MonoBehaviour {

	public Text timerText;
	public float startTime, totalTime;
	bool isRunning = true;

	// Use this for initialization
	void OnEnable() {
		//startTime = Time.time;
		//isRunning = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (isRunning) {
			float t = Time.time - startTime;
			string minutes = ((int)t / 60).ToString ();
			string seconds = (t % 60).ToString ("f2");
			timerText.text = minutes + ":" + seconds;
		}
	}

	public float endTask(){
		isRunning = false;
		totalTime = Time.time - startTime;
		return totalTime;
	}

	public void startTask(){
		startTime = Time.time;
		isRunning = true;
	}


}