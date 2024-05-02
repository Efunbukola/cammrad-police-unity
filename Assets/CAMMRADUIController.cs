using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMMRADUIController : MonoBehaviour {

	private date_controller date;
	private network_controller network;
	private battery_controller battery;

	// Use this for initialization
	void Start () {

		date = GameObject.FindGameObjectWithTag ("date_controller").GetComponent<date_controller>();
		//network = GameObject.FindGameObjectWithTag ("network_controller").GetComponent<network_controller>();
		battery = GameObject.FindGameObjectWithTag ("battery_controller").GetComponent<battery_controller>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateValue(string id, string value){

		switch (id) {
		case "battery":
			battery.setBatteryPercentage (value);
			break;

		case "date":
			date.setDate (value);
			break;

		case "network":
			network.setSignalStrength (value);
			break;

		default:
			break;
		}
	}

}
