using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogFileCreator : MonoBehaviour {

	public System.IO.StreamWriter streamWriter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void openNewFile(string path){
		//streamWriter = new System.IO.StreamWriter (path);
	}

	public void writeToFile(string text){

		if (streamWriter == null)
			return;

		//streamWriter.WriteLine (text);
	}


	public void closeFile(){
		
		if (streamWriter == null)
			return;
		
		//streamWriter.Close();

	}


}


