﻿using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading;

using TensorFlow;


public class AccelerometerInputCNN : MonoBehaviour
{
	// set per person
	private float height = GlobalVariables.height;

    Thread run;
    Thread collect;

    // used to determine direction to walk
    private float yaw;
    private float rad;
    private float xVal;
    private float zVal;

    // determine if person is picking up speed or slowing down
    public static float velocity = 0f;
    public static float method1StartTimeGrow = 0f;
    public static float method1StartTimeDecay = 0f;
    //phase one when above (+/-) 0.10 threshold
    public static bool wasOne = false;
    //phase two when b/w -0.10 and 0.10 thresholds
    public static bool wasTwo = true;
    private float decayRate = 0.4f;

    // initial X and Y angles - used to determine if user is looking around
    private float eulerX;
    private float eulerZ;

    // indicates if person is looking around - not implemented yet
    bool looking = false;

    // set by trained CNN model
    public int inputWidth = 40;

    // third value corresponds to inputWidth
    public TextAsset graphModel;
    private float[,,,] inputTensor = new float[1, 1, 40, 3];

    // list for keeping track of values for tensor
    private List<float> accelX;
    private List<float> accelY;
    private List<float> accelZ;

    // determine if person is walking from cnn returned value
    private bool walking = false;
    private int standIndex = 0;
    private int lookIndex = 2;

    // how many options of activities we have - standing, walking, jogging
    private int activityIndexChoices = 3;

    // FOR DEBUGGING PUTTING AT GLOBAL SCOPE
    float confidence = 0;
    float sum = 0f;
    float test = 0f;
    int activity = 0;
    bool here = false;
    bool longTime = false;
    float line = 0f;
    int index = 0;
    int countCNN = 0;
    float total = 0;
    float test1 = 0f;
    float test2 = 0f;
    float test3 = 0f;
    bool one = true;

    int diff = 20;

    float ctime = 0f;
    float ptime = 0f;

    // initialize display to get accelerometer from Oculus GO
    OVRDisplay display;

    void Start()
    {
        // tensorflowsharp requires this statement
#if UNITY_ANDROID
		TensorFlowSharp.Android.NativeBinding.Init ();
#endif

        // enable the gyroscope on the phone
        Input.gyro.enabled = true;

        // if we are on the right VR, then setup a client device to read transform data from
        if (Application.platform == RuntimePlatform.Android)
            SetupClient();

        // user must be looking ahead at the start
        eulerX = InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.x;
        eulerZ = InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.z;

        // initialize the oculus go display
        display = new OVRDisplay();

        // initialize the cnn lists
        accelX = new List<float>();
        accelY = new List<float>();
        accelZ = new List<float>();

        // start collection thread
        collect = new Thread(manageCollection);
        collect.Start();

        // start cnn thread - different thread so cnn doesn't interfere with graphics
        run = new Thread(manageCNN);
        run.Start();
    }

	void Update() {
		OVRInput.Update ();
	}

    void FixedUpdate() //was previously FixedUpdate()
    {
        // send the current transform data to the server (should probably be wrapped in an if isAndroid but I haven't tested)

        string path = Application.persistentDataPath + "/WIP_CNN_GO.txt";

        // debugging output
		string appendText = "\n" + DateTime.Now.ToString() + ";" + 
			Time.time + ";" + 

			// NEED TO CHANGE TO WHATEVER IS ANALOGOUS TO THIS IN GIO
			OVRInput.Get(OVRInput.Button.One) + ";" +

			display.acceleration.x + ";" + 
			display.acceleration.y + ";" + 
			display.acceleration.z + ";" + 

			gameObject.transform.position.x + ";" + 
			gameObject.transform.position.y + ";" + 
			gameObject.transform.position.z + ";" +

			UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.Head).eulerAngles.x + ";" +
			UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.Head).eulerAngles.y + ";" +
			UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.Head).eulerAngles.z + ";" +

			gateCollider.isInGate + ";" + 
			gateCollider.isTouchingWall + ";" + velocity;

        File.AppendAllText(path, appendText);

        // do the movement algorithm, more details inside
        move();

        if (myClient != null)
			myClient.Send(MESSAGE_DATA, new TDMessage(this.transform.localPosition, Camera.main.transform.eulerAngles, false));
    }

    void OnApplicationQuit()
    {
        collect.Abort();
        run.Abort();
    }

    // manages the accelerometer data collection thread
    void manageCollection()
    {
        // thread sleep for 1000 as to not connect information on boot
        Thread.Sleep(1000);
        while (true)
        {
            // sleeping time is linked to collection time
            Thread.Sleep(5);
            collectValues();
        }
    }

    // poll the GO for accelerometer data
    void collectValues()
    {
        float currX = display.acceleration.x;
        float currY = display.acceleration.y;
        float currZ = display.acceleration.z;
        test = currY;

        // collect - want the list to always be inputWidth
        if (accelX.Count < inputWidth)
        {
            // times for debugging
            if (accelX.Count == 0)
            {
                ptime = Time.time;
            }
            accelX.Add(currX);
            accelY.Add(currY);
            accelZ.Add(currZ);
        }
        if (accelX.Count == inputWidth)
        {
            ctime = Time.time - ptime;
            accelX.RemoveAt(0);
            accelY.RemoveAt(0);
            accelZ.RemoveAt(0);
            accelX.Add(currX);
            accelY.Add(currY);
            accelZ.Add(currZ);
        }
        line = currY;
    }

    // thread to manage the CNN
    void manageCNN()
    {

        while(true)
        {
            // sleeps so doesn't run while application is booting
            Thread.Sleep(100);

            // time is for debugging
            float prev = Time.time;

            // run the cnn
            evaluate();

            float len = Time.time - prev;
            diff = (int)((0.5 - len)*1000);
            if(diff < 0)
            {
                diff *= -1;
            }
        }
    }

    // run the CNN
    void evaluate ()
	{
        // only run CNN if we have enough accelerometer values 
        if (accelX.Count == inputWidth)
        {
            // convert from list to tensor
            // if tensor is 1 under, add dummy last value
            int i;
            for (i = 0; i < accelX.Count; i++)
            {
                inputTensor[0, 0, i, 0] = accelX[i];
                test = inputTensor[0, 0, i, 0];
            }
            if (i != inputWidth)
            {
                inputTensor[0, 0, inputWidth - 1, 0] = 0;
            }

            for (i = 0; i < accelY.Count; i++)
            {
                inputTensor[0, 0, i, 1] = accelY[i];
            }
            if (i != inputWidth)
            {
                inputTensor[0, 0, inputWidth - 1, 1] = 0;
            }

            for (i = 0; i < accelZ.Count; i++)
            {
                inputTensor[0, 0, i, 2] = accelZ[i];
            }
            if (i != inputWidth)
            {
                inputTensor[0, 0, inputWidth - 1, 2] = 0;
            }


            // tensor output variable
            float[,] recurrentTensor;

            // create tensorflow model
            using (var graph = new TFGraph())
            {

                graph.Import(graphModel.bytes);
                var session = new TFSession(graph);

                var runner = session.GetRunner();


                // do input tensor list to array and make it one dimensional
                TFTensor input = inputTensor;


                // set up input tensor and input
                runner.AddInput(graph["input_placeholder_x"][0], input);

                // set up output tensor
                runner.Fetch(graph["output_node"][0]);

                // run model
                recurrentTensor = runner.Run()[0].GetValue() as float[,];
                here = true;

                // dispose resources - keeps cnn from breaking down later
                session.Dispose();
                graph.Dispose();

            }

            // find the most confident answer
            float highVal = 0;
            int highInd = -1;
            sum = 0f;

            // *MAKE SURE ACTIVITYINDEXCHOICES MATCHES THE NUMBER OF CHOICES*
            for (int j = 0; j < activityIndexChoices; j++)
            {

                confidence = recurrentTensor[0, j];
                if (highInd > -1)
                {
                    if (recurrentTensor[0, j] > highVal)
                    {
                        highVal = confidence;
                        highInd = j;
                    }
                }
                else
                {
                    highVal = confidence;
                    highInd = j;
                }

                // debugging - sum should = 1 at the end
                sum += confidence;
            }

            // debugging
            test1 = recurrentTensor[0, 0];
            test2 = recurrentTensor[0, 1];
            test3 = recurrentTensor[0, 2];

            // used in movement to see if we should be moving
            index = highInd;
            countCNN++;
        }
       
    }

	// algorithm to determine if the user is looking around. Looking and walking generate similar gyro.accelerations, so we
	//want to ignore movements that could be spawned from looking around. Makes sure user's head orientation is in certain window
	bool look (double start, double curr, double diff)
	{
		//Determines if the user's current angle (curr) is within the window (start +/- diff)
		//Deals with wrap around values (eulerAngles is in range 0 to 360)
		if ((start + diff) > 360f) {
			if (((curr >= 0f) && (curr <= (start + diff - 360f))) || ((((start - diff) <= curr) && (curr <= 360f)))) {
				return false;
			}
		} else if ((start - diff) < 0f) {
			if (((0f <= curr) && (curr <= (start + diff))) || (((start - diff + 360f) <= curr) && (curr <= 360f))) {
				return false;
			}
		} else if (((start + diff) <= curr) && (curr <= (start + diff))) {
			return false;
		}
		return true;
	}

	// if the user is walking, moves them in correct direction with varying velocities
	// also sets velocity to 0 if it is determined that the user is no longer walking
	void move ()
	{
		// get the yaw of the subject to allow for movement in the look direction
		yaw = InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.y;
		// convert that value into radians because math uses radians
		rad = yaw * Mathf.Deg2Rad;
		// map that value onto the unit circle to faciliate movement in the look direction
		zVal = Mathf.Cos (rad);
		xVal = Mathf.Sin (rad);

		bool looking = (look (eulerX, InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.x, 20f) || look (eulerZ, InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.z, 20f));

		if (index != standIndex && index != lookIndex) {
			walking = true;
		}
        // if the user isn't looking and is walking then set the velocity based on increasing or decreasing speed
        if (!looking && walking)
        {
            if ((display.acceleration.y >= 0.75f || display.acceleration.y <= -0.75f))
            {
                if (wasTwo)
                { //we are transitioning from phase 2 to 1
                    method1StartTimeGrow = Time.time;
                    wasTwo = false;
                    wasOne = true;
                }
            }
            else
            {
                if (wasOne)
                {
                    method1StartTimeDecay = Time.time;
                    wasOne = false;
                    wasTwo = true;
                }
            }
            if ((display.acceleration.y >= 0.75f || display.acceleration.y <= -0.75f))
            {
                velocity = 1.65f - (1.65f - velocity) * Mathf.Exp((method1StartTimeGrow - Time.time) / 0.2f); //grow
            }
            else
            {
                // if the acceleration values are low, indicates the user is walking slowly, and exponentially decrease the velocity to 0
                velocity = 0.0f - (0.0f - velocity) * Mathf.Exp((method1StartTimeDecay - Time.time) / decayRate); //decay
            }
        }
        else
        {
            velocity = 0f;
        }

        // multiply intended speed (called velocity) by delta time to get a distance, then multiply that distamce
        // by the unit vector in the look direction to get displacement.
        transform.Translate (xVal * velocity * Time.fixedDeltaTime, 0, zVal * velocity * Time.fixedDeltaTime);
	}

	#region NetworkingCode

	//Declare a client node
	NetworkClient myClient;
	//Define two types of data, one for setup (unused) and one for actual data
	const short MESSAGE_DATA = 880;
	const short MESSAGE_INFO = 881;
	//Server address is Flynn, tracker address is Baines, port is for broadcasting
	const string SERVER_ADDRESS = "192.168.1.2";
	const string TRACKER_ADDRESS = "192.168.1.100";
	const int SERVER_PORT = 5000;

	//Message and message text are now depreciated, were used for debugging
	public string message = "";
	public Text messageText;

	//Connection ID for the client server interaction
	public int _connectionID;
	//transform data that is being read from the clien
	public static Vector3 _pos = new Vector3 ();
	public static Vector3 _euler = new Vector3 ();

	// Create a client and connect to the server port
	public void SetupClient ()
	{
		myClient = new NetworkClient (); //Instantiate the client
		myClient.RegisterHandler (MESSAGE_DATA, DataReceptionHandler); //Register a handler to handle incoming message data
		myClient.RegisterHandler (MsgType.Connect, OnConnected); //Register a handler to handle a connection to the server (will setup important info
		myClient.Connect (SERVER_ADDRESS, SERVER_PORT); //Attempt to connect, this will send a connect request which is good if the OnConnected fires
	}

	// client function to recognized a connection
	public void OnConnected (NetworkMessage netMsg)
	{
		_connectionID = netMsg.conn.connectionId; //Keep connection id, not really neccesary I don't think
	}

	// Clinet function that fires when a disconnect occurs (probably unnecessary
	public void OnDisconnected (NetworkMessage netMsg)
	{
		_connectionID = -1;
	}

	//I actually don't know for sure if this is useful. I believe that this is erroneously put here and was duplicated in TDServer code.
	public void DataReceptionHandler (NetworkMessage _transformData)
	{
		TDMessage transformData = _transformData.ReadMessage<TDMessage> ();
		_pos = transformData._pos;
		_euler = transformData._euler;
	}

	#endregion
}