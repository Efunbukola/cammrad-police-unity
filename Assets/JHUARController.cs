﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


/*
 * Author: John's Hopkins University
 * Created At: 8/01/17
 * 
 * 
 * Updated by: Saboor
 * Updated At: 8/8/18
 * 
 * 
*/

public class JHUARController : ARSetupController {

	public Text DisplayTextFab;
	public Image DisplayImgFab;
	public GameObject Cube, Sphere, Pill;

	private string JsonPath;
	private string jsonResourcesPath;
	private string jsonString;
	public  int currentStep = 0;
	public CAMMRADWorkflow workflow;


	public GameObject Display;
	public GameObject Marker;
	private Text stepName;
	private Text stepNumber;


	public CAMMRADMainController.CAMMRADStepOverlayObject[] featObjects;
	public CAMMRADMainController.CAMMRADStepUIImage[] displayImage;
	public CAMMRADMainController.CAMMRADStepUIText[] displayText;

	public float timeDelayForMultipleObjects = 5.0f;

	private bool hideOverlays = false;

	public List<string> jsonFiles;


	/*
	#if !UNITY_EDITOR && UNITY_METRO
	private static Stream OpenFileForRead( string folderName, string fileName ) {
	Stream stream = null;
	bool taskFinish = false;
	#if !UNITY_EDITOR && UNITY_METRO
	Task task = new Task(
	async () => {
	try {
	StorageFolder folder = await StorageFolder.GetFolderFromPathAsync( folderName );
	var item = await folder.TryGetItemAsync( fileName );
	if( item != null ) {
	StorageFile file = await folder.GetFileAsync( fileName );
	if( file != null ) {
	stream = await file.OpenStreamForReadAsync();
	}
	}
	}
	catch( Exception ) { }
	finally { taskFinish = true; }

	} );
	task.Start();
	while( !taskFinish ) {
	task.Wait();
	}
	#endif
	return stream;
	}
	#endif

*/

	// Use this for initialization
	void Start() {

		//initWorkflow ();
		//jsonResourcesPath = workflow.tasks [0].ResourcesPath;
		//DisplayStep (workflow.tasks[0].steps[currentStep]);

		//Display = GameObject.Find("DisplayAnchored");
		//  Display = GameObject.Find("/Canvas/DisplayAnchored");
		//Marker = GameObject.Find("Marker1");
		//stepName = GameObject.Find( "StepName" ).GetComponent<Text>();
		//stepNumber = GameObject.Find( "StepNumber" ).GetComponent<Text>();

	}


	double timer = 0.0;
	int currentImagetoBeDisplayed;
	int currentTexttoBeDisplayed;
	private List<Image> imageList;
	private List<Text> textList;

	// Update is called once per frame
	void Update() {


		if(Input.GetKeyDown(KeyCode.RightArrow)){
			//next();

		}

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			//previous();

		}

		

		if( timeDelayForMultipleObjects != 0.0f ) {
			timer += Time.deltaTime;
			if( timer > timeDelayForMultipleObjects ) {
				timer = 0.0f;
				currentImagetoBeDisplayed++;
				currentTexttoBeDisplayed++;

				Debug.Log (imageList.ToString());

				if( currentImagetoBeDisplayed >= imageList.Count ) {
					currentImagetoBeDisplayed = 0;
				}
				if( currentTexttoBeDisplayed >= textList.Count ) {
					currentTexttoBeDisplayed = 0;
				}
				foreach( Image img in imageList ) {
					img.enabled = false;
				}
				foreach( Text txt in textList ) {
					txt.enabled = false;
				}
				if( imageList.Count > 0 ) {
					imageList[currentImagetoBeDisplayed].enabled = !hideOverlays;
				}
				if( textList.Count > 0) {
					textList[currentTexttoBeDisplayed].enabled = !hideOverlays;
				}
			}
		}
		else {
			foreach( Image img in imageList ) {
				img.enabled = !hideOverlays;
			}
			foreach( Text txt in textList ) {
				txt.enabled = !hideOverlays;
			}
		}
	}

	public override void DisplayStep(CAMMRADMainController.CAMMRADWorkflowStep step){

		return;
		
		Debug.Log("Display step called...");
		Debug.Log("**********************************");

		// Get rid of leftovers from older steps
		foreach( Transform child in Display.transform ) {
			GameObject.Destroy( child.gameObject );
		}

		// Change step name & number
		//stepName.text = step.Name;
		//stepNumber.text = "step " + currentStep.ToString();

		displayText = step.ui_texts;
		displayImage = step.ui_images;

		Debug.Log ("This many display anchored images found " + displayImage.Length);


		featObjects = step.overlays;

		timer = timeDelayForMultipleObjects;

		textList = new List<Text>();

		foreach(CAMMRADMainController.CAMMRADStepUIText entry in displayText ) {
			//double[] position = entry.Position.ToArray();
			float scale = entry.scale;

			//Vector3 pos = new Vector3( (float)position[0], (float)position[1], (float)position[2] );

			Vector3 pos = new Vector3( entry.x, entry.y, entry.z);

			Text newTextInstance = MonoBehaviour.Instantiate( DisplayTextFab ) as Text;
			newTextInstance.transform.SetParent( Display.transform );
			newTextInstance.transform.localPosition = pos;
			newTextInstance.transform.localRotation = Quaternion.identity;
			if( scale > 0 ) {
				newTextInstance.transform.localScale = Vector3.Scale( newTextInstance.transform.localScale, new Vector3( scale, scale, 1 ) );
			}

			textList.Add( newTextInstance );
			newTextInstance.text = entry.text;
			newTextInstance.gameObject.SetActive( true );
		}


		currentTexttoBeDisplayed = textList.Count - 1;
		imageList = new List<Image>();


		foreach( CAMMRADMainController.CAMMRADStepUIImage myImg in displayImage ) {

			Debug.Log ("Image path was: " +  myImg.imageURL);

			if( myImg.imageURL == "" ) {
				continue;
			}

			//double[] position = myImg.Position.ToArray();
			//float scale = myImg.Scale;

			//Vector3 pos = new Vector3( (float)position[0], (float)position[1], (float)position[2] );

			Vector3 pos = new Vector3( myImg.x, myImg.y, myImg.z);

			//Create new UI image object for use
			Image newImgInstance = MonoBehaviour.Instantiate( DisplayImgFab ) as Image;
			newImgInstance.transform.SetParent( Display.transform );
			newImgInstance.transform.localPosition = pos;
			newImgInstance.transform.localRotation = Quaternion.identity;


			imageList.Add( newImgInstance );

			var tex = new Texture2D( 512, 512 );

			string filePath = Application.streamingAssetsPath + "/Resources/" + jsonResourcesPath + myImg.imageURL;

			// Code to deal with android compatibility
			if( filePath.Contains( "://" ) ) { // When in android
				WWW www = new WWW( filePath );
				//yield return www;
				while( !www.isDone ) { }

				tex.LoadImage( www.bytes );
			}
			else {                       // When not in android
				tex.LoadImage( File.ReadAllBytes( filePath ) );
			}


			if( tex == null )
				Debug.Log( myImg.imageURL + " not loaded properly");
			newImgInstance.sprite = Sprite.Create( ( tex as Texture2D ), new Rect( 0.0f, 0.0f, tex.width, tex.height ), new Vector2( 0, 0 ), 100 );
			int width = newImgInstance.sprite.texture.width;
			int height = newImgInstance.sprite.texture.height;
			newImgInstance.transform.localScale = Vector3.Scale( newImgInstance.transform.localScale, new Vector3( (float)width / height, 1, 1 ) );

			//if( scale > 0 ) {
			//	newImgInstance.transform.localScale = Vector3.Scale( newImgInstance.transform.localScale, new Vector3( scale, scale, 1 ) );
			//}

			newImgInstance.gameObject.SetActive( true );




		}
		currentImagetoBeDisplayed = imageList.Count - 1;

		//Clears pre-existing Marker objects before creating new ones
		foreach( Transform child in Marker.transform ) {
			GameObject.Destroy( child.gameObject );
		}

		foreach( CAMMRADMainController.CAMMRADStepOverlayObject feat in featObjects ) {
			//double[] position = feat.Position.ToArray();
			//Vector3 pos = new Vector3( (float)position[0], (float)position[1], (float)position[2]);

			Vector3 pos = new Vector3( feat.x, feat.y, feat.z);

			//Vector3 pos = new Vector3( 0f,0f,0f);

			Debug.Log ("Object name was: " + feat.object_name);
			Debug.Log ("Object animation was: " + feat.animation_name);
		
			if( feat.object_name.Trim().ToLower().Equals( "cube" )) {


				Debug.Log ("Object was matched!!: " + feat.object_name);
				
				GameObject featureInstance = MonoBehaviour.Instantiate( Cube );
				featureInstance.layer = LayerMask.NameToLayer( "AR foreground" );
				featureInstance.transform.SetParent( Marker.transform );
				featureInstance.transform.localPosition = pos;

				featureInstance.GetComponent<RegisteredObjectScript> ().animator_state = feat.animation_name;
				featureInstance.GetComponent<Animator> ().Play (feat.animation_name);

			}else if( feat.object_name.Trim().ToLower().Equals( "sphere" )) {

				GameObject featureInstance = MonoBehaviour.Instantiate( Sphere );
				featureInstance.layer = LayerMask.NameToLayer( "AR foreground" );
				featureInstance.transform.SetParent( Marker.transform );
				featureInstance.transform.localPosition = pos;

				featureInstance.GetComponent<RegisteredObjectScript> ().animator_state = feat.animation_name;
				featureInstance.GetComponent<Animator> ().Play (feat.animation_name);

			}else if( feat.object_name.Trim().ToLower().Equals( "pill" )) {

				GameObject featureInstance = MonoBehaviour.Instantiate( Pill );
				featureInstance.layer = LayerMask.NameToLayer( "AR foreground" );
				featureInstance.transform.SetParent( Marker.transform );
				featureInstance.transform.localPosition = pos;

				featureInstance.GetComponent<RegisteredObjectScript> ().animator_state = feat.animation_name;
				featureInstance.GetComponent<Animator> ().Play (feat.animation_name);

			}


		}



	}




	public void next() {
		currentStep = (currentStep + 1);
		//DisplayStep (workflow.tasks[0].steps[currentStep]);
		Debug.Log( "next" );
	}

	public void previous() {
		currentStep = (currentStep - 1);
		//if (currentStep < 0)
		//	currentStep = steps.Count + currentStep;
		//DisplayStep (workflow.tasks[0].steps[currentStep]);
		Debug.Log( "previous" );
	}



	private int currentTask = 0;

	/*
	public void switchTask() {
		Debug.Log( "switchTask" );
		if( jsonFiles.Count > 0 ) {
			currentTask = ( currentTask + 1 ) % jsonFiles.Count;
			currentStep = 0;
			if( string.IsNullOrEmpty( jsonFiles[currentTask] ) ) {
				Debug.Log("Filename is empty!");
				return;
			}
			#if !UNITY_EDITOR && UNITY_WSA
			//HoloLens code
			JsonPath = ApplicationData.Current.RoamingFolder.Path + "\\" + jsonFiles[currentTask];
			#else
			JsonPath = Application.streamingAssetsPath + "/" + jsonFiles[currentTask];
			#endif
			// Code to deal with android compatibility
			if( JsonPath.Contains( "://" ) ) { // When in android
				WWW www = new WWW( JsonPath );
				//yield return www;
				while( !www.isDone ) { }

				jsonString = www.text;
				Debug.Log( "You entered the android section: " + JsonPath );
			}
			else {                        // When not in android
				#if !UNITY_EDITOR && UNITY_WSA
				//HoloLens code

				Debug.Log( "You entered the Hololens section: " + JsonPath );
				try {
				using (Stream stream = OpenFileForRead(ApplicationData.Current.RoamingFolder.Path, jsonFiles[currentTask])) {
				byte[] data = new byte[stream.Length];
				stream.Read(data, 0, data.Length);
				jsonString = Encoding.ASCII.GetString(data);
				}
				}
				catch (Exception e) {
				Debug.Log(e);
				}
				#else
				jsonString = File.ReadAllText( JsonPath );
				Debug.Log( "You entered the non-android section: " + JsonPath );
				#endif
			}

		}
	}

	public void hide() {
		if( !hideOverlays ) {
			hideOverlays = true;
			foreach( Image img in imageList ) {
				img.enabled = false;
			}
			foreach( Text txt in textList ) {
				txt.enabled = false;
			}
			foreach( Transform child in Marker.transform ) {
				child.gameObject.SetActive( false );
			}
		}
	}

	public void show() {
		if( hideOverlays ) {
			hideOverlays = false;
			timer = timeDelayForMultipleObjects;
			currentImagetoBeDisplayed = imageList.Count - 1;
			currentTexttoBeDisplayed = textList.Count - 1;
			foreach( Image img in imageList ) {
				img.enabled = false;
			}
			foreach( Text txt in textList ) {
				txt.enabled = false;

			}
			if( imageList.Count > 0 ) {
				imageList[0].enabled = true;
			}
			if( textList.Count > 0 ) {
				textList[0].enabled = true;
			}
			foreach( Transform child in Marker.transform ) {
				child.gameObject.SetActive( true );
			}
		}
	}


	public void reload() {
		Debug.Log( "Reload" );
		if( jsonFiles.Count > 0 ) {
			if( string.IsNullOrEmpty( jsonFiles[currentTask] ) ) {
				Debug.Log( "Filename is empty!" );
				return;
			}
			#if !UNITY_EDITOR && UNITY_WSA
			//HoloLens code
			JsonPath = ApplicationData.Current.RoamingFolder.Path + "\\" + jsonFiles[currentTask];
			#else
			JsonPath = Application.streamingAssetsPath + "/" + jsonFiles[currentTask];
			#endif
			// Code to deal with android compatibility
			if( JsonPath.Contains( "://" ) ) { // When in android
				WWW www = new WWW( JsonPath );
				//yield return www;
				while( !www.isDone ) { }

				jsonString = www.text;
				Debug.Log( "You entered the android section: " + JsonPath );
			}
			else {                        // When not in android
				#if !UNITY_EDITOR && UNITY_WSA
				//HoloLens code
				Debug.Log( "You entered the Hololens section: " + JsonPath );
				try {
				using( Stream stream = OpenFileForRead( ApplicationData.Current.RoamingFolder.Path, jsonFiles[currentTask] ) ) {
				byte[] data = new byte[stream.Length];
				stream.Read( data, 0, data.Length );
				jsonString = Encoding.ASCII.GetString( data );
				}
				}
				catch( Exception e ) {
				Debug.Log( e );
				}
				#else
				jsonString = File.ReadAllText( JsonPath );
				Debug.Log( "You entered the non-android section: " + JsonPath );
				#endif
			}

		}
	}

	public void reset() {
		Debug.Log( "Reset" );
		if( jsonFiles.Count > 0 ) {
			currentStep = 0;
			if( string.IsNullOrEmpty( jsonFiles[currentTask] ) ) {
				Debug.Log( "Filename is empty!" );
				return;
			}
			#if !UNITY_EDITOR && UNITY_WSA
			//HoloLens code
			JsonPath = ApplicationData.Current.RoamingFolder.Path + "\\" + jsonFiles[currentTask];
			#else
			JsonPath = Application.streamingAssetsPath + "/" + jsonFiles[currentTask];
			#endif
			// Code to deal with android compatibility
			if( JsonPath.Contains( "://" ) ) { // When in android
				WWW www = new WWW( JsonPath );
				//yield return www;
				while( !www.isDone ) { }

				jsonString = www.text;
				Debug.Log( "You entered the android section: " + JsonPath );
			}
			else {                        // When not in android
				#if !UNITY_EDITOR && UNITY_WSA
				//HoloLens code
				Debug.Log( "You entered the Hololens section: " + JsonPath );
				try {
				using( Stream stream = OpenFileForRead( ApplicationData.Current.RoamingFolder.Path, jsonFiles[currentTask] ) ) {
				byte[] data = new byte[stream.Length];
				stream.Read( data, 0, data.Length );
				jsonString = Encoding.ASCII.GetString( data );
				}
				}
				catch( Exception e ) {
				Debug.Log( e );
				}
				#else
				jsonString = File.ReadAllText( JsonPath );
				Debug.Log( "You entered the non-android section: " + JsonPath );
				#endif
			}

		}
	}
*/
	public void initWorkflow(){

		string json_string = "";

		/*
		#if !UNITY_EDITOR && UNITY_WSA

		try {
		using (Stream stream = OpenFileForRead(ApplicationData.Current.RoamingFolder.Path, "all_workflow.json")) {
		byte[] data = new byte[stream.Length];
		stream.Read(data, 0, data.Length);
		json_string = Encoding.ASCII.GetString(data);
		}
		}
		catch (Exception e) {
		Debug.Log(e);
		}

		#else
		*/

		string JsonPath = Application.streamingAssetsPath + "/all_workflow.json";

		json_string = File.ReadAllText( JsonPath );

		//Debug.Log( "You entered the non-android section: " + JsonPath );


		//#endif

		Debug.Log(json_string);


		workflow =JsonUtility.FromJson<CAMMRADWorkflow>(json_string);


		Debug.Log ("Tasks parsed:" + workflow.tasks.Count);

	}



	[System.Serializable]
	public class CAMMRADWorkflow
	{
		public List<CAMMRADTask> tasks;
	}

	[System.Serializable]
	public class CAMMRADTask
	{
		public double Version;
		public string Name;
		public string dir_name;
		public string ResourcesPath;
		public List<CAMMRADStep> steps;
	}

	[System.Serializable]
	public class CAMMRADStep
	{
		public string Name;
		public double Version;
		public int Duration;
		public List<FeatureAnchoredObject> FeatureAnchoredObjects;
		public List<DisplayAnchoredImage> DisplayAnchoredImage;
		public List<DisplayAnchoredText> DisplayAnchoredText;
		public Wheel Wheel;
	}

	[System.Serializable]
	public class FeatureAnchoredObject
	{
		public string Image_Path;
		public List<double> Position;
		public string MarkerName;
		public List<AnimationPath> AnimationPath;
	}

	[System.Serializable]
	public class DisplayAnchoredImage
	{
		public string Image_Path;
		public List<double> Position;
		public float Scale;
		public List<AnimationPath> AnimationPath;

	}

	[System.Serializable]
	public class AnimationPath
	{
		public List<double> Position;
		public double Time;
	}


	[System.Serializable]
	public class DisplayAnchoredText
	{
		public string Text;
		public List<double> Position;
		public float Scale;
	}

	[System.Serializable]
	public class Wheel
	{
		public string Text;
		public string Icon;
	}



}