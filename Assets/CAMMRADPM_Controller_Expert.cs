using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;


public class CAMMRADPM_Controller_Expert : MonoBehaviour {

	public ListPositionCtrl listPositionCtrl;

	public float startTime, totalStepTime;

	public CAMMRADMainController mainController;

	public int index1, index2, index3, index4, index5;

	public List<GameObject> iconObjs;

	private List<Texture2D> images = new List<Texture2D>();

	public int currentStep = 0;

	public Text centerText, currentIndexText;

	Dictionary<string,string> headers = new Dictionary<string, string>();



	// Use this for initialization
	void Start () {


	}



	void OnEnable () {

		headers.Add ("Content-Type", "application/json");
		createPerformanceRecord();


		//If workflow data is present they PM should deactivate it self
		if (!mainController.isWorkflowInit) {
			gameObject.SetActive(false);
			return;
		}

		index1 = 1;
		index2 = 2; 
		index3 = 3;
		index4 = 4;
		index5 = 5;

		changeCurrentItem(0);

		startTime = Time.time;


		//stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow
		//stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
		//stepObjs[index3].GetComponent<Animator>().enabled = true;

		//stepObjs [index3].GetComponentInChildren<Text> ().fontSize = 40;
		//stepObjs [index3].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(50f, 50f);

		//CAMMRADImageLoader.loadAllImages



	}

	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			next();
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			back();
		}


		if(Input.GetKeyDown(KeyCode.F)){
			mainController.finishTask ();
		} 



	}

	public void next(){

		//Time it took to complete previous step
		float t = Time.time - startTime;

		//createPsychomotorAndLearningassRecord (0.9, t, true, "1");

		Debug.Log (t);

		string minutes = ((int)t / 60).ToString ();
		string seconds = (t % 60).ToString ("f2");
		string time = minutes + ":" + seconds;

		Debug.Log (time);



		if (currentStep < mainController.currentTask.steps.Length - 1 ) {



			resetShownItemIndexes (true);

			changeCurrentItem(currentStep+1);

			//mainController.GetComponent<LogFileCreator> ().writeToFile ("STEP " +  currentStep + " TIME : " + time);

			startTime = Time.time;

			Debug.Log ("Read step: " + mainController.currentTask.steps[currentStep].step.step_title);

			//AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			//AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
			//mainActivity.Call ("speak", mainController.currentTask.steps[currentStep].StepDescription);

			listPositionCtrl.nextContent ();

		}

	}


	public void back(){


		if (currentStep > 0) {



			resetShownItemIndexes (false);

			changeCurrentItem(currentStep-1);

			Debug.Log ("Read step: " + mainController.currentTask.steps[currentStep].step.step_title);

			listPositionCtrl.lastContent ();


		}


	}



	void changeCurrentItem(int index) {


		currentStep = index;

		if (centerText != null) {
			centerText.text = mainController.currentTask.steps [currentStep].step.step_title; 
		}

		currentIndexText.text = "Step " + (index+1) + " of " + mainController.currentTask.steps.Length;

		for (int i = 0; i < iconObjs.Count; i++) {


			iconObjs [i].GetComponent<ExpertPMStep> ().indicatorBar.color = new Color32 (0, 0, 0, 0);


			//stepObjs [i].GetComponent<Image> ().color = new Color32(80,145,242,252); //blue;
			//stepObjs[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(50f, 50f);
			//stepObjs [i].GetComponentInChildren<Text> ().fontSize = 25;
			//stepObjs [i].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(30f, 30f);

		}



		Debug.Log ("index1: " + index1);
		Debug.Log ("index2: " + index2);
		Debug.Log ("index3: " + index3);
		Debug.Log ("index4: " + index4);
		Debug.Log ("index5: " + index5);

		Debug.Log (iconObjs[index3]);


		//stepObjs[index3].GetComponentInChildren<Text> ().text = index + 1 + "";

//		iconObjs[index3].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index].StepImageAid);

		iconObjs[index3].GetComponent<ExpertPMStep>().indicatorBar.color = new Color32(255,205,11,255);


		//centerImage.GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index].StepImageAid);



		if (index - 1 < 0) { //If index before current is less than 0 show first to last step in place       2

			//stepObjs[index2].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Length - 1) + 1 + "";
			//iconObjs[index2].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Length - 1].StepImageAid);

			//stepObjs[index1].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Length - 2) + 1 + "";
			//iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Length - 2].StepImageAid);


		} else {

			//stepObjs[index2].GetComponentInChildren<Text> ().text = (index - 1) + 1 + "";
			//iconObjs[index2].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index - 1].StepImageAid);


			if (index - 2 < 0) { //If index before current is less than 0 show second to last step in place   1

				//stepObjs[index1].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Length - 1) + 1 + "";
				//iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Length - 1].StepImageAid);


			} else {

				//stepObjs[index1].GetComponentInChildren<Text> ().text = (index - 2) + 1 + "";
				//iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index - 2].StepImageAid);


			}


		}




		//*********************************************




		if (index + 1 > mainController.currentTask.steps.Length - 1) { //If index before current is less than 0 show first to last step in place

			//stepObjs[index4].GetComponentInChildren<Text> ().text = (0) + 1+  "";
			//iconObjs[index4].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[0].StepImageAid);


		} else {

			//stepObjs[index4].GetComponentInChildren<Text> ().text = (index + 1) + 1 + "";
			//iconObjs[index4].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index + 1].StepImageAid);

		}



		if (index + 2 > mainController.currentTask.steps.Length - 1) { //If index before current is less than 0 show second to last step in place

			//stepObjs[index5].GetComponentInChildren<Text> ().text = (0 + 1) + + 1 +"";
			//iconObjs[index5].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[0+ 1].StepImageAid);




		} else {

			//stepObjs[index5].GetComponentInChildren<Text> ().text = (index + 2) + 1 + "";
			//iconObjs[index5].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index + 2].StepImageAid);


		}




	}


	void resetShownItemIndexes( bool IsNext){

		if (IsNext) {


			if (index1  < iconObjs.Count - 1){

				index1++;

			} else {

				index1 = 0;

			}



			if (index2  < iconObjs.Count - 1){

				index2++;

			} else {

				index2 = 0;

			}


			if (index3  < iconObjs.Count - 1){

				index3++;

			} else {

				index3 = 0;

			}


			if (index4  < iconObjs.Count - 1){

				index4++;

			} else {

				index4 = 0;

			}

			if (index5  < iconObjs.Count - 1){

				index5++;

			} else {

				index5 = 0;

			}





		} else {


			if (index1 > 0){

				index1--;

			} else {

				index1 = iconObjs.Count - 1;

			}


			if (index2 > 0){

				index2--;

			} else {

				index2 = iconObjs.Count - 1;

			}


			if (index3 > 0){

				index3--;

			} else {

				index3 = iconObjs.Count - 1;

			}


			if (index4 > 0){

				index4--;

			} else {

				index4 = iconObjs.Count - 1;

			}


			if (index5 > 0){

				index5--;

			} else {

				index5 = iconObjs.Count - 1;

			}


		}

	}


	public void setCenterTextShown(bool s){

		Debug.Log (s + "");
		centerText.enabled = s;
	}

	public void createPerformanceRecord(){

		string json = @"{'createdAt':'Wed Oct 18 2017','task_id':'1','performance_profile_id':'" + PlayerPrefs.GetString("performance_profile_id") + "}";

		json = json.Replace("'", "\"");

		Debug.Log (json);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://10.0.0.5:8080/api/performance/create/performance_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

	}


	//Once new workflow version is downloaded this needs to use actual step_id
	//Get performance profile id from signed in user 

	public void createPsychomotorAssessmentRecord(float accuracy, float duration, bool completed, string step_id){



	}

		
		

	public IEnumerator createPsychomotorAndLearningRecord(float accuracy, float duration, bool completed, string step_id){

		string data = "";//"{\"created_at\":\"Wed Oct 18 2017 12:41:34\",\"task_id\": \"1\",\"performance_profile_id\":\"3\"}";
		byte[] body = Encoding.UTF8.GetBytes(data);

		Hashtable headers = new Hashtable();
		headers.Add("Content-Type", "application/json");

		UnityWebRequest www = UnityWebRequest.PostWwwForm("http://10.0.0.5:8080/api/performance/save/psychomotor_assessment_record", data);
		www.SetRequestHeader("Content-Type", "application/json");

		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {
			Debug.Log (www.downloadHandler.text);
		}
	}


	[System.Serializable]
	public class AccelerometerReading {
		public string time_stamp;
		public float x, y, z;
		public AccelerometerReading() {}
	}

	[System.Serializable]
	public class GyroscopeReading {
		public string time_stamp;
		public float x, y, z;
		public GyroscopeReading() {}
	}

	//Wait for the www Request
	IEnumerator WaitForRequest(WWW www){
		yield return www;
		if (www.error == null){

			//Print server response
			Debug.Log(www.text);
			Debug.Log(www.responseHeaders);

		} else {
			//Something goes wrong, print the error response
			Debug.Log(www.error);
		}
	}


}
