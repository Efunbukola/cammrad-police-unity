using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;


/*
 * 
 * Author: Saboor Salaam 7/12/18
 * 
 * 
 * 
*/
using UnityEngine.UI;

public class CAMMRADCacheManager : MonoBehaviour {

	public CAMMRADPMController pm;
	public ai_suggestions_script ais;

	void Start(){
	}


	//Load workflow from server or check local storage for workflow in no network connection present
	public void loadWorkflow(System.Action<string> callback){
		StartCoroutine(GetWorkflow(callback));
	}

	//IEnumerator to retrieve workflow from server
	public IEnumerator GetWorkflow(System.Action<string> callback){

		Debug.Log ("https://scribar.herokuapp.com/api/profile/get/cammrad/workflow?workflow_id=424&as_base_64=false"); //PlayerPrefs.GetString ("performance_profile_id")

		//Get profile ID from auth user
		UnityWebRequest www = UnityWebRequest.Get("https://scribar.herokuapp.com/api/profile/get/cammrad/workflow?workflow_id=424&as_base_64=false"); 
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
			callback ("{}");
		} else {

			Debug.Log (www.downloadHandler.text);

			//Store workflow locally
			//PlayerPrefs.SetString (PlayerPrefs.GetString ("performance_profile_id") + "_workflow",www.downloadHandler.text);

			//Return workflow string to callback
			callback (www.downloadHandler.text);

		}
	}


	//Get path of localy stored copy of video 
	public static string getVideoLocalPath(string video_url){

		//Check for refernce to local path in player prefs 
		if (PlayerPrefs.HasKey (video_url)) {
			return PlayerPrefs.GetString (video_url);
		} else {
			//Return empty string if value does not exist in player prefs
			return "";
		}

	}


	//Check for existing video and download if neccessary 
	public void saveVideo(string video_url){


		//Check if video has already been saved already
		if (PlayerPrefs.HasKey (video_url) && false) {

			return;

		} else {


			//Start coroutine to download and save video
			StartCoroutine (downloadVideo (video_url));

		}

	}


	//IEnumerator to download actual video and save locally 
	public IEnumerator downloadVideo(string video_url){

		//Get video 
		UnityWebRequest www = UnityWebRequest.Get(video_url); 
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else if(!String.IsNullOrEmpty(video_url)){
			
			Debug.Log (System.Guid.NewGuid().ToString());

			//Get target path
			string filePath = GetComponent<CAMMRADPropertiesManager> ().getVideosDirectoryPath();

			try
			{	//Check if dir already exists
				if (!Directory.Exists(filePath))
				{
					//Create directory if doesnt exist
					Directory.CreateDirectory(filePath);
				}

			}
			catch (IOException ex)
			{
				Debug.Log (ex.Message);
			}

			//Generate unique file name and combine with path
			string file_name = Path.Combine(filePath, System.Guid.NewGuid().ToString() + "_video.mp4");

			Debug.Log (file_name);

			try {

				//Write downloaded bytes to file 
				File.WriteAllBytes(file_name, www.downloadHandler.data);

				//Store reference from newly saved file to url
				PlayerPrefs.SetString(video_url, file_name);

			}
			catch(Exception e){

			}

		}
	}
		
	//Retrieve icon 
	public void LoadIcon(string file_name){
		pm.centerImage.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(file_name.Trim());
	}

	//Get suggestion icon
	public void LoadSuggestionIcon(string file_name){
		ais.suggestionIcon.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(file_name.Trim());
	}

	public IEnumerator DownloadIconFromUrl(string url)
	{

		WWW www = new WWW (url);

		while (!www.isDone)
		{
			Debug.Log("Download image on progress" + www.progress);
			yield return null;
		}

		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.Log("Download failed");
		}
		else
		{
			Debug.Log("Download success");
			Texture2D texture = new Texture2D (1, 1);
			texture.LoadImage (www.bytes);
			texture.Apply ();
			pm.centerImage.GetComponent<RawImage>().texture = texture;
		}
			

	}


	//Download icon or use stored copy
	public void saveIcon(string step_id){

		//Check for icon in local storage
		if (PlayerPrefs.HasKey("icon" + step_id)  && !PlayerPrefs.GetString("icon" + step_id).Equals("ERROR") && false) {

			return;

		} else {
			//Else download icon
			StartCoroutine(DownloadIcon(step_id));

		}

	}


	//IEnumerator to download base64 encoding of icon from serve
	public IEnumerator DownloadIcon(string step_id){


		UnityWebRequest www = UnityWebRequest.Get("https://scribar.herokuapp.com/api/profile/get/icon?step_id=" + step_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {
			//Capture reponse as icon object
			CAMMRADMainController.CAMMRADStepIcon icon = JsonUtility.FromJson<CAMMRADMainController.CAMMRADStepIcon>((www.downloadHandler.text).Replace("'", "\""));

			Debug.Log(www.downloadHandler.text);

			PlayerPrefs.SetString ("icon" + step_id, icon.file_path);
			Debug.Log (PlayerPrefs.GetString ("icon" + step_id));

		}
	}


}


