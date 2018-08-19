using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExperimentManager : MonoBehaviour {

	private GameObject blackCube;
	private GameObject puck;
	private GameObject maze;
	public int subtrial = 0;
	//each run contains 9 puck distances
	double[,] puckDist = {{10.06, 7.49, 5.05, 5.07, 9.92, 7.5, 7.45, 10.03, 4.93}, {7.51, 7.59, 9.98, 9.98, 9.94, 4.91, 7.41, 5.08, 5.09}, {4.9, 4.96, 10.1, 7.51, 9.92, 10.07, 7.53, 7.44, 5.07}, {5.02, 5.07, 7.5, 7.41, 9.98, 9.95, 10.06, 7.52, 5.08}, {7.54, 9.98, 5.0, 7.41, 5.0, 10.02, 10.1, 7.41, 4.98}, {7.51, 9.9, 5.09, 5.02, 4.99, 7.45, 10.06, 7.59, 10.06}, {10.02, 7.47, 10.02, 7.5, 5.0, 10.01, 4.94, 5.01, 7.47}, {4.95, 7.53, 10.05, 9.9, 7.4, 9.95, 5.07, 7.42, 5.06}, {9.98, 4.96, 7.48, 5.04, 9.94, 7.49, 4.99, 7.4, 9.92}, {7.51, 10.06, 5.04, 10.1, 4.93, 7.54, 10.0, 7.44, 5.04}, {7.57, 5.08, 10.05, 4.96, 7.59, 7.46, 9.95, 9.98, 5.09}, {7.47, 9.94, 9.99, 7.48, 5.04, 10.03, 5.0, 7.44, 5.09}, {4.97, 10.09, 7.44, 4.94, 10.03, 5.1, 7.47, 10.07, 7.56}, {5.08, 7.55, 9.96, 4.94, 4.93, 7.57, 9.97, 7.52, 9.97}, {7.47, 9.98, 10.06, 7.57, 10.01, 7.52, 5.07, 4.91, 5.06}, {9.91, 7.42, 10.08, 4.9, 7.42, 5.07, 5.06, 7.53, 9.95}, {7.45, 7.5, 9.91, 5.09, 9.92, 7.46, 5.08, 10.03, 4.98}, {7.44, 10.04, 10.01, 7.6, 5.09, 5.09, 10.03, 7.52, 4.92}, {5.03, 7.46, 7.47, 5.03, 10.07, 9.96, 7.48, 10.08, 5.09}, {7.42, 10.07, 4.91, 10.06, 10.0, 4.91, 7.46, 7.53, 5.01}, {10.08, 4.96, 9.93, 7.5, 7.46, 7.58, 10.08, 4.92, 5.06}, {7.54, 9.91, 4.94, 10.09, 4.99, 4.93, 7.45, 10.06, 7.53}, {9.93, 9.95, 7.43, 5.01, 5.03, 4.98, 7.47, 10.07, 7.41}, {7.5, 5.05, 9.95, 7.5, 4.99, 7.45, 5.04, 9.98, 10.01}, {5.05, 7.54, 7.46, 4.93, 5.08, 10.05, 10.04, 9.97, 7.57}, {7.54, 10.05, 4.91, 10.0, 5.05, 7.54, 7.47, 9.93, 5.01}, {10.09, 9.98, 7.58, 10.06, 7.49, 5.06, 5.03, 5.07, 7.54}, {7.45, 9.99, 4.98, 4.96, 9.94, 4.92, 9.9, 7.55, 7.4}, {10.07, 10.0, 5.06, 10.05, 7.59, 4.95, 5.04, 7.56, 7.51}, {10.0, 9.96, 7.51, 9.97, 5.05, 5.09, 7.43, 5.09, 7.5}, {5.03, 5.0, 10.02, 10.07, 4.98, 7.46, 7.44, 10.08, 7.4}, {4.93, 7.47, 5.02, 9.98, 5.01, 7.4, 9.99, 10.07, 7.48}, {7.53, 10.07, 5.03, 10.05, 10.06, 7.59, 4.98, 5.03, 7.56}, {4.99, 9.97, 5.0, 7.48, 7.5, 4.91, 9.9, 10.03, 7.55}, {4.91, 7.5, 10.0, 5.08, 7.57, 4.95, 10.05, 9.93, 7.47}, {9.91, 10.08, 5.07, 5.06, 10.07, 7.59, 7.44, 4.97, 7.54}, {9.93, 7.52, 5.09, 9.93, 7.55, 4.99, 7.45, 5.08, 10.01}, {4.92, 7.41, 4.98, 7.52, 5.0, 7.44, 9.94, 9.91, 10.08}, {10.06, 10.02, 4.92, 9.99, 7.43, 7.48, 5.0, 7.45, 4.98}, {7.48, 4.98, 7.58, 10.08, 10.05, 7.52, 4.96, 5.07, 9.93}, {9.92, 7.41, 5.08, 4.91, 9.95, 7.57, 9.92, 7.41, 5.07}, {10.01, 7.52, 5.03, 7.41, 10.02, 9.97, 4.95, 5.09, 7.59}, {7.55, 9.93, 7.6, 5.05, 5.04, 7.45, 10.07, 5.04, 10.08}, {7.58, 7.54, 9.9, 4.99, 7.48, 4.93, 9.92, 5.01, 10.0}, {9.97, 5.07, 9.97, 7.51, 4.96, 7.48, 5.06, 7.45, 10.06}, {7.43, 5.08, 9.97, 5.01, 9.92, 10.0, 5.04, 7.46, 7.57}, {4.97, 5.05, 5.0, 9.97, 7.43, 7.58, 9.97, 7.51, 9.91}, {4.93, 7.52, 5.06, 10.05, 7.6, 10.04, 5.0, 7.56, 9.97}, {7.48, 9.91, 7.55, 9.92, 10.01, 7.44, 5.01, 5.04, 5.08}, {5.03, 4.93, 7.47, 10.08, 7.4, 5.07, 9.96, 10.04, 7.57}, {5.1, 7.44, 5.06, 10.05, 5.05, 10.0, 7.55, 10.0, 7.45}, {4.96, 4.9, 5.04, 10.01, 10.09, 10.05, 7.42, 7.46, 7.58}, {4.96, 4.96, 7.6, 7.52, 10.03, 10.1, 5.0, 7.51, 9.93}, {7.47, 5.07, 7.49, 9.97, 10.0, 10.05, 4.98, 5.09, 7.56}, {5.1, 4.96, 10.06, 5.02, 10.04, 9.99, 7.45, 7.55, 7.5}, {7.59, 4.95, 4.95, 9.92, 4.91, 10.04, 7.5, 10.07, 7.48}, {10.0, 10.07, 7.45, 4.98, 7.42, 5.03, 5.01, 7.46, 10.06}, {5.07, 10.0, 7.58, 4.92, 9.98, 7.44, 7.48, 4.92, 10.08}, {7.59, 5.1, 5.0, 7.59, 7.53, 10.01, 4.9, 9.99, 9.95}, {7.51, 7.52, 5.01, 7.43, 9.96, 9.98, 10.04, 5.02, 5.09}, {7.47, 10.08, 9.96, 7.53, 5.01, 10.06, 5.04, 4.91, 7.54}, {9.98, 9.9, 7.58, 9.9, 7.44, 5.04, 5.01, 7.51, 4.95}, {7.55, 9.92, 7.43, 7.45, 9.91, 4.9, 4.97, 5.0, 10.04}, {5.09, 9.96, 4.95, 5.03, 7.45, 7.54, 9.97, 7.5, 10.09}, {5.01, 5.01, 7.53, 7.58, 9.91, 10.01, 7.48, 10.0, 5.01}, {4.92, 7.53, 10.04, 7.51, 10.07, 10.04, 7.51, 4.97, 4.98}, {9.92, 5.07, 9.99, 5.07, 7.58, 9.98, 7.43, 4.97, 7.57}, {4.98, 10.09, 9.98, 7.56, 10.02, 5.01, 5.03, 7.49, 7.58}, {7.45, 7.47, 4.92, 10.08, 5.0, 10.06, 7.6, 4.93, 10.03}, {7.53, 5.08, 4.96, 10.05, 9.99, 9.91, 7.6, 5.08, 7.47}, {7.5, 4.93, 10.04, 5.03, 10.1, 5.08, 7.47, 7.53, 9.9}, {7.49, 4.99, 9.94, 5.06, 5.03, 7.55, 10.04, 9.99, 7.5}, {9.93, 10.09, 4.94, 7.56, 7.46, 7.53, 5.05, 4.91, 10.07}, {4.9, 10.06, 9.95, 7.47, 5.1, 7.58, 7.53, 5.06, 10.09}, {7.45, 9.92, 5.03, 5.04, 5.04, 10.03, 7.51, 9.98, 7.56}, {7.46, 4.96, 7.54, 10.0, 9.99, 10.01, 5.02, 4.98, 7.6}, {9.95, 7.55, 9.97, 7.47, 9.93, 5.01, 5.07, 7.52, 5.06}, {7.51, 4.99, 9.92, 9.94, 7.52, 5.07, 7.48, 4.94, 10.03}, {10.07, 9.95, 4.95, 7.6, 7.53, 4.91, 7.46, 4.98, 9.97}, {5.02, 5.02, 7.45, 10.07, 7.55, 9.96, 5.06, 7.41, 10.06}, {5.04, 10.04, 7.56, 7.58, 9.93, 7.48, 5.08, 9.95, 4.92}, {7.57, 5.03, 7.59, 9.98, 5.08, 7.5, 9.95, 10.05, 5.03}, {7.54, 10.09, 5.08, 9.99, 5.01, 9.98, 7.42, 5.08, 7.41}, {9.96, 10.08, 5.08, 7.49, 4.91, 7.44, 9.93, 7.57, 5.06}, {5.03, 9.99, 10.0, 10.08, 7.59, 7.49, 4.98, 5.08, 7.51}, {5.06, 7.52, 7.42, 4.98, 9.9, 10.01, 10.08, 5.1, 7.56}, {10.08, 9.93, 5.0, 9.94, 7.47, 7.43, 7.4, 5.06, 4.99}, {7.59, 7.45, 7.51, 5.06, 10.01, 4.91, 9.91, 10.02, 5.03}, {7.57, 7.46, 9.92, 7.6, 5.0, 5.05, 4.96, 10.06, 9.96}, {4.91, 9.98, 7.52, 7.57, 10.09, 5.05, 7.53, 9.94, 5.08}, {5.04, 7.44, 5.09, 10.1, 9.95, 9.91, 7.4, 4.97, 7.59}, {9.94, 4.97, 5.0, 7.53, 7.44, 4.96, 10.04, 7.48, 9.96}, {5.09, 7.48, 4.99, 10.09, 5.04, 10.04, 7.47, 7.41, 10.06}, {7.54, 7.51, 4.99, 10.06, 10.02, 9.95, 4.99, 7.5, 5.07}, {10.06, 7.49, 7.48, 9.9, 5.04, 5.09, 5.03, 7.53, 9.97}, {10.04, 4.94, 7.43, 10.02, 7.6, 4.92, 7.53, 10.09, 4.92}, {7.49, 7.57, 10.09, 4.96, 9.94, 5.01, 4.98, 10.02, 7.41}, {10.01, 5.08, 7.54, 4.9, 9.91, 7.46, 5.04, 7.5, 9.97}, {7.5, 10.05, 7.57, 7.5, 10.05, 4.9, 4.96, 10.1, 4.92}, {10.01, 9.91, 7.41, 7.55, 7.57, 4.91, 9.9, 5.1, 4.9}, {5.08, 9.97, 7.51, 7.45, 4.95, 9.9, 9.98, 7.49, 4.94}, {7.41, 9.99, 5.07, 9.97, 7.43, 4.93, 5.08, 7.46, 10.02}, {4.95, 4.94, 7.41, 4.96, 9.94, 7.48, 9.97, 9.95, 7.45}, {7.42, 5.07, 7.52, 5.08, 10.06, 5.08, 7.41, 10.08, 10.01}, {7.54, 9.93, 4.93, 9.99, 7.51, 10.03, 7.57, 4.9, 4.93}, {4.99, 9.91, 7.58, 9.94, 7.57, 5.08, 5.01, 7.54, 10.01}, {5.03, 5.06, 7.47, 7.42, 9.91, 7.59, 9.95, 5.03, 10.02}, {5.09, 7.47, 5.04, 9.95, 9.93, 7.42, 10.08, 7.43, 4.9}, {7.41, 7.49, 7.47, 9.94, 10.05, 5.02, 5.0, 9.93, 4.99}, {7.52, 10.1, 7.57, 9.9, 5.03, 10.03, 4.92, 7.47, 4.94}, {5.05, 5.01, 7.6, 10.01, 10.0, 7.58, 7.52, 9.98, 4.96}, {5.07, 7.46, 9.91, 7.59, 10.03, 7.5, 4.96, 4.95, 9.99}, {7.51, 4.98, 9.98, 4.96, 7.5, 9.99, 7.57, 4.97, 10.07}, {10.01, 7.51, 5.06, 10.05, 7.4, 10.01, 5.04, 7.5, 5.08}, {5.05, 7.43, 7.49, 10.07, 10.09, 4.96, 7.47, 4.92, 10.01}, {9.97, 5.1, 5.0, 7.45, 10.08, 9.94, 7.42, 5.1, 7.6}, {7.46, 5.09, 9.92, 5.02, 10.1, 5.06, 7.43, 9.98, 7.4}, {10.07, 7.44, 4.92, 5.04, 9.91, 7.49, 4.98, 7.49, 9.94}, {10.07, 5.01, 9.99, 7.59, 5.06, 7.45, 7.41, 10.08, 5.06}, {9.94, 5.05, 10.09, 7.43, 4.99, 7.43, 7.5, 5.08, 10.02}, {4.97, 7.5, 7.4, 9.96, 7.49, 10.02, 4.96, 10.05, 4.96}, {5.09, 9.96, 9.99, 4.96, 7.54, 5.06, 10.02, 7.51, 7.53}, {5.06, 10.03, 5.05, 7.59, 10.02, 5.08, 7.47, 9.97, 7.47}, {9.91, 10.07, 4.94, 9.94, 5.08, 4.98, 7.52, 7.59, 7.56}, {5.05, 7.43, 7.55, 7.6, 4.93, 5.01, 10.03, 10.01, 9.99}, {7.47, 10.04, 4.92, 9.93, 7.56, 4.93, 9.96, 5.02, 7.43}};

	// experiment manager will set up the experiment depending on the person (0 - 17 inclusive)
	// we know the order (C, M, B) of each person --> might want to do something where we load
	// scenes in different order depending on the person (AKA if 0 then C, M, B; if 5 then M, B, C)
	// within each scene need to randomly generate the order (5, 7.5, 10) and the number (5 +/- 0.1, etc)
	// so best thing is have experiment manager set up the order of the scenes
	// maybe have first middle last so if first then when done go to middle middle go to last and when last done
	// do nothing or something to indicate this is the last one
	// then have the scenes set up their own individualized stuff

	// have global thing for gear or go ?? and other global variables

	// so have 2d array where first index is the user ID number (0 through 17 inclusive) and then 2D part of the array
	// could also do a hash map & mod 6 for this bc we repeat :-) but that's up to you @ Taylor
	// is the order of C, M, B --> [ [ C, M, B ] [ B, M, C ] [ C, B, M ], etc]
	// SceneManager loads scenes in order of 2Darray[userID][0], 2Darray[userID][1], 2Darray[userID][2]
	// and sets global variables of first, middle, last

	// within each scene when we first load we need to randomly generate the order of the 9 trials (5, 7.5, 10 * 3)
	// and randomly generate the actual distances
	// also have them press to start the 1 minute trial where they do something, then after one minute start trials
	// start by looking at puck press to activate the black
	// cube and have them walk in place. when done press again and then transport back to origin or whatever or just have puck
	// spawn X distance away from where the player is
	// so each trial has 2 button presses so when you hit the 18th button then you are moving on to the next scene
	// it can automatically load the next scene and then press to begin

	// when you are walking it is using the script CNN, miti, or biomechanical and j keep track of the transform in a global variable
	// and at the end when they press button to end that trial print out pertinent information


	//How to tell if oculus button is pressed: OVRInput.Get(OVRInput.Button.One)

	// Use this for initialization
	void Start () {
		blackCube = GameObject.Find ("Black Cube");
		puck = GameObject.Find ("hocket puck");
		maze = GameObject.Find ("Maze 2");
		blackCube.SetActive (false);
		puck.SetActive (false);
		maze.SetActive (true);

		//File.AppendAllText (Application.persistentDataPath + subjectID.ToString() + ".txt", "New Run" + "/n");

	}

	private static int LEARNING_SCENE = -2;
	private static int LONG_WAIT_SCENE = -1;
	private static int DESERT_SCENE = 0;
	private static int PUCK_SCENE = 1;
	private static int BLACK_SCENE = 2;
	private static int WAIT_SCENE = 3;
	private int scene = LEARNING_SCENE;
	private float timer = 0f;
	
	// Update is called once per frame
	void Update () {
		string path = Application.persistentDataPath + "/" + WalkingTechManager.statSubject + "_" + WalkingTechManager.walkingType() + ".txt";

		OVRInput.Update ();
		//at this point the other script has selected the appropriate method of walking, this component can be enabled and disabled as needed
		if (scene == LEARNING_SCENE) {
			if (Input.GetMouseButtonDown(0)){
				maze.SetActive (true);
				puck.SetActive (false);
				blackCube.SetActive (false);
				timer = Time.time + 5;
				scene = LONG_WAIT_SCENE;
			}
		}else if (scene == LONG_WAIT_SCENE) {
			if (Time.time > timer){
				maze.SetActive (false)	;
				puck.SetActive (false);
				blackCube.SetActive (false);
				WalkingTechManager.walkingEnabled (false);
				this.transform.position = new Vector3 (0f, GlobalVariables.height, 0f);
				puck.transform.position = new Vector3 ((float)puckDist [WalkingTechManager.statSubject * 7 + WalkingTechManager.statTrial, subtrial], 0f, 0f);
				scene = DESERT_SCENE;
			}
		}else if (scene == DESERT_SCENE) {
			if (Input.GetMouseButtonDown(0)){
				puck.SetActive (true);
				blackCube.SetActive (false);
				//set puck location
				scene = PUCK_SCENE;
			}
		}else if (scene == PUCK_SCENE) {
			if (Input.GetMouseButtonDown(0)){
				puck.SetActive (false);
				blackCube.SetActive (true);
				WalkingTechManager.walkingEnabled (true);
				scene = BLACK_SCENE;
			}
		}else if (scene == BLACK_SCENE) {
			if (Input.GetMouseButtonDown(0)){
				puck.SetActive (false);
				blackCube.SetActive (true);
				timer = Time.time + 1;
				scene = WAIT_SCENE;
			}
		}else if (scene == WAIT_SCENE) {
			if (Time.time > timer){
				puck.SetActive (false);
				blackCube.SetActive (false);
				WalkingTechManager.walkingEnabled (false);
				//record data
				string appendText = DateTime.Now.ToString () + ";" +
				                    Time.time + ";" +
				                    this.transform.position.ToString ("F2") + ";" +
				                    puck.transform.position.ToString ("F2") + "\r\n";
				File.AppendAllText (path, appendText);
				//reset variables
				this.transform.position = new Vector3 (0f, GlobalVariables.height, 0f);
				puck.transform.position = new Vector3 ((float)puckDist [WalkingTechManager.statSubject * 7 + WalkingTechManager.statTrial, subtrial], 0f, 0f);
				subtrial++;
				if (subtrial < 9) {
					scene = DESERT_SCENE;
					blackCube.SetActive (false);
				}
				else
					scene = -100;
			}
		}


	}

	public void recordData(){
		string path = Application.persistentDataPath + "/" + WalkingTechManager.statSubject + "_WIP_MITIP_GO.txt";

		// debug output
		string appendText = "\n" + DateTime.Now.ToString() + ";" + 
			Time.time + ";" + 

			// NEED TO CHANGE TO WHATEVER IS ANALOGOUS TO THIS IN GIO
			OVRInput.Get(OVRInput.Button.One);

		File.AppendAllText (path, appendText);
	}
}
