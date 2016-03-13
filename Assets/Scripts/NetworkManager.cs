using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Connect ();
	}

	void Connect() 
	{
		PhotonNetwork.ConnectUsingSettings ("XR702 1.0.0 Beta");
	}

	void OnGUI() 
	{
		string status = PhotonNetwork.connectionStateDetailed.ToString (); 
		//GUILayout.Label (status);
		GUI.Label(new Rect(10, 10, 200, 50), status);

		Debug.Log (status);
	}
}
