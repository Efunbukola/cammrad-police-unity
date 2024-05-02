using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;


public class scribar_authentication_controller : MonoBehaviour {

	public InputField input, input2;
	public GameObject errorText;

	// Use this for initialization
	void Start () {

		//PlayerPrefs.SetString ("product_key", "wdwdwd");

		errorText.SetActive(false);


		if (PlayerPrefs.HasKey ("product_key")) {
			if (!PlayerPrefs.GetString ("product_key").Equals ("")) {
				Debug.Log ("Product key is saved");
				StartCoroutine(ValidateKey(PlayerPrefs.GetString ("product_key"), OnValidatedKey));
			}
		} else {
			Debug.Log ("Product key is NOT saved");
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnValidatedKey(bool isValid){
		Debug.Log ("Was valid = " + isValid);

		if (isValid) {
			SceneManager.LoadScene("ProfileAuthScene", LoadSceneMode.Single);
			//gameObject.SetActive (false);
		} else {
			//gameObject.SetActive(true);
			errorText.SetActive(true);

		}

	}

	public void OnSubmit(){
		Debug.Log (input.textComponent.text);
		StartCoroutine(Register(input.textComponent.text, input2.textComponent.text, OnValidatedKey));
	}


	//Posibly one method for just validing key and one for first time registration 

	public IEnumerator Register(string key, string device_name, Action<bool> onComplete){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/device/register?product_key=" + key + "&device_name=" + device_name + "&device_id=" + SystemInfo.deviceUniqueIdentifier);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log ("http://localhost:8080/api/device/register?product_key=" + key + "&device_name=" + device_name + "&device_id=" + SystemInfo.deviceUniqueIdentifier);
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
			onComplete(false);
		} else {
			Debug.Log (www.downloadHandler.text);
			PlayerPrefs.SetString ("product_key", key);
			onComplete(true);
		}
	}

	public IEnumerator ValidateKey(string key, Action<bool> onComplete){

		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/account/verify/license?product_key=" + key);
		yield return www.Send();

		if (www.isHttpError) {
			Debug.Log (www.error);
			Debug.Log (www.responseCode);
			onComplete(false);
		} else {
			Debug.Log (www.downloadHandler.text);
			PlayerPrefs.SetString ("product_key", key);
			onComplete(true);
		}
	}

}
