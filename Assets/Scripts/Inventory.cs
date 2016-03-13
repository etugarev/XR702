using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	#region Public Fields & Properties
	public GameObject[] items; 

	public Transform leftHandAttachPoint;
	public Transform rightHandAttachPoint;

//	public Transform inHands;

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

		Vector3 hands = leftHandAttachPoint.position - rightHandAttachPoint.position;
		// Add gun 
//		if (_playerController.isArmed && inHands.childCount == 0) {
//			GameObject go = Instantiate(items[0], inHands.position, inHands.rotation) as GameObject;
//			go.transform.parent = inHands;
//		}
//		// Remove gun when unarmed
//		if (!_playerController.isArmed && inHands.childCount == 1) {
//			GameObject go = inHands.GetChild (0).gameObject;
//			Destroy (go);
//		}


		 // Add gun 
		if (_playerController.isArmed && rightHandAttachPoint.childCount == 0) {
			GameObject go = Instantiate(items[0], rightHandAttachPoint.position, rightHandAttachPoint.rotation) as GameObject;
			go.transform.parent = rightHandAttachPoint;
		}
		// Remove gun when unarmed
		if (!_playerController.isArmed && rightHandAttachPoint.childCount == 1) {
			GameObject go = rightHandAttachPoint.GetChild (0).gameObject;
			Destroy (go);
		}

	
	}
	#endregion
}
