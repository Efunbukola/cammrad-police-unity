using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


/*
 * Create by Saboor Salaam 
 * Last edited 9/19/2017
 * 
 * 
 * CAMMRADPMController
 * 
 * Purpose: To serve as controller for the Pictoral Mnemonic 
 * 
 * 
 * METHODS: 
 * __________________________________________________________
 * 
 * Start() 
 * Inherited from MonoBehaviour
 * Called once in the GameObjects lifetime when it is initialized 
 * 
 * Update()
 * Inherited from MonoBehaviour
 * Called once every frame
 * 
 * 
 * next()
 * Used to increment the currently highlighted step
 * 
 * 
 * back()
 * Used to decrement the currently highlighted step
 * 
 * 
 * changeCurrentItem ( int index)
 * Updates currentStep, changes centerText, updates the indexes of each shown item, updates  
 * 
 * parameters: 
 * 
 * int index 
 * the current step we are changing to 

 * 
 * 
 * 
 * 
 */
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class CAMMRADPMController : MonoBehaviour {

	bool isRotating = false, isWaitingForConditionResponse = false;
	bool isRotatingForward = false;
	float degreesRotatedAlready = 0;
	float distanceBetweenPoints = 30f;

	public RequestController requestController;

	public countdown_controller countDownController; 

	public CAMMRADCacheManager cacheManager;

	public float startTime, totalStepTime;

	public CAMMRADMainController mainController;

	public Text centerText, conditionsText;

	public int index1, index2, index3, index4, index5, currentStepConditionIndex;

	public List<GameObject> stepObjs, iconObjs;

	public GameObject iconsCircle, stepsCircle, centerImage, report, reportContent;

	private List<Texture2D> images = new List<Texture2D>();

	Dictionary<string,string> headers = new Dictionary<string, string>();

	List<AccelerometerReading> accelData = new List<AccelerometerReading> ();
	List<GyroscopeReading> gyroData = new List<GyroscopeReading> ();

	public EMGData emgData = new EMGData();
	public NISData nisData = new NISData();

	public int currentStep = 0;

	public float rotationSpeed = 5f;

	PerformanceRecord p;

	string result_string = "";

	public GameObject videoAid;

	public Report r;

	void OnEnable () {

		conditionsText.text = "";

		Debug.Log(System.DateTime.Now);

		//createPerformanceRecord();

		//initRecordingData();

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


		countDownController.startCountDown(mainController.currentTask.steps[currentStep].step.expected_time);


		//requestController.setStep(mainController.currentTask.steps[currentStep].step.step_id);


		startTime = Time.time;

	
		stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow
		stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
		stepObjs[index3].GetComponent<Animator>().enabled = true;

		//stepObjs [index3].GetComponentInChildren<Text> ().fontSize = 40;
		//stepObjs [index3].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(50f, 50f);

		//CAMMRADImageLoader.loadAllImages



	}

	void Update ()
	{

		if(Input.GetKeyDown(KeyCode.R) && !isRotating){
			mainController.resumeExpertInterface();
		} 

		if(Input.GetKeyDown(KeyCode.F) && !isRotating){
			mainController.finishTask ();
		} 

		if(Input.GetKeyDown(KeyCode.LeftArrow) && !isRotating){
			back();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow) && !isRotating){
			next();
		}

		if(Input.GetKeyDown(KeyCode.F) && !isRotating){
			mainController.finishTask ();
		}

		if(Input.GetKeyDown(KeyCode.H) && !isRotating){
			requestHelp();
		}


		if(Input.GetKeyDown(KeyCode.P) && !isRotating){
			endHelp();
		}





		if (isRotating && !isRotatingForward) {

			stepsCircle.transform.Rotate(Vector3.forward, 1 * rotationSpeed );
			iconsCircle.transform.Rotate(Vector3.forward, 1 * rotationSpeed );

			degreesRotatedAlready += 1 * rotationSpeed;  

			if (degreesRotatedAlready >= distanceBetweenPoints) { //Stop rotating

				isRotating = false;
				isRotatingForward = false;
				degreesRotatedAlready = 0;

				for (int i = 0; i < stepObjs.Count; i++) {

					stepObjs [i].GetComponentInChildren<Text> ().enabled = true;

				}


				for (int i = 0; i < stepObjs.Count; i++) {

					if (i != index3) {
						stepObjs [i].GetComponent<Image> ().color = new Color32(80,145,242,252); //blue;
						stepObjs[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(50f, 50f);
						stepObjs[i].GetComponent<Animator>().enabled = false;
						//stepObjs [i].GetComponentInChildren<Text> ().fontSize = 25;
						//stepObjs [i].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(30f, 30f);
					} else {

						stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow;
						stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
						stepObjs[index3].GetComponent<Animator>().enabled = true;

						//stepObjs [index3].GetComponentInChildren<Text> ().fontSize = 40;
						//stepObjs [index3].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(50f, 50f);

					}

				}

			}

		} else if (isRotating && isRotatingForward) {

			stepsCircle.transform.Rotate(Vector3.back, 1 * rotationSpeed );
			iconsCircle.transform.Rotate(Vector3.back, 1 * rotationSpeed );


				degreesRotatedAlready += 1 * rotationSpeed;  


				if (degreesRotatedAlready >= distanceBetweenPoints) { //Stop rotating

					isRotating = false;
					isRotatingForward = false;
					degreesRotatedAlready = 0;

					for (int i = 0; i < stepObjs.Count; i++) {

						stepObjs [i].GetComponentInChildren<Text> ().enabled = true;

					}


				for (int i = 0; i < stepObjs.Count; i++) {

					if (i != index3) {
						stepObjs [i].GetComponent<Image> ().color = new Color32(80,145,242,252); //blue;
						stepObjs[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(50f, 50f);
						stepObjs[i].GetComponent<Animator>().enabled = false;
						//stepObjs [i].GetComponentInChildren<Text> ().fontSize = 25;
						//stepObjs [i].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(30f, 30f);

					} else {

						stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow;
						stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
						stepObjs[index3].GetComponent<Animator>().enabled = true;
						//stepObjs [index3].GetComponentInChildren<Text> ().fontSize = 40;
						//stepObjs [index3].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(50f, 50f);


					}

				}

				}
		}



		for (int i = 0; i < stepObjs.Count; i++) {

			stepObjs[i].GetComponentInChildren<Text>().transform.rotation = Quaternion.identity;
			//iconObjs[i].transform.rotation = Quaternion.identity;

		}


	}

	public void next(){

		if (isRotating || isWaitingForConditionResponse) {
			return;
		}

		//countDownController.startCountDown(mainController.currentTask.steps[currentStep].step.expected_time);


		//AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		//AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		//mainActivity.Call ("speak", mainController.currentTask.steps[currentStep].step.completion_response);



		float t = Time.time - startTime;


		//createPsychomotorAssessmentRecord(0.9f, t, true, "3");

		//saveData();

		Debug.Log (t);



		string minutes = ((int)t / 60).ToString ();
		string seconds = (t % 60).ToString ("f2");
		string time = minutes + ":" + seconds;

		Debug.Log (time);



		if (currentStep < mainController.currentTask.steps.Length - 1 ) {

			isRotating = true;
			isRotatingForward = true;
			degreesRotatedAlready = 0;		

			resetShownItemIndexes (true);

			for (int i = 0; i < stepObjs.Count; i++) {

				stepObjs [i].GetComponentInChildren<Text> ().enabled = false;

			}


			changeCurrentItem(currentStep+1);


			//requestController.setStep(mainController.currentTask.steps[currentStep].step.step_id);

			//StartCoroutine (SetProfileDeviceAssociationStep(mainController.currentTask.steps[currentStep].step.step_id));

			//mainController.GetComponent<LogFileCreator> ().writeToFile ("STEP " +  currentStep + " TIME : " + time);

			startTime = Time.time;


			//Debug.Log ("Read step: " + mainController.currentTask.steps[currentStep].step.step_title);

//			AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			//AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
			//mainActivity.Call ("speak", mainController.currentTask.steps[currentStep].StepDescription);

			//Check conditions and execute if exist
			//executeConditions();

		}

	}


	//Check for conditions in current step
	public void executeConditions(){

//		Debug.Log ("Read step: " + mainController.currentTask.steps[currentStep].step.step_title);

		result_string = "";

		//If here are conditions
		if (mainController.currentTask.steps [currentStep].conditions.Length > 0) {

			conditionsText.text = mainController.currentTask.steps [currentStep].conditions [currentStepConditionIndex].condition_query + "                              Say \"Yes\" or \"No\"";

			//Signal that we are waiting for a response 
			isWaitingForConditionResponse = true;
			currentStepConditionIndex = 0;

			//Send condition to android code and wait
			//androidExecuteCondition();
			Invoke("androidExecuteCondition", 2);

			countDownController.startCountDown(mainController.currentTask.steps[currentStep].step.expected_time);


		} else {


			Debug.Log ("no conditions!!");

		}
			

	}

	//Mock function to simulate executing android code
	public void mockAndroidExecuteCondition(){

		Debug.Log ("Reading condition: " + mainController.currentTask.steps [currentStep].conditions[currentStepConditionIndex].condition_query);
		onRespondTrueToCondition();
	}



	//Pass condition execution to android
	public void androidExecuteCondition(){


		//Call android function
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		mainActivity.Call ("executeCondition", mainController.currentTask.steps [currentStep].conditions[currentStepConditionIndex].condition_query);
	}


	//Function called by android when user responds yes
	public void onRespondTrueToCondition(){

		result_string += "T";

		//If there are remaining conditions for this step then execute the next one
		if ((currentStepConditionIndex+1) <= (mainController.currentTask.steps[currentStep].conditions.Length-1)) {
			Debug.Log((currentStepConditionIndex+1) + "vs" + (mainController.currentTask.steps[currentStep].conditions.Length-1));
			currentStepConditionIndex += 1;
			//Send condition to android code and wait

			conditionsText.text = mainController.currentTask.steps [currentStep].conditions [currentStepConditionIndex].condition_query + "                             Say \"Yes\" or \"No\"";
			//Wait to 2 seconds then execute next condition
			Invoke("androidExecuteCondition", 2);
			isWaitingForConditionResponse = true;


		} else {


		

			//Route to step


			conditionsText.text = "";

			Debug.Log ("Result string: " + result_string);

			foreach (CAMMRADMainController.CAMMRADStepRoute route in mainController.currentTask.steps[currentStep].routes) {

				if(route.condition_result.ToLower().Equals(result_string.ToLower())){

					AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
					AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
					mainActivity.Call ("speak", "Changing to step " + route.result_step_index);

					changeCurrentItem (route.result_step_index);
					stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow;

				}

			}

			isWaitingForConditionResponse = false;
			currentStepConditionIndex = 0;


		}


	}

	//Function called by android when user responds no
	public void onRespondFalseToCondition(){

		result_string += "F";

		//If there are remaining conditions for this step then execute the next one
	
		if ((currentStepConditionIndex+1) <= (mainController.currentTask.steps[currentStep].conditions.Length-1)) {

			Debug.Log((currentStepConditionIndex+1) + "vs" + (mainController.currentTask.steps[currentStep].conditions.Length-1));
			currentStepConditionIndex += 1;
			//Send condition to android code and wait

			conditionsText.text = mainController.currentTask.steps [currentStep].conditions [currentStepConditionIndex].condition_query + "                             Say \"Yes\" or \"No\"";

			//Wait to 2 seconds then execute next condition
			Invoke("androidExecuteCondition", 2);
			isWaitingForConditionResponse = true;


		}else {


		
			conditionsText.text = "";

			Debug.Log ("Result string: " + result_string);


			//Route to step

			foreach (CAMMRADMainController.CAMMRADStepRoute route in mainController.currentTask.steps[currentStep].routes) {

				if(route.condition_result.ToLower().Equals(result_string.ToLower())){



					AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
					AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
					mainActivity.Call ("speak", "Changing to step " + route.result_step_index);
					stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow;
					changeCurrentItem (route.result_step_index);


				}

			}

			isWaitingForConditionResponse = false;
			currentStepConditionIndex = 0;

			


		}


	}



	public void back(){

		if (isRotating || isWaitingForConditionResponse) {
			return;
		}

		if (currentStep > 0) {
			
			isRotating = true;
			isRotatingForward = false;


			degreesRotatedAlready = 0;
			//changeCurrentItem (currentItem - 1);


			resetShownItemIndexes (false);

			for (int i = 0; i < stepObjs.Count; i++) {

				stepObjs [i].GetComponentInChildren<Text> ().enabled = false;

			}

			changeCurrentItem(currentStep-1);

			//requestController.setStep(mainController.currentTask.steps[currentStep].step.step_id);

			//StartCoroutine (SetProfileDeviceAssociationStep(mainController.currentTask.steps[currentStep].step.step_id));


			//Debug.Log ("Read step: " + mainController.currentTask.steps[currentStep].step.step_title);


			//Check conditions and execute if exist
			executeConditions();

		}

	}


	public void changeCurrentItemForAssistence(int index){

		changeCurrentItem (index);

		stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow;
		stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
		stepObjs[index3].GetComponent<Animator>().enabled = true;

		requestHelp();

	}


	public void changeCurrentItem(int index) {


		currentStep = index;

		if (centerText != null) {

			/*
			if (mainController.currentTask.steps [currentStep].StepDescription.Length > 70) {
				centerText.text = mainController.currentTask.steps [currentStep].StepDescription.Substring (0, 69) + "...";
			} else {
			}
			*/

			centerText.text =   mainController.currentTask.steps[currentStep].step.step_title; 



		}

		for (int i = 0; i < stepObjs.Count; i++) {

			stepObjs [i].GetComponent<Image> ().color = new Color32(80,145,242,252); //blue;
			stepObjs[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(50f, 50f);
			//stepObjs [i].GetComponentInChildren<Text> ().fontSize = 25;
			//stepObjs [i].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(30f, 30f);

		}


		stepObjs[index3].GetComponentInChildren<Text> ().text = index + 1 + "";
		//iconObjs[index3].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index].StepImageAid);

		//centerImage.GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadIcon (mainController.currentTask.steps[index].step.step_id.ToString());

		Debug.Log ("Loading icon cammrad pm: " + mainController.currentTask.steps[index].icon.file_path.Replace("http", "https"));

		//centerImage.GetComponent<RawImage> ().texture = cacheManager.text;

		//cacheManager.LoadIcon(mainController.currentTask.steps[index].icon.file_path);

		StartCoroutine(cacheManager.DownloadIconFromUrl(mainController.currentTask.steps[index].icon.file_path.Replace("http", "https")));


		if (index - 1 < 0) { //If index before current is less than 0 show first to last step in place       2

			stepObjs[index2].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Length - 1) + 1 + "";
			//iconObjs[index2].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Length - 1].StepImageAid);


			stepObjs[index1].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Length - 2) + 1 + "";
			//iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Length - 2].StepImageAid);


		} else {

			stepObjs[index2].GetComponentInChildren<Text> ().text = (index - 1) + 1 + "";
			//iconObjs[index2].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index - 1].StepImageAid);


			if (index - 2 < 0) { //If index before current is less than 0 show second to last step in place   1
				 
				stepObjs[index1].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Length - 1) + 1 + "";
				//iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Length - 1].StepImageAid);


			} else {

				stepObjs[index1].GetComponentInChildren<Text> ().text = (index - 2) + 1 + "";
				//iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index - 2].StepImageAid);


			}


		}



		//*********************************************




		if (index + 1 > mainController.currentTask.steps.Length - 1) { //If index before current is less than 0 show first to last step in place

			stepObjs[index4].GetComponentInChildren<Text> ().text = (0) + 1+  "";
			//iconObjs[index4].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[0].StepImageAid);


		} else {

			stepObjs[index4].GetComponentInChildren<Text> ().text = (index + 1) + 1 + "";
			//iconObjs[index4].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index + 1].StepImageAid);

		}



		if (index + 2 > mainController.currentTask.steps.Length - 1) { //If index before current is less than 0 show second to last step in place

			stepObjs[index5].GetComponentInChildren<Text> ().text = (0 + 1) + + 1 +"";
			//iconObjs[index5].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[0+ 1].StepImageAid);




		} else {

			stepObjs[index5].GetComponentInChildren<Text> ().text = (index + 2) + 1 + "";
			//iconObjs[index5].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index + 2].StepImageAid);


		}




	}


	public void CreateReport(){

		return;

		StartCoroutine (GenerateReport());

		report.SetActive(true);

	}

	public void HideReport(){

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}


	//Shows either aid or video to the user
	public void requestHelp(){

		Debug.Log(mainController.currentTask.steps[currentStep].step.videoUrl);

		videoAid.SetActive(true);
		videoAid.GetComponent<StreamVideo>().PlayVideo(mainController.currentTask.steps[currentStep].step.videoUrl);
		centerImage.SetActive(false);
		return;


		if (mainController.currentTask.steps [currentStep].aids.Length > 0) {
			centerImage.GetComponent<RawImage> ().texture = GetComponent<CAMMRADImageLoader> ().LoadAid (mainController.currentTask.steps [currentStep].aids [0].aid_id.ToString());
		} else {
			Debug.Log("NO AIDS PRESENT!!");
		}

	}


	public void endHelp(){
		videoAid.SetActive(false);
		centerImage.SetActive(true);
	}





	void resetShownItemIndexes( bool IsNext){

		if (IsNext) {


			if (index1  < stepObjs.Count - 1){

				index1++;

			} else {

				index1 = 0;

			}



			if (index2  < stepObjs.Count - 1){

				index2++;

			} else {

				index2 = 0;

			}


			if (index3  < stepObjs.Count - 1){

				index3++;

			} else {

				index3 = 0;

			}


			if (index4  < stepObjs.Count - 1){

				index4++;

			} else {

				index4 = 0;

			}

			if (index5  < stepObjs.Count - 1){

				index5++;

			} else {

				index5 = 0;

			}





		} else {


			if (index1 > 0){

				index1--;

			} else {

				index1 = stepObjs.Count - 1;

			}


			if (index2 > 0){

				index2--;

			} else {

				index2 = stepObjs.Count - 1;

			}


			if (index3 > 0){

				index3--;

			} else {

				index3 = stepObjs.Count - 1;

			}


			if (index4 > 0){

				index4--;

			} else {

				index4 = stepObjs.Count - 1;

			}


			if (index5 > 0){

				index5--;

			} else {

				index5 = stepObjs.Count - 1;

			}


		}

	}


	public void setCenterTextShown(bool s){

		Debug.Log (s + "");
		centerText.enabled = s;
	}



	public void createPerformanceRecord(){

		string json = @"{'task_name':'" + mainController.currentTask.task.task_name + "','task_id':'" + mainController.currentTask.task.task_id + @"','performance_profile_id':'" + PlayerPrefs.GetString("performance_profile_id") + "'}";

		json = json.Replace("'", "\"");

		Debug.Log (json);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8081/api/performance/create/performance_record", postData, headers);


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

	//Once new workflow version is downloaded this needs to use actual step_id
	//Get performance profile id from signed in user 

	public void createPsychomotorAssessmentRecord(float accuracy, float duration, bool completed, string step_id){

		string json = @"{
		'step_id':'" 
			+ mainController.currentTask.steps[currentStep].step.step_id + @"',
		'accuracy':'.8',
			'performance_record_id':'"
			+ p.performance_record_id + @"',
		'completed':'true', 'duration':'" + duration + @"'}";

		json = json.Replace("'", "\"");

		Debug.Log("PsychomotorAssessmentRecord...");
		Debug.Log (json);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8081/api/performance/save/psychomotor_assessment_record", postData, headers);

		StartCoroutine(SavePsychomotorData(www));

	}

	//Wait for the www Request
	IEnumerator SavePsychomotorData(WWW www){
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


	[System.Serializable]
	public class AccelerometerReading {
		
		public string time_stamp;
		public float x, y, z;

		public AccelerometerReading() {}

		public AccelerometerReading(float x ,float y, float z, string time_stamp) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.time_stamp = time_stamp;
		}

	}

	[System.Serializable]
	public class GyroscopeReading {
		
		public string time_stamp;
		public float x, y, z;
		public GyroscopeReading() {}

		public GyroscopeReading(float x ,float y, float z, string time_stamp) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.time_stamp = time_stamp;
		}

	}

	[System.Serializable]
	public class EMGData {

		public float min, max;
		public List<EMGReading> data = new List<EMGReading>();

		public EMGData() {}

		public EMGData(float min ,float max) {
			this.min = min;
			this.max = max;
		}

	}

	[System.Serializable]
	public class EMGReading {

		public string time_stamp;
		public float amplitude;
		public EMGReading() {}

		public EMGReading(float amplitude, string time_stamp) {
			this.amplitude = amplitude;
			this.time_stamp = time_stamp;
		}

	}

	[System.Serializable]
	public class NISData {

		public float min, max;
		public List<NISReading> data = new List<NISReading>();

		public NISData() {}

		public NISData(float min ,float max) {
			this.min = min;
			this.max = max;
		}

	}

	[System.Serializable]
	public class NISReading {

		public string time_stamp;
		public float amplitude;
		public NISReading() {}

		public NISReading(float amplitude, string time_stamp) {
			this.amplitude = amplitude;
			this.time_stamp = time_stamp;

		}

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


	public void initRecordingData(){

		InvokeRepeating ("generateRandomDataReading", 1.0f, .1f);


	}

	public void generateRandomDataReading(){

		accelData.Add(new AccelerometerReading(Random.Range(0.0f, 3.0f),Random.Range(0.0f, 3.0f),Random.Range(0.0f, 3.0f), System.DateTime.Now.ToUniversalTime().ToString()));
		gyroData.Add (new GyroscopeReading(Random.Range(0.0f, 3.0f),Random.Range(0.0f, 3.0f),Random.Range(0.0f, 3.0f), System.DateTime.Now.ToUniversalTime().ToString()));

		emgData.max = 5f;
		emgData.min = -5f;
		emgData.data.Add(new EMGReading(Random.Range(-5f, 5f), System.DateTime.Now.ToUniversalTime().ToString() ));

		nisData.max = 5f;
		nisData.min = -5f;
		nisData.data.Add(new NISReading(Random.Range(-5f, 5f), System.DateTime.Now.ToUniversalTime().ToString() ));
	}

	public void saveData(){
		
		CancelInvoke ();


		string accel_data = "[", gyro_data = "[";

		//Ensure that trailing comma is removed

		if(accelData.Count > 0){
			
			accel_data+= "{%time%:%" + accelData[0].time_stamp + "%,%x%:" + accelData[0].x + ",%y%:" + accelData[0].y + ",%z%:" + accelData[0].z + "}";

				for (int i = 1; i < accelData.Count; i++) {
				accel_data+= ",{%time%:%" + accelData[i].time_stamp + "%,%x%:" + accelData[i].x + ",%y%:" + accelData[i].y + ",%z%:" + accelData[i].z + "}";
				}
		}



		accel_data+= "]";

		if(gyroData.Count > 0){

			gyro_data+= "{%time%:%" + gyroData[0].time_stamp + "%,%x%:" + gyroData[0].x + ",%y%:" + gyroData[0].y + ",%z%:" + gyroData[0].z + "}";

			for (int i = 1; i < gyroData.Count; i++) {
				gyro_data+= ",{%time%:%" + gyroData[i].time_stamp + "%,%x%:" + gyroData[i].x + ",%y%:" + gyroData[i].y + ",%z%:" + gyroData[i].z + "}";
			}

		}


		gyro_data+= "]";


		Debug.Log (gyro_data);
		Debug.Log (accel_data);

		string json = @"{
		'step_id':'" 
			+ mainController.currentTask.steps[currentStep].step.step_id + @"',
		'performance_record_id':'" 
			+ p.performance_record_id + @"',
		'accelerometer_data':'" + accel_data + @"',
		'gyroscope_data':'" + gyro_data + "'}";




		json = json.Replace("'", "\"");

		Debug.Log (json);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8081/api/performance/save/hand_position_assessment_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

		//***************************************

		string emg_data = "{%min%:" + emgData.min + ",%max%:" +  emgData.max + ", %time_series%:[";
		string nis_data = "{%min%:" + emgData.min + ",%max%:" +  emgData.max + ", %time_series%:[";


		if(emgData.data.Count > 0){

			emg_data+="{%time%:%" + emgData.data[0].time_stamp + "%,%amplitude%: " + emgData.data[0].amplitude + "}";

			for (int i = 1; i < emgData.data.Count; i++) {
				emg_data+=",{%time%:%" + emgData.data[i].time_stamp + "%,%amplitude%: " + emgData.data[i].amplitude + "}";
			}

		}

		if(nisData.data.Count > 0){

			nis_data+="{%time%:%" + nisData.data[0].time_stamp + "%,%amplitude%: " + nisData.data[0].amplitude + "}";

			for (int i = 1; i < nisData.data.Count; i++) {
				nis_data+=",{%time%:%" + nisData.data[i].time_stamp + "%,%amplitude%: " + nisData.data[i].amplitude + "}";
			}

		}


		emg_data+= "]}";
		nis_data+= "]}";

		Debug.Log (emg_data);
		Debug.Log (nis_data);


		json = @"{
		'step_id':'" 
			+ mainController.currentTask.steps[currentStep].step.step_id + @"',
		'performance_record_id':'"
			+ p.performance_record_id + @"',
		'electromyograph_data':'" + emg_data + @"'}";

		json = json.Replace("'", "\"");

		Debug.Log (json);

		postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		www = new WWW("http://localhost:8081/api/performance/save/hand_muscle_assessment_record", postData, headers);

		StartCoroutine(WaitForRequest(www));


		json = @"{
		'step_id':'" 
			+ mainController.currentTask.steps[currentStep].step.step_id + @"',
		'performance_record_id':'" 
			+ p.performance_record_id + @"',
		'cerebral_cortex_data':'" + nis_data + @"'}";

		json = json.Replace("'", "\"");

		Debug.Log (json);

		postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		www = new WWW("http://localhost:8081/api/performance/save/brain_concentration_record", postData, headers);

		StartCoroutine(WaitForRequest(www));
	
		

		emgData.data = new List<EMGReading>();
		nisData.data = new List<NISReading>();

		initRecordingData();


	}

	//Set current performance profile 
	public IEnumerator SetProfileDeviceAssociationStep(int step_id){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/device/set/profile/current/step?association_id=" + mainController.DeviceProfileAssociationID + "&step_id=" + step_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {


		}
	}

	//Set current performance profile 
	public IEnumerator GenerateReport(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/performance/generate/report?performance_record_id=" + p.performance_record_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {


			Debug.Log (www.downloadHandler.text);

			r = JsonUtility.FromJson<Report> (www.downloadHandler.text);

			reportContent.GetComponent<Text> ().text =
				"Total time: " + Mathf.Round(r.actual_time) + "\n" +
				"Steps completed: " + r.number_of_steps_completed + " of " + r.number_of_steps + "\n" + 
				"Completion Rating: " + Mathf.Round(r.completion_rating) +
				" \n Percentage of task completed: " + r.percentage_of_task_completed + " \n\n Step summary \n" + "_______________________________";

			for (int i = 0; i < r.step_completion_report.Length; i++) {

				reportContent.GetComponent<Text> ().text += "\n" + (r.step_completion_report [i].step_index + 1) + ". " + r.step_completion_report [i].step_instruction; 
				reportContent.GetComponent<Text> ().text += "\n     Actual Time: " + Mathf.Round(r.step_completion_report [i].actual_time); 
				reportContent.GetComponent<Text> ().text += "\n     Expected Time: " + Mathf.Round(r.step_completion_report [i].expect_time); 
				reportContent.GetComponent<Text> ().text += "\n     Accuracy: " + r.step_completion_report [i].completion_rating; 

			}


			Debug.Log (r.percentage_of_task_completed + "");


		}
	}




	//Set current performance profile 
	public IEnumerator SetProfileDeviceAssociationRecordId(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/device/set/profile/performance/record?association_id=" + mainController.DeviceProfileAssociationID + "&performance_record_id=" + p.performance_record_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {


		}
	}


	[System.Serializable]
	public class Report {
		public float percentage_of_task_completed, expect_time, actual_time, completion_rating;
		public int number_of_steps, number_of_steps_completed; 
		public string task_name;

		public StepReport[] step_completion_report;
		public Report() {}
	}

	[System.Serializable]
	public class StepReport {
		public float completion_rating, expect_time, actual_time;
		public string step_instruction;
		public int step_id, step_index;
		public StepReport() {}
	}



}
