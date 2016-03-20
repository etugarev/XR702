﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public Transform spawnSpot;

	public bool offlineMode = false;

	// Use this for initialization
	void Start () 
	{
		Connect ();
	}

	void Connect() 
	{
		if (offlineMode) 
		{
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby ();
		} else 
		{
			PhotonNetwork.ConnectUsingSettings ("XR702 1.0.0 Beta");
		}
	}

	void OnGUI() 
	{
		string status = PhotonNetwork.connectionStateDetailed.ToString (); 
		//GUILayout.Label (status);
		GUI.Label(new Rect(10, 10, 200, 50), status);

//		Debug.Log (status);
	}

	void OnJoinedLobby()
	{
		Debug.Log ("OnJoinedLobby");
		//PhotonNetwork.JoinRandomRoom ();
		PhotonNetwork.JoinOrCreateRoom("default", new RoomOptions(), null);
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log ("OnPhotonRandomJoinFailed");
//		PhotonNetwork.JoinRoom (null);
	}

	void OnJoinedRoom()
	{
		Debug.Log ("OnJoinedRoom");
		SpawnMyPlayer ();
	}

	void SpawnMyPlayer()
	{
		GameObject player =  PhotonNetwork.Instantiate ("Remy", spawnSpot.position, spawnSpot.rotation, 0);
		//player.GetComponent<CharacterController> ().enabled = true;
		player.GetComponent<Animator> ().enabled = true;
		player.GetComponent<PlayerController> ().enabled = true;
		player.GetComponent<Weapon> ().enabled = true;
		player.GetComponent<PlayerAnimator> ().enabled = true;
//		player.GetComponent<HeadLookController> ().enabled = true;
		player.GetComponent<CharacterMotor> ().enabled = true;
		player.GetComponent<Inventory> ().enabled = true;

		player.GetComponentInChildren<Camera> ().enabled = true;

	}
}
