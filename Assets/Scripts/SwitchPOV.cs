using UnityEngine;
using System.Collections;

public class SwitchPOV : MonoBehaviour {

	public GameObject firstPersonCamera;
	public GameObject thirdPesronCamera;
	private bool isFirstPersonMode; 

	// Use this for initialization
	void Start () {
		firstPersonCamera.SetActive (true);
		thirdPesronCamera.SetActive (false);
		isFirstPersonMode = true;
    	  
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.V)) {
			if (isFirstPersonMode) {
				thirdPesronCamera.SetActive (true);
				firstPersonCamera.SetActive (false);				
			} else {
				firstPersonCamera.SetActive (true);
				thirdPesronCamera.SetActive (false);					
			}
			isFirstPersonMode = !isFirstPersonMode;
		}
	}
}
