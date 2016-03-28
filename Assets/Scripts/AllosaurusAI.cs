using UnityEngine;
using System.Collections;

public class AllosaurusAI : MonoBehaviour {

	public int moveRadius;

	private float angle;

	private Vector3 target;

	private SimpleAllosaurusCharacter allosaurus;

	// Use this for initialization
	void Start () {
		allosaurus = GetComponent<SimpleAllosaurusCharacter> ();
		SetDestination();
	
	}

	private void SetDestination() {
		angle = Random.Range (0, 2 * Mathf.PI);
		target.x = transform.position.x + moveRadius * Mathf.Cos (angle);
		target.z = transform.position.z + moveRadius * Mathf.Sin (angle);
	  		 
		transform.Rotate (0, Mathf.Rad2Deg * angle, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		Vector3 relativePos = target - transform.position;
//		Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
//        
//		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime);
//		transform.rotation = rotation;



		allosaurus.Move(3f, 0);

		float remainingDestanceX = target.x - transform.position.x; 
		float remainingDestanceZ = target.z - transform.position.z; 
//		Debug.Log ("remainingDestance");

		if (remainingDestanceX < 2f || remainingDestanceZ < 2f) {
//			Debug.Log ("SetDestination");
			
			SetDestination ();
		}

//		if (relativePos.AlmostEquals (Vector3.zero, 2f)) {
//			SetDestination ();
//		}
		

	
	}
}
