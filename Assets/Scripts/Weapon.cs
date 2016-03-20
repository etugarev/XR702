using UnityEngine;
using System.Collections;

using Globals;

public class Weapon : MonoBehaviour {

	#region Public Fields & Properties
	public float lerpSpeed = 2f;
	public float fireDelay;

	private Transform gunRig;
	private Transform muzzle;

	public GameObject bulletHole;
	public GameObject muzzleFlash;

	public Transform rightHandAttachPoint;
	public Transform rightHandUsePoint;
	public GameObject rightIKLimb;

	public Camera cam;    
    #endregion

    #region Private Fields & Properties
	private float _fireCounter;

	private Ray ray;

	private PlayerController player;

	private AudioSource _audioSource;
    #endregion

    #region Getters & Setters

    #endregion

    #region System Methods
    // Use this for initialization
    private void Start()
    {
		player = GetComponent<PlayerController> ();

    }

	public bool isWeaponAvailable()
	{
		return player.isArmed && rightHandAttachPoint.childCount > 0; 
	}

    // Update is called once per frame
    private void LateUpdate()
    {
		bool canShoot = isWeaponAvailable (); 
		rightIKLimb.SetActive (canShoot);

		if (!canShoot) 
		{
			return;
		}

		gunRig = rightHandAttachPoint.GetChild (0);
		muzzle = gunRig.GetChild (0);


		ray = cam.ScreenPointToRay (new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

//		if (player.aim) 
//		{
//			gunRig.forward = ray.direction;
//		} 
//		else 
//		{
//			gunRig.forward = rightHandUsePoint.forward;	
//		}

		if (Input.GetButton (PlayerInput.Fire1) && _fireCounter > fireDelay) {
			
			AudioSource audioSource = muzzle.GetComponent<AudioSource> ();
			audioSource.Play ();
			_fireCounter = 0f;

			RaycastHit hit;
//			if (player.aim) {
				if (Physics.Raycast (ray, out hit, 50f)) {
					Instantiate (bulletHole, hit.point, Quaternion.FromToRotation (Vector3.up, hit.normal));
				    
				    // we hit something
					Debug.Log("We hit: " + hit.transform.gameObject.name);

				    Health h = hit.transform.GetComponent<Health> ();
				 
					if (h != null) {
					h.TakeDamage (25f);
					}
				}
//			} else {
//				if (Physics.Raycast (muzzle.position, muzzle.forward, out hit, 50f)) {
//					Instantiate (bulletHole, hit.point, Quaternion.FromToRotation (Vector3.up, hit.normal));					
//
//				}
//			}

			StartCoroutine (MuzzleFlash ());
		}

		_fireCounter += Time.deltaTime;
    }
    #endregion

    #region Custom Methods
	private IEnumerator MuzzleFlash()
	{
		GameObject go = (GameObject) Instantiate (muzzleFlash, muzzle.position, Quaternion.Euler (muzzle.eulerAngles.x, muzzle.eulerAngles.y - 90, muzzle.eulerAngles.z));
		go.transform.parent = muzzle;
		yield return new WaitForSeconds (go.GetComponent<ParticleSystem>().duration + 0.05f);
		Destroy (go);
	}
    
    #endregion
}
