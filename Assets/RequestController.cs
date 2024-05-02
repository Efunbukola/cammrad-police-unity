using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestController : MonoBehaviour {

	public GameObject selectTaskDialog, waitingForMatchDialog, foundMatchDialog, rateInstructorDialog;
	public Text instructorName, instructorRatingType;
	public RatingSystem instructorRating, selectedInstructorRating;
	public CAMMRADMainController.CAMMRADWorkflowTask selectedTask;
	public CAMMRADMainController mainController;
	public Request request;
	public RequestMatch match;

	Dictionary<string,string> headers = new Dictionary<string, string>();


	// Use this for initialization
	void Start () {

		headers.Add ("Content-Type", "application/json");


	}
	
	// Update is called once per frame
	void Update () {

		if (rateInstructorDialog.activeSelf) {
			switch (selectedInstructorRating.startupRating) {
			case 1:
				instructorRatingType.text = "Very bad";
				instructorRatingType.color = Color.red;
				break;
			case 2:
				instructorRatingType.text = "Bad";
				instructorRatingType.color = Color.red;
				break;
			case 3:
				instructorRatingType.text = "Ok";
				instructorRatingType.color = Color.gray;
				break;
			case 4:
				instructorRatingType.text = "Good";
				instructorRatingType.color = Color.green;
				break;
			case 5:
				instructorRatingType.text = "Great";
				instructorRatingType.color = Color.green;
				break;

			}
		}
		
	}

	public void startInstructorRequest(){

		selectTaskDialog.SetActive(true);

	}


	public void cancelInstructorRequest(){

		selectTaskDialog.SetActive(false);

	}

	public void selectTask(string index ){

		int i = int.Parse (index);

		selectedTask = mainController.workflow.tasks[i];


		StartCoroutine (SendRequest ());




	}

	public void setStep(int step_id){

		if (match == null) {

			return;

		}

		StartCoroutine (UpdateStep (step_id));

	}


	public void finishTask(){

		StartCoroutine (Finish());


	}

	public IEnumerator UpdateStep(int step_id){
		

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/matching/update/step?request_match_id=" + match.request_match_id + "&step_id=" + step_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {


			mainController.setCurrentTask("0");
			foundMatchDialog.SetActive(false);

		}
	}

	public IEnumerator Finish(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/matching/finish?request_match_id=" + match.request_match_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {

			foundMatchDialog.SetActive(false);
			rateInstructorDialog.SetActive (true);	
		}
	}


	public void rateInstructor(){

		StartCoroutine (sendInstructorRating());

	}

	public IEnumerator sendInstructorRating(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/matching/rate/instructor?instructor_id=" + match.instructor_id + "&rating=" + selectedInstructorRating.startupRating);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {

			rateInstructorDialog.SetActive (false);

		}
	}



	//Set current performance profile 
	public IEnumerator SendRequest(){

		string json = @"{
		'performance_profile_id':'" 
			+ 3 + @"',
		'task_id':'" + selectedTask.task.task_id + "'}";

		json = json.Replace("'", "\"");

		Debug.Log (json);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);

		WWW www = new WWW("http://localhost:8081/api/matching/create/request", postData, headers);

		yield return www;
		if (www.error == null){
			//Print server response
			Debug.Log(www.text);
			Debug.Log(www.responseHeaders);

			selectTaskDialog.SetActive(false);
			waitingForMatchDialog.SetActive (true);

			request = JsonUtility.FromJson<Request> (www.text);

			InvokeRepeating ("checkForMatch", 0f, 5f);


		} else {
			//Something goes wrong, print the error response
			Debug.Log(www.error);
		}
	}

	void checkForMatch(){

		StartCoroutine (GetMatch ());

	}

	public void startTask(){
		StartCoroutine(StartTask());
	}

	public IEnumerator StartTask(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/matching/set/match/active?request_match_id=" + match.request_match_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {


			mainController.setCurrentTask("0");
			foundMatchDialog.SetActive(false);

		}
	}

	public IEnumerator GetMatch(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/matching/check/request?request_id=" + request.request_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {

			Debug.Log (www.downloadHandler.text);

			match = JsonUtility.FromJson<RequestMatch> (www.downloadHandler.text);

			CancelInvoke ();

			waitingForMatchDialog.SetActive(false);
			foundMatchDialog.SetActive (true);

			instructorName.text = match.instructor_first_name + " " + match.instructor_last_name;
			Debug.Log ("Rating was " + Mathf.Round(match.instructor_rating));
			instructorRating.setRating (match.instructor_rating < 5 ?Mathf.Round(match.instructor_rating): 5);

		}
	}


	[System.Serializable]
	public class Request{
		public int request_id, performance_profile_id,task_id; 
		public Request() {}
	}

	[System.Serializable]
	public class RequestMatch {
		public string instructor_first_name, instructor_last_name;
		public float instructor_rating;
		public int request_match_id, instructor_id;
		public RequestMatch() {}
	}


}
