﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPMScript : MonoBehaviour {


	bool isRotating = false;
	bool isRotatingForward = false;
	float degreesRotatedAlready = 0;
	float distanceBetweenPoints = 0;

	int currentItem = 0;


	public float rotationSpeed = 5f;

	public int numPoints = 20;                        //number of points on radius to place prefabs
	public Vector3 centerPos = new Vector3(0,0,32);    //center of circle/elipsoid

	List<GameObject> stepObjs = new List<GameObject> ();

	public GameObject parent;
	public GameObject pointPrefab;                    //generic prefab to place on each point
	public float radiusX,radiusY;                    //radii for each x,y axes, respectively

	public bool isCircular = false;                    //is the drawn shape a complete circle?
	public bool vertical = true;                    //is the drawb shape on the xy-plane?

	public GameObject centerObj;                              //position to place each prefab along the given circle/eliptoid
	public GameObject centerPoint; 
	Vector3 pointPos;

	//*is set during each iteration of the loop

	// Use this for initialization
	void Start () {


		distanceBetweenPoints = 360 / numPoints;
		pointPos = centerPoint.transform.position;

		for(int i = 0; i<numPoints;i++){
			//multiply 'i' by '1.0f' to ensure the result is a fraction
			float pointNum = ((numPoints - i)*1.0f)/numPoints;

			//angle along the unit circle for placing points
			float angle = pointNum*Mathf.PI*2;

			float x = Mathf.Sin (angle)*radiusX;


			float y = Mathf.Cos (angle)*radiusY;


			//Debug.Log (i + ". ");
			//Debug.Log ("Point number" + pointNum);
			Debug.Log ("XY:" + x + ", " + y);


			//Debug.Log ("Angle " + i + ": " + angle);

			//position for the point prefab
			if(vertical)
				pointPos = new Vector3(x, y)+centerPos;
			else if (!vertical){
				pointPos = new Vector3(x, 0, y)+centerPos;
			}

			//place the prefab at given position
			GameObject item = Instantiate (pointPrefab, pointPos, Quaternion.identity);
			//item.transform.GetChild(0).GetComponent<TextMesh> ().text = i + "";
			stepObjs.Add (item);
			item.transform.parent = gameObject.transform;
		}

		//keeps radius on both axes the same if circular
		if(isCircular){
			radiusY = radiusX;
		}

		changeCurrentItem (0);

	}


	void Update ()
	{

		for (int i = 0; i < stepObjs.Count; i++) {

			stepObjs [i].transform.rotation = Quaternion.identity;

		}


		if(Input.GetKeyDown(KeyCode.LeftArrow) && !isRotating){
			back();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow) && !isRotating){
			next();
		}


		if (isRotating && !isRotatingForward) {

			transform.Rotate(Vector3.forward, 1 * rotationSpeed );

			degreesRotatedAlready += 1 * rotationSpeed;  


			if (degreesRotatedAlready >= distanceBetweenPoints) { //Stop rotating

				isRotating = false;
				isRotatingForward = false;
				degreesRotatedAlready = 0;

			}
		}else 
			if (isRotating && isRotatingForward) {

				transform.Rotate(Vector3.back, 1 * rotationSpeed );

				degreesRotatedAlready += 1 * rotationSpeed;  


				if (degreesRotatedAlready >= distanceBetweenPoints) { //Stop rotating

					isRotating = false;
					isRotatingForward = false;
					degreesRotatedAlready = 0;

				}
			}


	}


	void next(){

		if (currentItem < numPoints - 1) {
			isRotating = true;
			isRotatingForward = true;
			degreesRotatedAlready = 0;
			changeCurrentItem (currentItem+1);
		}

	}


	void back(){

		if (currentItem > 0) {
			isRotating = true;
			isRotatingForward = false;
			degreesRotatedAlready = 0;
			changeCurrentItem (currentItem-1);
		}

	}

	void goTo(int index){

		currentItem = index;
		Debug.Log ("Current Item:" + currentItem);

		//centerObj.GetComponent<TextMesh>().text =  "Step " + currentItem;

	}

	void changeCurrentItem(int index){

		currentItem = index;
		Debug.Log ("Current Item:" + currentItem);

		//centerObj.GetComponent<TextMesh>().text =  "Step " + currentItem;

	}



}