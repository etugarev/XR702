using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float hitPoints = 100f;
	float currentHitPoints;

	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDamage(float amount) {
		currentHitPoints -= amount;

		if (currentHitPoints <= 0) {
			Die ();
		}
	}

	void Die() {
		Destroy (gameObject);
	}

}
