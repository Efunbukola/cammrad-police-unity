using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class sample_expert_ui : MonoBehaviour {

	public List<GameObject> icons = new List<GameObject>();
	public GameObject icon;
	public CAMMRADMainController mainController;
	Dictionary<string,string> headers = new Dictionary<string, string>();

	PerformanceRecord p;



	// Use this for initialization
	public void Init() {

		headers.Add ("Content-Type", "application/json");

		createPerformanceRecord();


		//Print height and width of screen
		//Debug.Log("Screen Height : " + Screen.height);
		//Debug.Log("Icon Height : " + Screen.height/icons.Count);

		//Create an icon for each step and add to list of icons
		for (int i = 0; i < mainController.currentTask.steps.Length; i++) {
			
			GameObject _icon = Instantiate (icon, gameObject.transform);

			//Add icon to list 
			icons.Add(_icon);

			Debug.Log (mainController.currentTask.steps[i].step.step_id.ToString());

			//Load image into icon object
			icons[i].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader>().LoadIcon (mainController.currentTask.steps[i].step.step_id.ToString());

		}


		//Loop through all icons and set size and position
		for (int i = 0; i < icons.Count; i++) {

			//Set size to numbers of icon divided by screen width so each icon has room
			icons [i].GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(Mathf.Clamp(Screen.height / (icons.Count+1), 0.0f, 100f), Mathf.Clamp(Screen.height / (icons.Count+1), 0.0f, 100f));

			//Icrementally set position of each icon along y axis
			icons [i].GetComponent<RawImage> ().rectTransform.position = new Vector3 (
				icons [i].GetComponent<RawImage> ().rectTransform.position.x, 
				Screen.height - ((i * (Screen.height / icons.Count)) + Screen.height/(icons.Count+4)),
				icons [i].GetComponent<RawImage> ().rectTransform.position.z);
				
			//Position icons to left side of the screen exactly 10% from edge
			icons [i].GetComponent<RawImage> ().rectTransform.localPosition = new Vector2 ((Screen.width/2) - ((Screen.width/2)*0.1f),icons [i].GetComponent<RawImage> ().rectTransform.localPosition.y );

		}
		
	}

	void OnDisable(){

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.H)){
			mainController.requestAssitance("3");
		}


	}


	public void createPerformanceRecord(){

		string json = @"{'task_name':'" + mainController.currentTask.task.task_name + "','task_id':'" + mainController.currentTask.task.task_id + @"','performance_profile_id':'" + PlayerPrefs.GetString("performance_profile_id") + "'}";

		json = json.Replace("'", "\"");

		Debug.Log (json);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8080/api/performance/create/performance_record", postData, headers);


		StartCoroutine(CreatePerformanceRecord(www));

	}


	[System.Serializable]
	public class PerformanceRecord {
		public int performance_record_id;
		public PerformanceRecord() {}
	}





	//Wait for the www Request
	IEnumerator CreatePerformanceRecord(WWW www){
		yield return www;
		if (www.error == null){
			//Print server response
			Debug.Log(www.text);
			Debug.Log(www.responseHeaders);

			p = JsonUtility.FromJson<PerformanceRecord> (www.text);
			Debug.Log ("Inserted record id: " + p.performance_record_id);

			StartCoroutine (SetProfileDeviceAssociationRecordId ());


		} else {
			//Something goes wrong, print the error response
			Debug.Log(www.error);
		}
	}


	//Set current performance profile 
	public IEnumerator SetProfileDeviceAssociationRecordId(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/device/set/profile/performance/record?association_id=" + mainController.DeviceProfileAssociationID + "&performance_record_id=" + p.performance_record_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {


		}
	}



}


// Upload video to youtube for gastro
// Work on collage
// Arrow animation appears when expert interface is first shown: blinking animation