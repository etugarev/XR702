using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public Transform spawnSpot;

	// Use this for initialization
	void Start () 
	{
		Connect ();
	}

	void Connect() 
	{
		PhotonNetwork.offlineMode = true;
		PhotonNetwork.ConnectUsingSettings ("XR702 1.0.0 Beta");
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
		player.GetComponent<PlayerController> ().enabled = true;
		player.GetComponent<HeadLookController> ().enabled = true;
		player.GetComponent<Inventory> ().enabled = true;
		player.GetComponent<CharacterMotor> ().enabled = true;
		player.GetComponent<PlayerAnimator> ().enabled = true;


		player.GetComponentInChildren<Camera> ().enabled = true;

	}
}
