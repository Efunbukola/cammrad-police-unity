using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network_controller : MonoBehaviour {

	public GameObject bar1, bar2, bar3, bar4, bar5;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void setSignalStrength (string x){

		int s = int.Parse (x);
		switch (s) {

		case 1: 

			bar1.SetActive (true);
			bar2.SetActive (false);
			bar3.SetActive (false);
			bar4.SetActive (false);
			bar5.SetActive (false);

			break;
		case 2: 

			bar1.SetActive (true);
			bar2.SetActive (true);
			bar3.SetActive (false);
			bar4.SetActive (false);
			bar5.SetActive (false);


			break;
		case 3: 

			bar1.SetActive (true);
			bar2.SetActive (true);
			bar3.SetActive (true);
			bar4.SetActive (false);
			bar5.SetActive (false);


			break;
		case 4: 


			bar1.SetActive (true);
			bar2.SetActive (true);
			bar3.SetActive (true);
			bar4.SetActive (true);
			bar5.SetActive (false);


			break;
		case 5: 

			bar1.SetActive (true);
			bar2.SetActive (true);
			bar3.SetActive (true);
			bar4.SetActive (true);
			bar5.SetActive (true);

			break;

		}

	}

}
