using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/*
 * 
 * Author: Saboor Salaam 7/12/18
 * 
 * 
 * 
*/

//Add comments to json file itself!!!
//Handle CAMMRAD properties 
public class CAMMRADPropertiesManager : MonoBehaviour {

	//name of properties file
	string file_name = "cammrad_properties.json";

	CAMMRADProperties properties = new CAMMRADProperties();

	//Unity method called when object loads
	void Start(){
		LoadPropertiesFile();
	}


	//Load properties json file and read into object
	private void LoadPropertiesFile()
	{
		// Path.Combine combines strings into a file path
		// Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
		string filePath = Path.Combine(Application.streamingAssetsPath, file_name);

		Debug.Log(Application.streamingAssetsPath);


		//Check if file exists
		if(File.Exists(filePath))
		{
			// Read the json from the file into a string
			string dataAsJson = File.ReadAllText(filePath); 

			Debug.Log ("Reading properties file...");
			Debug.Log (dataAsJson);

			// Pass the json to JsonUtility, and tell it to create a GameData object from it
			properties = JsonUtility.FromJson<CAMMRADProperties>(dataAsJson);
		}
		else
		{
			Debug.LogError("Cannot load properties!");
		}
	}

	//Retrieve a property by key from properties file 
	public string getProperty(string key){

		for (int i = 0; i < properties.properties.Length; i++) {

			if (properties.properties[i].key == key) {
				return properties.properties [i].value;
			}
		}

		return "";

	}

	//Return path where videos should be stored
	public string getVideosDirectoryPath(){
		return Application.persistentDataPath + getProperty ("video_path");
	}


	[System.Serializable]
	public class CAMMRADProperties {
		public CAMMRADProperty[] properties;
		public CAMMRADProperties() {}
	}

	[System.Serializable]
	public class CAMMRADProperty {
		public string key, value;
		public CAMMRADProperty() {}
	}



}
