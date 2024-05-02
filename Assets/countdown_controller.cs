using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdown_controller : MonoBehaviour {

	public Text timerText;
	public float totalTime = 0f, remainingTime = 0f;
	public bool isRunning = true;
	public Image bar;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


		if (remainingTime > 0) {
			
			remainingTime -= Time.deltaTime;

			string minutes = ((int)remainingTime / 60).ToString ();

			string seconds = (remainingTime % 60).ToString ("f2");

			timerText.text = minutes + ":" + seconds;

			//Debug.Log((remainingTime / totalTime));

			bar.fillAmount = (remainingTime / totalTime);

			if ((remainingTime / totalTime) > 0.5f) {

				timerText.color = Color.green;

				bar.color  = Color.green;

			} else if ((remainingTime / totalTime) > 0.25f) {

				timerText.color = Color.yellow;

				bar.color  = Color.yellow;

			} else {

				timerText.color = Color.red;

				bar.color  = Color.red;
			}


		} else {

			timerText.text = "ALL TIME ELAPSED!";

		}

	}

	public void startCountDown(float time){

		totalTime = time;
		remainingTime = totalTime;

	}
}
