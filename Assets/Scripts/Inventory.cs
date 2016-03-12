using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	#region Public Fields & Properties
	public GameObject[] items; 

	public Transform inHands;

	#endregion

	#region Private Fields & Properties
	private PlayerController _playerController;


	#endregion

	#region Getters & Setters

	#endregion

	#region System Methods
	// Use this for initialization
	void Start () {
		_playerController = GetComponent<PlayerController> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_playerController.isArmed && inHands.childCount == 0) {
			GameObject go = Instantiate(items[0], inHands.position, Quaternion.identity) as GameObject;
			go.transform.parent = inHands;
		}
	
	}
	#endregion
}
