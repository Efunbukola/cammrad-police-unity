using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceLevel : MonoBehaviour {

	public Text level;

	// Use this for initialization
	void Start () {

		setVoiceLevel ("1.23455");
		
	}

	// Update is called once per frame
	void Update () {
		
	}


	void setVoiceLevel(string _level){

		level.text = "VL" + _level;
	}
}
