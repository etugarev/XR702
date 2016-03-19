using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	#region Public Fields & Properties
	public string[] items; 

	public Transform leftHandAttachPoint;
	public Transform rightHandAttachPoint;

	#endregion

	#region Private Fields & Properties
	private PlayerController player;


	#endregion

	#region Getters & Setters

	#endregion

	#region System Methods
	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		 // Add gun 
		if (player.isArmed && rightHandAttachPoint.childCount == 0) {
			GameObject go = PhotonNetwork.Instantiate(items[0], rightHandAttachPoint.position, rightHandAttachPoint.rotation, 0);
			go.transform.parent = rightHandAttachPoint;
		} else 
		// Remove gun when unarmed
		if (!player.isArmed && rightHandAttachPoint.childCount == 1) {
			GameObject go = rightHandAttachPoint.GetChild (0).gameObject;
			PhotonNetwork.Destroy (go);
		}

	
	}
	#endregion
}
