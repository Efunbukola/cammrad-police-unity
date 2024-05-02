using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class battery_controller : MonoBehaviour {

	public Image battery_fill;
	public Text percentage;

	// Use this for initialization
	void Start () {

	}

	
	// Update is called once per frame
	void FixedUpdate () {

		setBatteryPercentage(SystemInfo.batteryLevel + "");
	}

	public void setBatteryPercentage(string p){


		float percent = float.Parse(p);

		battery_fill.fillAmount = percent;

		percentage.text = (percent * 100) + "%";

	}
}
