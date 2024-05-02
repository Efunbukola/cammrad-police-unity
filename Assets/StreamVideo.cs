using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class StreamVideo : MonoBehaviour {
	public RawImage image;
	public VideoPlayer player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Init and play video
	public void PlayVideo(string url){

		StartCoroutine (playVideo(url));

	}


	//Wait for video to be prepared and play video
	IEnumerator playVideo(string url){


		Debug.Log("playing video: " + url);
		Debug.Log("local path: " + CAMMRADCacheManager.getVideoLocalPath(url)); 

		//Set player url to local path
		player.url = CAMMRADCacheManager.getVideoLocalPath(url);


		//Prepare video
		player.Prepare();

		//Wait a few seconds
		WaitForSeconds waitForSeconds = new WaitForSeconds (3);

		//Check for video prepared
		while (!player.isPrepared) {
			
			yield return waitForSeconds;
			break;
		}

		image.texture = player.texture;
		player.Play();

	}
}
