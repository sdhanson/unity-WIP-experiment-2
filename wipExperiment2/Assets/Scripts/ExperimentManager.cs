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
	public int subjectID;


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
		blackCube.SetActive (false);

		//File.AppendAllText (Application.persistentDataPath + subjectID.ToString() + ".txt", "New Run" + "/n");

	}
	
	// Update is called once per frame
	void Update () {
		
		OVRInput.Update ();
		if (OVRInput.Get (OVRInput.Button.One) == true) {
			blackCube.SetActive (true);
		}

		string path = Application.persistentDataPath + "/WIP_MITIP_GO.txt";

		// debug output
		string appendText = "\n" + DateTime.Now.ToString() + ";" + 
			Time.time + ";" + 

			// NEED TO CHANGE TO WHATEVER IS ANALOGOUS TO THIS IN GIO
			OVRInput.Get(OVRInput.Button.One);

		File.AppendAllText (path, appendText);

	}
}
