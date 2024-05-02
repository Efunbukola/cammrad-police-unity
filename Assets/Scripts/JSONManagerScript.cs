using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JSONManagerScript : MonoBehaviour {

	public CAMMRADWorkflowTask currentTask;
	private CAMMRADWorkflowFile workflowfile;
	public GameObject PM;
	public bool isJSONInit = false;

	// Use this for initialization
	
	public void initJSON(string json_string) {
		
		workflowfile = JsonUtility.FromJson<CAMMRADWorkflowFile>(json_string); 
		isJSONInit = true;
		Debug.Log ("Tasks parsed:" + workflowfile.tasks.Length);

	}


	public void setCurrentTask(int index){
		
		if (index < workflowfile.tasks.Length && index >= 0) {

			currentTask = workflowfile.tasks[index];
		}

	}


	void Start () {

		PM.SetActive (false);


	}
	
	// Update is called once per frame
	void Update () {
		
	}



[System.Serializable]
public class CAMMRADWorkflowFile {
 public string name, type;
 public CAMMRADWorkflowTask[] tasks;
 public CAMMRADWorkflowFile() {}
}

[System.Serializable]
public class CAMMRADWorkflowTask {
  public string name;
  public CAMMRADWorkflowStep[] steps;
  public CAMMRADWorkflowTask() {
  }
}
[System.Serializable]
public class CAMMRADWorkflowStep {
 public string StepDescription, StepImageAid, StepVideoAid;
 public CAMMRADWorkflowStep() {
 }
}


}
