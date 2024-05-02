using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class profile_auth_controller : MonoBehaviour {

	public InputField input;
	public GameObject errorText;


	//Add to one class which encapsulates all different types of authentication ( product key, user profile code, biometric etc.)
	//Research and potentially add base 64 encoding of images
	// Use this for initialization
	void Start () {

		//PlayerPrefs.SetString ("auth_code", "hhhwdwd");

		errorText.SetActive(false);

		if (PlayerPrefs.HasKey ("auth_code")) {
			if (!PlayerPrefs.GetString ("auth_code").Equals ("")) {
				Debug.Log ("Auth code is saved");
				//StartCoroutine(ValidateKey(PlayerPrefs.GetString ("auth_code"), OnValidatedKey));
			}
		} else {
			Debug.Log ("Auth code is NOT saved");
		}


	}

	// Update is called once per frame
	void Update () {

	} 

	public void OnValidatedKey(bool isValid){
		Debug.Log ("Was valid = " + isValid);

		if (isValid) {
			SceneManager.LoadScene("MainScene_Expert", LoadSceneMode.Single);
			//gameObject.SetActive (false);
		} else {
			//gameObject.SetActive(true);
			errorText.SetActive(true);

		}

	}

	public void OnSubmit(){
		Debug.Log (input.textComponent.text);
		StartCoroutine(ValidateKey(input.textComponent.text, OnValidatedKey));
	}

	public IEnumerator GetSkillLevel(string key, Action<bool> onComplete){

		UnityWebRequest www = UnityWebRequest.Get("http://scribar.herokuapp.com/api/profile/get/skill/level/by/code?profile_authentication_code=" + key);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log ("http://scribar.herokuapp.com/api/profile/get/skill/level?profile_authentication_code=" + key);
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
			onComplete(false);
		} else {
			
			Debug.Log (www.downloadHandler.text);
			SkillLevel s = JsonUtility.FromJson<SkillLevel> (www.downloadHandler.text);
			switch (s.name) {
			case "Novice":
				PlayerPrefs.SetInt ("skill_level", CAMMRADMainController.NOVICE);
				break;
			case "Intermediate":
				PlayerPrefs.SetInt ("skill_level", CAMMRADMainController.INTER);
				break;
			case "Expert":
				PlayerPrefs.SetInt ("skill_level", CAMMRADMainController.EXPERT);
				break;
			default:
				PlayerPrefs.SetInt ("skill_level", CAMMRADMainController.NOVICE);
				break;
			}

			onComplete(true);




			//PerformanceProfile p = JsonUtility.FromJson<PerformanceProfile> (www.downloadHandler.text);
			//PlayerPrefs.SetString ("user_full_name", p.first_name + " " + p.last_name);
			//Debug.Log (p.first_name);
		}
	}

	public IEnumerator ValidateKey(string key, Action<bool> onComplete){

		UnityWebRequest www = UnityWebRequest.Get("http://scribar.herokuapp.com/api/profile/verify/code?code=" + key);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log ("http://scribar.herokuapp.com/api/profile/verify/code?code=" + key);
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
			onComplete(false);
		} else {
			Debug.Log ("http://scribar.herokuapp.com/api/profile/verify/code?code=" + key);
			Debug.Log (www.downloadHandler.text);
			PlayerPrefs.SetString ("auth_code", key);
			PerformanceProfile p = JsonUtility.FromJson<PerformanceProfile> (www.downloadHandler.text);
			PlayerPrefs.SetString ("user_full_name", p.first_name + " " + p.last_name);
			PlayerPrefs.SetString ("performance_profile_id", p.performance_profile_id + "");

		
			Debug.Log (p.first_name + p.performance_profile_id);

			//onComplete(true);

			StartCoroutine(GetSkillLevel(PlayerPrefs.GetString ("auth_code"), OnValidatedKey));

		}
	}

	[System.Serializable]
	public class PerformanceProfile {
		public string first_name, last_name;
		public int performance_profile_id;
		public PerformanceProfile() {}
	}


	[System.Serializable]
	public class SkillLevel {
		public string name;
		public SkillLevel() {}
	}

}
