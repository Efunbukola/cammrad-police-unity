using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CAMMRADMainController : MonoBehaviour {


	public const int NOVICE = 1, INTER = 2, EXPERT = 3;

	public int skill_level;

	public GameObject validateKeyView;

	public GameObject CAMMRADPM_Normal, CAMMRADPM_Expert, CAMMRADPM_Collage, Timer, TaskList, cutOffMarker, centerText, quitButton, showTextToggle, taskHeading, nameHeading;
	public bool isWorkflowInit = false;

	public GameObject AISuggesstion;

	public CAMMRADWorkflowTask currentTask;
	public CAMMRADWorkflow workflow;

	public int DeviceProfileAssociationID = 0;


	void Start(){

		PlayerPrefs.SetString("performance_profile_id", "51");
		skill_level = 1; //PlayerPrefs.GetInt("skill_level", NOVICE);
		//Debug.Log ("Skill level is " + PlayerPrefs.GetInt("skill_level", NOVICE));



		//Calculate correct position for PM (half of circle hidden)
		CAMMRADPM_Normal.GetComponent<RectTransform> ().localPosition = new Vector3 (((Screen.width / 2) - cutOffMarker.transform.localPosition.x), CAMMRADPM_Normal.GetComponent<RectTransform> ().localPosition.y, CAMMRADPM_Normal.GetComponent<RectTransform> ().localPosition.z);

		CAMMRADPM_Normal.SetActive (false);
		Timer.SetActive (false);
		centerText.SetActive (false);   


		GetWorkflow();

		//initWorkflow ("{\"workflow\":{\"workflow_id\":101,\"workflow_type_id\":0,\"workflow_name\":\"CAMMRAD Construction\",\"workflow_description\":\"sample\",\"cover_img\":\"\",\"company_id\":13,\"is_active\":false,\"createdAt\":null,\"handler\":{},\"hibernateLazyInitializer\":{}},\"tasks\":[{\"task\":{\"task_id\":171,\"workflow_id\":101,\"task_name\":\"Weatherization: Caulk Window \",\"task_description\":\"this is a sample\",\"createdAt\":1541608555000,\"expected_time\":1},\"steps\":[{\"step\":{\"step_id\":3443691,\"task_id\":171,\"action_id\":0,\"step_index\":0,\"step_title\":\"Remove old caulk from around the window frames\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443691,\"icon_title\":\"icon_2018-11-07115354977\",\"file_path\":\"Caulking_Step-01\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443701,\"task_id\":171,\"action_id\":0,\"step_index\":1,\"step_title\":\"Prepare surface of the window by removing any dirt, debris, and old paint that will hinder adhesion\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[],\"icon\":{\"step_id\":3443701,\"icon_title\":\"icon_2018-11-07122005538\",\"file_path\":\"Caulking_Step-02\"}},{\"step\":{\"step_id\":3443711,\"task_id\":171,\"action_id\":0,\"step_index\":2,\"step_title\":\"Wash the area, but make sure it is dry before caulk is applied\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443711,\"icon_title\":\"icon_2018-11-07122042665\",\"file_path\":\"Caulking_Step-03\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443721,\"task_id\":171,\"action_id\":0,\"step_index\":3,\"step_title\":\"Place tube in the caulk gun\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443721,\"icon_title\":\"icon_2018-11-07122049635\",\"file_path\":\"Caulking_Step-04\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443731,\"task_id\":171,\"action_id\":0,\"step_index\":4,\"step_title\":\"Cut the tip of the tube\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443731,\"icon_title\":\"icon_2018-11-07122058118\",\"file_path\":\"Caulking_Step-05\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443741,\"task_id\":171,\"action_id\":0,\"step_index\":5,\"step_title\":\"Press gun tip firmly against one corner of the window and apply caulk to the next corner\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":420},\"icon\":{\"step_id\":3443741,\"icon_title\":\"icon_2018-11-07122122754\",\"file_path\":\"Caulking_Step-06\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443751,\"task_id\":171,\"action_id\":0,\"step_index\":6,\"step_title\":\"Use an object to smooth over the sealant and remove excess caulk\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443751,\"icon_title\":\"icon_2018-11-07122127419\",\"file_path\":\"Caulking_Step-07\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]}]}]}");
		//Parse workflow json
		//initWorkflow ("{\"name\":\"Workflow1\",\"type\":\"medic\",\"tasks\":[{\"name\":\"BVM\",\"dir_name\":\"BVM\",\"steps\":[{\"StepDescription\":\"Position patient & maintain proper airway.\",\"StepImageAid\":\"1\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Select proper mask size.\",\"StepImageAid\":\"2\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Seal mask to face.\",\"StepImageAid\":\"3\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Ventilate the patient.\",\"StepImageAid\":\"4\",\"StepVideoAid\":\"somepath/video.mp4\"}]},{\"name\":\"EMERGENCY BANDAGE\",\"dir_name\":\"Emergency_Bandage\",\"steps\":[{\"StepDescription\":\"Take body substance isolation.\",\"StepImageAid\":\"1\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Expose the wound.\",\"StepImageAid\":\"2\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Pack the wound with gauze. Gauze should extend 1-2 inches above the skin.\",\"StepImageAid\":\"3\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\" Place porti.on of the appropriate size dressing down covering all of the wound \",\"StepImageAid\":\"4\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Securely wrap the elastic portion of the bandage around the extremity.\",\"StepImageAid\":\"5\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Continue to wrap the wound tightly ensuring all edges of the wound pad are covered.\",\"StepImageAid\":\"8\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Secure the closure bar to the bandage.\",\"StepImageAid\":\"9\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Loosen the tourniquet. (Do not remove from the limb)\",\"StepImageAid\":\"10\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Secure the bandage with tape.\",\"StepImageAid\":\"11\",\"StepVideoAid\":\"somepath/video.mp4\"}]},{\"name\":\"COMBAT APPLICATION TOURNIQUET\",\"dir_name\":\"Tourniquet\",\"steps\":[{\"StepDescription\":\"Take body substance isolation.\",\"StepImageAid\":\"1\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Expose the wound.\",\"StepImageAid\":\"2\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Route the band around the limb above the wound on the injured extremity.\",\"StepImageAid\":\"3\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Pass the red tip through the inside slit in the buckle, and position the CAT 2-3 inches above the wound, and directly on the skin.\",\"StepImageAid\":\"4\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Pull the band as tight as possible and secure the Velcro back on itself all the way around the limb, but not over the rod clips.\",\"StepImageAid\":\"5\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Twist the windlass until the bleeding stops. (Should occur within 3 rotations of the windlass)\",\"StepImageAid\":\"6\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Secure the windlass rod inside the windlass clip to lock it into place.\",\"StepImageAid\":\"7\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Check for distal pulse.\",\"StepImageAid\":\"8\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"If possible, continue to route the self-adhering band between the windlass clips and over the windlass rod. Secure the rod and band with the windlass strap.\",\"StepImageAid\":\"9\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Place a \\\"T\\\" and the time of application on the casualty.\",\"StepImageAid\":\"10\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Secure the CAT in place with tape.\",\"StepImageAid\":\"11\",\"StepVideoAid\":\"somepath/video.mp4\"}]},{\"name\":\"ADMINISTER NALOXONE\",\"dir_name\":\"NALAXONE\",\"steps\":[{\"StepDescription\":\"Do rescue breathing for a few quick breaths if the person is not breathing.\",\"StepImageAid\":\"1\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Affix the nasal atomizer to the needleless syringe and then assemble the glass cartridge of naloxone.\",\"StepImageAid\":\"2\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Tilt the head back and spray half of the naloxone up one side of the nose (1cc) and half up the other side of the nose (1cc).\",\"StepImageAid\":\"3\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"If there is no breathing or breathing continues to be shallow, continue to perform rescue breathing for them while waiting for the naloxone to take effect.\",\"StepImageAid\":\"4\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"If there is no change in 3-5 minutes, administer another dose of naloxone and continue to breathe for them\",\"StepImageAid\":\"5\",\"StepVideoAid\":\"somepath/video.mp4\"}]},{\"name\":\"NEEDLE DECOMPRESSION\",\"dir_name\":\"NEEDLE\",\"steps\":[{\"StepDescription\":\"Clean the site.\",\"StepImageAid\":\"1\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Insert needle.\",\"StepImageAid\":\"2\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Administer Catheter.\",\"StepImageAid\":\"3\",\"StepVideoAid\":\"somepath/video.mp4\"},{\"StepDescription\":\"Listen for Breathing.\",\"StepImageAid\":\"4\",\"StepVideoAid\":\"somepath/video.mp4\"}]}]}");


		//Show list of workflow tasks

		//initWorkflow ("{\"workflow\":{\"workflow_id\":101,\"workflow_type_id\":0,\"workflow_name\":\"CAMMRAD Construction\",\"workflow_description\":\"sample\",\"cover_img\":\"\",\"company_id\":13,\"is_active\":false,\"createdAt\":null,\"handler\":{},\"hibernateLazyInitializer\":{}},\"tasks\":[{\"task\":{\"task_id\":171,\"workflow_id\":101,\"task_name\":\"Needle Decompression\",\"task_description\":\"this is a sample\",\"createdAt\":1541608555000,\"expected_time\":1},\"steps\":[{\"step\":{\"step_id\":3443691,\"task_id\":171,\"action_id\":0,\"step_index\":0,\"step_title\":\"Prepare area with an antiseptic solution\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443691,\"icon_title\":\"icon_2018-11-07115354977\",\"file_path\":\"S1\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443691,\"task_id\":171,\"action_id\":0,\"step_index\":1,\"step_title\":\"  Insert needle over the top of the rib, at a 90 degree angle to the chest wall, to the hub.\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443691,\"icon_title\":\"icon_2018-11-07115354977\",\"file_path\":\"S2\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443691,\"task_id\":171,\"action_id\":0,\"step_index\":2,\"step_title\":\"Secure the catheter hub to chest.\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443691,\"icon_title\":\"icon_2018-11-07115354977\",\"file_path\":\"S3\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]},{\"step\":{\"step_id\":3443691,\"task_id\":171,\"action_id\":0,\"step_index\":3,\"step_title\":\"Verbalize continued reassessment of casualty for reoccurrence of respiratory distress\",\"object_name\":null,\"videoUrl\":null,\"completion_response\":null,\"expected_time\":60},\"icon\":{\"step_id\":3443691,\"icon_title\":\"icon_2018-11-07115354977\",\"file_path\":\"S4\"},\"aids\":[],\"conditions\":[],\"routes\":[],\"overlays\":[],\"overlays_raw\":null,\"ui_images\":[],\"ui_texts\":[]}]}]}");



	}
	void Update(){


		if(Input.GetKeyDown(KeyCode.H)){
			showSequence ("");
		} 


		if(Input.GetKeyDown(KeyCode.F)){
			hideSequence ("");
			finishTask ();
		} 


	}

	//Called by android activity to store Workflow data at beginning of application
	public void initWorkflow(string json_string) {

		workflow = CAMMRADWorkflowParserFactory.getParser(CAMMRADWorkflowParserFactory.JSON).parse(json_string);
		isWorkflowInit = true;

		//Debug.Log (\"Tasks parsed:\" + workflow.tasks.Length);

		showTaskList();

	}

	public void showTaskList(){
		
		TaskList.SetActive(true);
		quitButton.SetActive(false);
		//showTextToggle.SetActive(false);
	}

	public void hideTaskList(){
		TaskList.SetActive(false);
	}

	public void takeScreenshot(){
		
		ScreenCapture.CaptureScreenshot ("screenshot.png");

		Debug.Log (Application.persistentDataPath);

		Invoke ("uploadScreenshot", 25);

	}

	//Pass screenshot to android class
	public void uploadScreenshot(){

		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");

		AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");

		//Combine data path to generate absolute path
		mainActivity.Call ("saveScreenshot", Application.persistentDataPath + "/screenshot3435.png");

	}

	public void showSuggestion(string id){
		AISuggesstion.GetComponent<ai_suggestions_script>().showSuggestion(id);
	}

	//Called by android activity to restart PM showing a new task
	public void setCurrentTask(string index){

		//AISuggesstion.GetComponent<ai_suggestions_script>().showSuggestion("EMPATHY");
		
		//showSuggestion("EMPATHY");

		Debug.Log ("Setting current task " + index);
		int i = int.Parse (index);

		if (i < workflow.tasks.Length && i >= 0) {
			currentTask = workflow.tasks [i];
			Debug.Log ("current task set");
		} else {
			return;
		}


		if (workflow.tasks[i].steps.Length < 1) {
			return;
		}

		taskHeading.GetComponent<Text>().text = "Task: " + workflow.tasks [i].task.task_name;
		hideTaskList();
		quitButton.SetActive(true);

		switch (skill_level){ // {
		case NOVICE:
			CAMMRADPM_Normal.SetActive(false);
			CAMMRADPM_Normal.SetActive(true);
			centerText.SetActive (true);

			AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
			mainActivity.Call ("startTask");

			break;
		case INTER:
			CAMMRADPM_Normal.SetActive(false);
			CAMMRADPM_Normal.SetActive(true);
			centerText.SetActive (false);

			break;

		case EXPERT:
			CAMMRADPM_Expert.SetActive (false);
			CAMMRADPM_Expert.SetActive (true);
			CAMMRADPM_Expert.GetComponent<sample_expert_ui> ().Init ();
			centerText.SetActive (false);

			//mainActivity.Call (\"OnStartTaskAsExpert\", \"\");

			break;
		case 5:
			CAMMRADPM_Expert.SetActive(false);
			CAMMRADPM_Expert.SetActive(false);
			centerText.SetActive(false);

			CAMMRADPM_Collage.SetActive(true);
			CAMMRADPM_Collage.GetComponent<Sample_collage_ui>().Init();

			//mainActivity.Call (\"OnStartTaskAsCollage\", \"\");

			break;


		default:
			CAMMRADPM_Normal.SetActive(false);
			CAMMRADPM_Normal.SetActive(true);
			centerText.SetActive(true);

			break;
		}

		//Restart Timer
		//Timer.SetActive (true);
		//Timer.GetComponent<timer_controller>().startTask();

		//Hide task list
		
		//showTextToggle.SetActive(true);


	}


	public void showSequence(string dummy){
		CAMMRADPM_Collage.GetComponent<Sample_collage_ui> ().showNumbers();
	}

	public void hideSequence(string dummy){
		CAMMRADPM_Collage.GetComponent<Sample_collage_ui> ().hideNumbers();
	}

	public void requestAssitance(string current_step){
				
			CAMMRADPM_Expert.SetActive(false);
			CAMMRADPM_Normal.SetActive(true);
			CAMMRADPM_Normal.GetComponent<CAMMRADPMController> ().changeCurrentItemForAssistence(int.Parse(current_step));

	}

	public void resumeExpertInterface(){

		CAMMRADPM_Expert.SetActive(true);
		CAMMRADPM_Normal.SetActive(false);

	}


	public void finishTask(){

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		mainActivity.Call ("endTask");


		//StartCoroutine (SetProfileDeviceAssociationInActive());

		//taskHeading.GetComponent<Text> ().text = "";

		//Check whether or not we are on the last step
		//if (CAMMRADPM_Normal.GetComponent<CAMMRADPMController> ().currentStep < currentTask.steps.Length - 2) {
			//return;
		//}

		//CAMMRADPM_Normal.SetActive(false);
		//CAMMRADPM_Expert.SetActive(false);

		//centerText.SetActive (true);

		//float total_time = Timer.GetComponent<timer_controller> ().endTask();

		//GetComponent<LogFileCreator> ().writeToFile ("TOTAL TASK TIME: " + total_time);
		//GetComponent<LogFileCreator> ().closeFile ();


		//Timer.SetActive (false);

		//CAMMRADPM_Normal.GetComponent<CAMMRADPMController> ().CreateReport();

		//taskHeading.GetComponent<Text> ().text = "";

		//Debug.Log ("Total task time " + total_time);


	}



	//Factory
	public class CAMMRADWorkflowParserFactory {
		
		public const int JSON =  0, XML = 1;

		public static CAMMRADWorkflowParser getParser(int type){

			switch (type) {

			case JSON:

				return new CAMMRADWorkflowJSONParser();

				break;

			case XML:

				return new CAMMRADWorkflowXMLParser();

				break;

			default:

				return new CAMMRADWorkflowJSONParser();

			}

		}

	}


	public class CAMMRADWorkflowJSONParser : CAMMRADWorkflowParser {

		public CAMMRADWorkflow parse(string workflow_string){

			return JsonUtility.FromJson<CAMMRADWorkflow>(workflow_string); 

		}

	}


	public class CAMMRADWorkflowXMLParser : CAMMRADWorkflowParser {

		public CAMMRADWorkflow parse(string workflow_string){

			return new CAMMRADWorkflow ();

		}

	}


	public interface CAMMRADWorkflowParser{

		CAMMRADWorkflow parse(string workflow_string);
	}


	/*
	[System.Serializable]
	public class CAMMRADWorkflow {
		 public string name, type;
		 public CAMMRADWorkflowTask[] tasks;
		 public CAMMRADWorkflow() {}
	}

	[System.Serializable]
	public class CAMMRADWorkflowTask {
		  public string name;
		  public string dir_name;
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

*/

	 [System.Serializable]
	public class CAMMRADWorkflow {
		public string workflow_name, workflow_description;
		public int workflow_id;
		 public CAMMRADWorkflowTask[] tasks;
		 public CAMMRADWorkflow() {}
	}

	[System.Serializable]
	public class CAMMRADWorkflowTask {
		public CAMMRADWorkflowSubTask task;
		  public CAMMRADWorkflowStep[] steps;
		  public CAMMRADWorkflowTask() {
			  }
	}

	[System.Serializable]
	public class CAMMRADWorkflowSubTask {
		public string task_name, task_description;
		public int task_id;
		public CAMMRADWorkflowSubTask() {
		}
	}

	[System.Serializable]
	public class CAMMRADWorkflowStep {
	public CAMMRADWorkflowSubStep step;
	public CAMMRADStepCondition[] conditions;
	public CAMMRADStepRoute[] routes;
	public CAMMRADStepAid[] aids;
	public CAMMRADStepIcon icon;
	public CAMMRADStepOverlayObject[] overlays;
	public CAMMRADStepUIImage[] ui_images;
	public CAMMRADStepUIText[] ui_texts;
	public CAMMRADWorkflowStep() {}
	}

	[System.Serializable]
	public class CAMMRADWorkflowSubStep {
		public string step_title, videoUrl, completion_response;
		public float expected_time;
		public int step_id, step_index;
	public CAMMRADWorkflowSubStep() {}
		}

	[System.Serializable]
	public class CAMMRADStepAid {
	public string aid_title, file_path;
	public int aid_id;
	public CAMMRADStepAid() {}
			}


	[System.Serializable]
	public class CAMMRADStepRoute {
	public string condition_result;
	public int result_step_index;
	public CAMMRADStepRoute() {}
				}

	[System.Serializable]
	public class CAMMRADStepCondition {
	public string condition_query;
	public int condition_index;
	public CAMMRADStepCondition() {}
					}

	[System.Serializable]
	public class CAMMRADStepIcon {
	public string icon_title, file_path;
	public CAMMRADStepIcon() {}
						}

	[System.Serializable]
	public class CAMMRADStepOverlayObject {
		public string object_name, animation_name;
		public float x, y, z;
		public CAMMRADStepOverlayObject() {}
	}

	[System.Serializable]
	public class CAMMRADStepUIImage {
		public string imageURL;
		public float x, y, z;
		public CAMMRADStepUIImage() {}
	}

	[System.Serializable]
	public class CAMMRADStepUIText {
		public string text;
		public float x, y, z, scale;
		public CAMMRADStepUIText() {}
	}


	public void printDummyMessage(){

		Debug.Log ("Something clicked");

	}



	//Retrieve workflow from local storage or server
	public void GetWorkflow(){

		//Get workflow
		GetComponent<CAMMRADCacheManager> ().loadWorkflow ((json) => {

			//parse workflow json and store object
			initWorkflow (json);//.Replace("'", "\""));

			Debug.Log (json);


			//Loop through tasks and steps to download videos and icons for every step
			foreach (CAMMRADWorkflowTask task in workflow.tasks) {

				foreach (CAMMRADWorkflowStep step in task.steps) {

					//If there is a network connection
					if(Application.internetReachability != NetworkReachability.NotReachable){
						
					//Debug.Log("Attempting to save: " + step.step.videoUrl);

					//Download and save video
					//GetComponent<CAMMRADCacheManager>().saveVideo(step.step.videoUrl);

					//Debug.Log("Attempting to save icon: " + step.step.step_id.ToString());
					
					//Icon getting code needs to change too!!!!!!!!

				    //Download and save icon
					//GetComponent<CAMMRADCacheManager>().saveIcon (step.step.step_id.ToString());

					}

				}

			}

		});



	}



	//Set current performance profile 
	public IEnumerator SetProfileDeviceAssociation(int i){


		Debug.Log ("SetProfileDeviceAssociation");
		// + PlayerPrefs.GetString("performance_profile_id") 
		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/device/set/profile?profile_id=51&device_id=" + SystemInfo.deviceUniqueIdentifier 
			+ "&task_name=" + currentTask.task.task_name + "&skill_level=" + skill_level + "&step_id=" + currentTask.steps[0].step.step_id + "&task_id=" + currentTask.task.task_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {

			Debug.Log ("SetProfileDeviceAssociation success!!");

			Debug.Log (www.downloadHandler.text);

			DeviceProfileAssociation d = JsonUtility.FromJson<DeviceProfileAssociation> (www.downloadHandler.text);

			DeviceProfileAssociationID = d.association_id;

			PlayerPrefs.SetInt ("current_association_id", DeviceProfileAssociationID);

			Debug.Log (DeviceProfileAssociationID);



			//Add bar indicating which step you are currently on similair to bar with the cursor 
			//Indication of how many steps remain
			//Indication of which steps have been completed
			//Add step assistance


			//AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			//AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");

			/*
			skill_level = 1;

			switch (skill_level){ // {
			case NOVICE:
				CAMMRADPM_Normal.SetActive(false);
				CAMMRADPM_Normal.SetActive(true);
				centerText.SetActive (true);

				//mainActivity.Call ("OnStartTask", "");

				break;
			case INTER:
				CAMMRADPM_Normal.SetActive(false);
				CAMMRADPM_Normal.SetActive(true);
				centerText.SetActive (false);

				break;

			case EXPERT:
				CAMMRADPM_Expert.SetActive (false);
				CAMMRADPM_Expert.SetActive (true);
				CAMMRADPM_Expert.GetComponent<sample_expert_ui> ().Init ();
				centerText.SetActive (false);

				//mainActivity.Call ("OnStartTaskAsExpert", "");

				break;
			case 5:
				CAMMRADPM_Expert.SetActive (false);
				CAMMRADPM_Expert.SetActive (false);
				centerText.SetActive (false);

				CAMMRADPM_Collage.SetActive (true);
				CAMMRADPM_Collage.GetComponent<Sample_collage_ui> ().Init();

				//mainActivity.Call ("OnStartTaskAsCollage", "");

				break;


			default:
				CAMMRADPM_Normal.SetActive(false);
				CAMMRADPM_Normal.SetActive(true);
				centerText.SetActive (true);

				break;
			}



			//GetComponent<LogFileCreator> ().openNewFile (Application.persistentDataPath + "/" + currentTask.name + (System.DateTime.Now.Hour + "." + System.DateTime.Now.Minute) + ".txt");

			//GetComponent<LogFileCreator> ().writeToFile ("TASK: " + currentTask.name);
			//GetComponent<LogFileCreator> ().writeToFile ("DATE: " + (System.DateTime.Now.ToString()));


			//Restart PM

			//CAMMRADPM_Normal.SetActive(false);
			//CAMMRADPM_Normal.SetActive(true);




			//AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			//AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
			//mainActivity.Call ("OnStartTask", workflow.tasks[i].name);
			//mainActivity.Call ("speak", "Loading task." + workflow.tasks[i].name);
			//mainActivity.Call ("speak", currentTask.steps[CAMMRADPM_Normal.GetComponent<CAMMRADPMController>().currentStep].StepDescription);
			taskHeading.GetComponent<Text> ().text = "Task: " + workflow.tasks [i].task.task_name;


			//Restart Timer
			Timer.SetActive (true);
			Timer.GetComponent<timer_controller>().startTask();

			//Hide task list
			hideTaskList();

			quitButton.SetActive(true);
			//showTextToggle.SetActive(true);
*/


		}
	}

	//Set current performance profile 
	public IEnumerator SetProfileDeviceAssociationInActive(){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/device/set/profile/inactive?association_id=" + DeviceProfileAssociationID);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {
			

		}
	}

	[System.Serializable]
	public class DeviceProfileAssociation {
		public int association_id;
		public DeviceProfileAssociation() {}
	}


	void OnApplicationQuit()
	{

		StartCoroutine (SetProfileDeviceAssociationInActive());
		//Debug.Log("Application ending after " + Time.time + " seconds");


	}

}
