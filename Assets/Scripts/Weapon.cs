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

	public Camera cam;    
    #endregion

    #region Private Fields & Properties
	private float _fireCounter;

	private Ray _ray;

	private PlayerController _playerController;

	private AudioSource _audioSource;
    #endregion

    #region Getters & Setters

    #endregion

    #region System Methods
    // Use this for initialization
    private void Start()
    {
		_playerController = GetComponent<PlayerController> ();
		_audioSource = muzzle.GetComponent<AudioSource> ();

    }

	public bool isWeaponAvailable()
	{
		return _playerController.isArmed && rightHandAttachPoint.childCount > 1; 
	}

    // Update is called once per frame
    private void LateUpdate()
    {
		if (!isWeaponAvailable()) 
		{
			return;
		}

		gunRig = rightHandAttachPoint.GetChild (0);
		muzzle = gunRig.GetChild (0);

		_ray = cam.ScreenPointToRay (new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

		if (_playerController.aim) 
		{
			gunRig.forward = _ray.direction;
		} 
		else 
		{
			gunRig.forward = transform.forward;	
		}

		if (Input.GetButton (PlayerInput.Fire1) && _fireCounter > fireDelay) {
			_audioSource.Play ();
			_fireCounter = 0f;

			RaycastHit hit;
			if (_playerController.aim) {
				if (Physics.Raycast (_ray, out hit, 100f)) {
					Instantiate (bulletHole, hit.point, Quaternion.FromToRotation (Vector3.up, hit.normal));
				}
			} else {
				if (Physics.Raycast (muzzle.position, muzzle.forward, out hit, 100f)) {
					Instantiate (bulletHole, hit.point, Quaternion.FromToRotation (Vector3.up, hit.normal));					

				}
			}

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
