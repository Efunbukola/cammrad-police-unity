using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sample_collage_ui : MonoBehaviour {

	public List<GameObject> icons = new List<GameObject>();
	public GameObject icon;
	public CAMMRADMainController mainController;




	// Use this for initialization
	public void Init() {

		//Print height and width of screen
		Debug.Log("Screen Width : " + Screen.width);
		Debug.Log("Screen Height : " + Screen.height);

		//Create an icon for each step and add to list of icons
		for (int i = 0; i < mainController.currentTask.steps.Length; i++) {

			GameObject _icon = Instantiate (icon, gameObject.transform);
			_icon.GetComponentInChildren<Text> ().text = (i+1)+"";

			//Add icon to list 
			icons.Add(_icon);

			icons[i].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader>().LoadIcon (mainController.currentTask.steps[i].step.step_id.ToString());

			//Load image into icon object

		}

		int X_COUNTER = 0, Y_COUNTER = 0;
		float x_axis_counter = 0.1f * icons.Count;

		if (icons.Count < 30) {

			//Loop through all icons and set size and position
			for (int i = 0; i < icons.Count; i++) {



				//Set size to numbers of icon divided by screen width so each icon has room
				icons [i].GetComponent<RawImage> ().rectTransform.sizeDelta = new Vector2 (Mathf.Clamp (Screen.height / (icons.Count + 1), 0.0f, 100f), Mathf.Clamp (Screen.height / (8), 0.0f, 100f));

				//Icrementally set position of each icon along y axis
				icons [i].GetComponent<RawImage> ().rectTransform.position = new Vector3 (
					icons [i].GetComponent<RawImage> ().rectTransform.position.x, 
					Screen.height - ((Y_COUNTER * (Screen.height / icons.Count)) + (Screen.height / (icons.Count + 4))),
					icons [i].GetComponent<RawImage> ().rectTransform.position.z);

				//Position icons to left side of the screen exactly 10% from edge
				icons [i].GetComponent<RawImage> ().rectTransform.localPosition = new Vector2 ((X_COUNTER * (Screen.height / (icons.Count))) + 25, icons [i].GetComponent<RawImage> ().rectTransform.localPosition.y);

				if (X_COUNTER < 2) {
					X_COUNTER++;
				} else {
					X_COUNTER = 0;
					x_axis_counter -= 0.08f;
					Y_COUNTER++;
				}



			}
		}

	}

	void OnDisable(){

	}

	// Update is called once per frame
	void Update () {

	}


	public void hideNumbers(){

		for (int i = 0; i < icons.Count; i++) {

			icons [i].GetComponentInChildren<Text> ().text = "";

		}

	}


	public void showNumbers(){


		for (int i = 0; i < icons.Count; i++) {

			icons [i].GetComponentInChildren<Text> ().text = (i + 1) + "";

		}

	}
}


