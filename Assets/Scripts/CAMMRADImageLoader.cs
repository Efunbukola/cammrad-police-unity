using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;


/**
 * 
 * Author: Saboor Salaam
 * 
 * **/

public class CAMMRADImageLoader : MonoBehaviour {
		
	public const string PM = "PM", TRACKING= "Tracking";
	public List<TextureIDPair> tids = new List<TextureIDPair>();

	//Load aid
	public Texture2D LoadAid(string aid_id){

		//Check for aid in local storage
		if (PlayerPrefs.HasKey("aid" + aid_id)) {
			
			//Load base 64 string into texture
			byte[] bytes = System.Convert.FromBase64String (PlayerPrefs.GetString ("aid" + aid_id));
			Texture2D tex = new Texture2D(1,1);
			tex.LoadImage(bytes);
			return tex;
		} else {
			
			//Return a blank base 64 image
			byte[] bytes = System.Convert.FromBase64String ("iVBORw0KGgoAAAANSUhEUgAAAsAAAAGMAQMAAADuk4YmAAAAA1BMVEX///+nxBvIAAAAAXRSTlMAQObYZgAAADlJREFUeF7twDEBAAAAwiD7p7bGDlgYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwAGJrAABgPqdWQAAAABJRU5ErkJggg==");
			Texture2D tex = new Texture2D(1,1);
			tex.LoadImage(bytes);
			return tex;

		}

	}

	public Texture2D LoadIcon(string step_id){

		if (PlayerPrefs.HasKey("icon" + step_id) && !PlayerPrefs.GetString("icon" + step_id).Equals("ERROR")) {

			byte[] bytes = System.Convert.FromBase64String (PlayerPrefs.GetString ("icon" + step_id));
			Texture2D tex = new Texture2D(1,1);
			tex.LoadImage(bytes);
			return tex;
		} else {

			byte[] bytes = System.Convert.FromBase64String ("iVBORw0KGgoAAAANSUhEUgAAAsAAAAGMAQMAAADuk4YmAAAAA1BMVEX///+nxBvIAAAAAXRSTlMAQObYZgAAADlJREFUeF7twDEBAAAAwiD7p7bGDlgYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwAGJrAABgPqdWQAAAABJRU5ErkJggg==");
			Texture2D tex = new Texture2D(1,1);
			tex.LoadImage(bytes);
			return tex;

		}

	}

	public void DownloadAid(string aid_id){

		if (PlayerPrefs.HasKey("aid" + aid_id)  && false) {

			return;

		} else {

			StartCoroutine(GetAid(aid_id));

		}

	}


	public void DownloadIcon(string step_id){

		if (PlayerPrefs.HasKey("icon" + step_id)  && !PlayerPrefs.GetString("icon" + step_id).Equals("ERROR") && false) {

			return;

		} else {

			StartCoroutine(GetIcon(step_id));

		}

	}

	public IEnumerator GetAid(string aid_id){

		//Get profile ID from auth user

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/profile/get/aid?aid_id=" + aid_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {
			
			//Debug.Log (www.downloadHandler.text);
			CAMMRADMainController.CAMMRADStepAid aid = JsonUtility.FromJson<CAMMRADMainController.CAMMRADStepAid>((www.downloadHandler.text).Replace("'", "\""));
			PlayerPrefs.SetString ("aid" + aid_id, aid.file_path);
			Debug.Log (PlayerPrefs.GetString ("aid" + aid_id));
		}
	}

	public IEnumerator GetIcon(string step_id){

		//Get profile ID from auth user

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8081/api/profile/get/icon?step_id=" + step_id);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
		} else {

			//Debug.Log (www.downloadHandler.text);
			CAMMRADMainController.CAMMRADStepIcon icon = JsonUtility.FromJson<CAMMRADMainController.CAMMRADStepIcon>((www.downloadHandler.text).Replace("'", "\""));
			PlayerPrefs.SetString ("icon" + step_id, icon.file_path);
			Debug.Log (PlayerPrefs.GetString ("icon" + step_id));
		}
	}

	public Texture2D LoadImage(string type, string task_dir_name, string filename){
	

	Debug.Log (Application.persistentDataPath);
	Texture2D tex = null;
		byte[] fileData;
		Debug.Log ((Application.persistentDataPath + "/Resources/Tasks/" + task_dir_name + "/PM/Images/" + filename));

 
		switch (type) {
		case PM: //Looking for an image related to the PM

			bool found = false; 

			foreach (TextureIDPair t in tids) {

				if (t.id == (task_dir_name + "_" + filename)) {  //if file we are looking for has already been loaded use loaded texture instead

					tex = t.t;
					found = true;

					//Debug.Log ("Was already loaded, loaded from cache instead");


				}

			}


			if (!found) {



				if (File.Exists ((Application.persistentDataPath + "/Resources/Tasks/" + task_dir_name + "/PM/Images/" + filename))) {
					fileData = File.ReadAllBytes ((Application.persistentDataPath + "/Resources/Tasks/" + task_dir_name + "/PM/Images/" + filename));
					tex = new Texture2D (2, 2);
					tex.LoadImage (fileData); 
					tids.Add (new TextureIDPair (tex, task_dir_name + "_" + filename));

					//Debug.Log ("Loaded for first time");


				} else {

					Debug.Log ("file does not exists");

				}

			}


			break;

		default: 
			break;

		}

		return tex;

		}

	public class TextureIDPair{

		public Texture2D t;
		public string id;

		public TextureIDPair(Texture2D t, string id){

			this.t = t;
			this.id = id;
		}


	}

	}

