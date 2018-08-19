using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingTechManager : MonoBehaviour {

	public int subjectNumber;
	public int trialNumber; //-1 means for training

	public static int statSubject;
	public static int statTrial;

	private static WalkingTechManager singleton;

	private static System.Type[] conditionOrder = new System.Type[7];

	// Use this for initialization
	void Start () {
		statSubject = subjectNumber;
		statTrial = trialNumber;
		singleton = this;
		this.transform.position = this.transform.position + new Vector3 (0f, GlobalVariables.height, 0f);

		if (trialNumber < 0) {
			switch (trialNumber) {
			case -4:
				this.GetComponent<ThresholdGear> ().enabled = true;
				break;
			case -3:
				this.GetComponent<ThresholdGo> ().enabled = true;
				break;
			case -2:
				this.GetComponent<FreqGear> ().enabled = true;
				break;
			case -1:
				this.GetComponent<FreqGo> ().enabled = true;
				break;
			}
			return;
		}

		switch (subjectNumber % 12) {
		case 0:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputGo),
				typeof(RealWalking)
			};
			break;
		case 1:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputGo),
				typeof(RealWalking),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Gear)
			};
			break;
		case 2:
			conditionOrder = new System.Type[] {
				typeof(RealWalking),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputRateGo)
			};
			break;
		case 3:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputRateGear),
				typeof(RealWalking)
			};
			break;
		case 4:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Gear),
				typeof(RealWalking),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputGo)
			};
			break;
		case 5:
			conditionOrder = new System.Type[] {
				typeof(RealWalking),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Gear)
			};
			break;
		case 6:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputCNNGo),
				typeof(RealWalking)
			};
			break;
		case 7:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputCNNGo),
				typeof(RealWalking),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputCNNGear)
			};
			break;
		case 8:
			conditionOrder = new System.Type[] {
				typeof(RealWalking),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputRateGo)
			};
			break;
		case 9:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear),
				typeof(RealWalking)
			};
			break;
		case 10:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear),
				typeof(RealWalking),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputCNNGo)
			};
			break;
		case 11:
			conditionOrder = new System.Type[] {
				typeof(RealWalking),
				typeof(AccelerometerInputGo),
				typeof(AccelerometerInputRateGo),
				typeof(AccelerometerInputCNNGo),
				typeof(AccelerometerInput4Gear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear)
			};
			break;
		}

		if (conditionOrder[trialNumber] == typeof(AccelerometerInputGo))
			this.GetComponent<AccelerometerInputGo> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputRateGo))
			this.GetComponent<AccelerometerInputRateGo> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputCNNGo))
			this.GetComponent<AccelerometerInputCNNGo> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInput4Gear))
			this.GetComponent<AccelerometerInput4Gear> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputRateGear))
			this.GetComponent<AccelerometerInputRateGear> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputCNNGear))
			this.GetComponent<AccelerometerInputCNNGear> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(RealWalking))
			this.GetComponent<RealWalking> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void walkingEnabled(bool enabled){
		if (conditionOrder[statTrial] == typeof(AccelerometerInputGo))
			singleton.GetComponent<AccelerometerInputGo> ().enabled = enabled;
		if (conditionOrder[statTrial] == typeof(AccelerometerInputRateGo))
			singleton.GetComponent<AccelerometerInputRateGo> ().enabled = enabled;
		if (conditionOrder[statTrial] == typeof(AccelerometerInputCNNGo))
			singleton.GetComponent<AccelerometerInputCNNGo> ().enabled = enabled;
		if (conditionOrder[statTrial] == typeof(AccelerometerInput4Gear))
			singleton.GetComponent<AccelerometerInput4Gear> ().enabled = enabled;
		if (conditionOrder[statTrial] == typeof(AccelerometerInputRateGear))
			singleton.GetComponent<AccelerometerInputRateGear> ().enabled = enabled;
		if (conditionOrder[statTrial] == typeof(AccelerometerInputCNNGear))
			singleton.GetComponent<AccelerometerInputCNNGear> ().enabled = enabled;
		if (conditionOrder[statTrial] == typeof(RealWalking))
			singleton.GetComponent<RealWalking> ().enabled = enabled;
	}

	public static string walkingType(){
		return conditionOrder [statTrial].ToString ();
	}
}
