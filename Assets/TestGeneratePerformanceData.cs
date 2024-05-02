using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGeneratePerformanceData : MonoBehaviour {

	Dictionary<string,string> headers = new Dictionary<string, string>();

	void Start () {
		headers.Add( "Content-Type", "application/json");
	}

	public void SimulatePostData(){

		for (int i = 0; i < 100; i++) {
			saveHandMuscleRecord ();
			saveHandPositionRecord ();
			savePsychoMotorAssessmentRecord ();
			saveConversationRecord();
			saveBrainMemoryRecord();
			saveBrainConcentrationRecord();
			Debug.Log(i);
		}

	}


	public void saveHandMuscleRecord(){

		string json = @"{
		'performance_record_id':'2',
		'step_id':'2',
		'electromyograph_data': '{start_time:2113343, end_time: 232455, min:10,max:100,series: [{time:738834834,value:30},{time:738834834,value:30},{time:738834834,value:30},{time:738834834,value:30}]}'
		}";

		json = json.Replace("'", "\"");

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8080/api/performance/save/hand_muscle_assessment_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

	}

	public void saveHandPositionRecord(){

		string json = @"{
		'performance_record_id':'2',
		'step_id':'2',
		'gyroscope_data': '{start_time:2113343, end_time: 232455, series: [{time:738834834,x:30,y:12,z:23},{time:738834834,x:30,y:12,z:23},{time:738834834,x:30,y:12,z:23},{time:738834834,x:30,y:12,z:23}]}',
		'accelerometer_data': '{start_time:2113343, end_time: 232455, series: [{time:738834834,x:30,y:12,z:23},{time:738834834,x:30,y:12,z:23},{time:738834834,x:30,y:12,z:23},{time:738834834,x:30,y:12,z:23}]}'
		}";

		json = json.Replace("'", "\"");

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8080/api/performance/save/hand_position_assessment_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

	}

	public void savePsychoMotorAssessmentRecord(){

		string json = @"{
		'performance_record_id':'2',
		'step_id':'135',
		'duration': '56',
		'accuracy': '0.9',
		'completed':'true'
		}";

		json = json.Replace("'", "\"");

		byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8080/api/performance/save/psychomotor_assessment_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

	}


	public void saveConversationRecord(){

		string json = @"{
		'performance_record_id':'2',
		'recorded_data': '{start_time:2113343, end_time: 232455, series: [{query:sunu load task BVM,response:ok loading task,time_stamp:126374},{query:sunu next,response:ok,time_stamp:126374},{query:sunu back,response:ok,time_stamp:126374}]}'
		}";

		json = json.Replace("'", "\"");

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request
		WWW www = new WWW("http://localhost:8080/api/performance/save/conversation_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

	}

	public void saveBrainMemoryRecord(){

		string json = @"{
		'performance_record_id':'2',
		'step_id':'2',
		'hippocampus_data': '{start_time:2113343, end_time: 232455, min:10,max:100,series: [{time:738834834,value:30},{time:738834834,value:30},{time:738834834,value:30},{time:738834834,value:30}]}'
		}";
		

		json = json.Replace("'", "\"");

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request

		WWW www = new WWW("http://localhost:8080/api/performance/save/brain_memory_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

	}

	public void saveBrainConcentrationRecord(){

		string json = @"{
		'performance_record_id':'2',
		'step_id':'2',
		'cerebral_cortex_data': '{start_time:2113343, end_time: 232455, min:10,max:100,series: [{time:738834834,value:30},{time:738834834,value:30},{time:738834834,value:30},{time:738834834,value:30}]}'
		}";


		json = json.Replace("'", "\"");

		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		//Now we call a new WWW request

		WWW www = new WWW("http://localhost:8080/api/performance/save/brain_concentration_record", postData, headers);

		StartCoroutine(WaitForRequest(www));

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
