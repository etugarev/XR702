using UnityEngine;
using System.Collections;

using Globals;

public class PlayerCamera : MonoBehaviour
{

    #region Public Fields & Properties
	public float yMinLimit = -50f;
	public float yMaxLimit = 50f;

	public float normalFOV = 60f;
	public float zoomFOV = 30f;

	// Speed to switch between FOV
	public float lerpSpeed = 8.0f;

	public float positionLerp = 6f;
	public float normalHeight = 1.5f;
	public float normalAimHeight = 1.8f;

	public float minHeight = 0.5f;
	public float maxHeight = 2f;

	public float normalDistance = 2.5f;
	public float normalAimDistance = 1f;

	public float minDistance = 0.2f;
	public float maxDistance = 2.5f;

	public bool orbit;

	public Transform target;
	public Transform player;

	public Vector2 speed = new Vector2 (135f, 135f);
	public Vector2 aimSpeed = new Vector2 (100f, 100f);
	public Vector2 maxSpeed = new Vector2 (100f, 100f);

	public LayerMask hitLayer;

	public Vector3 normalDirection = new Vector3 (-1f, 0f, 0.3f);
	public Vector3 aimDirection = new Vector3 (-1f, 0f, 0.7f);
    #endregion

    #region Private Fields & Properties
	private float _x = 0.0f;
	private float _y = 0.0f;

	private float _deltaTime;
	private float _targetDistance;
	private float _targetHeight;

	private Transform _camTransform;

	private Camera _camera;

	private Vector3 _position;
	private Vector3 _camDir;
	private Vector3 _camPos;

	private Quaternion roation;

	private PlayerController playerController;

    #endregion

    #region Getters & Setters
	public float Y { get { return _y; } }
	public float X { get { return _x; } }    
    #endregion

    #region System Methods
    private void Start()
    {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		if (target == null || player == null) 
		{
			Destroy (this);
			return;
		}

		target.parent = null;

		_camTransform = transform;

		Vector3 angles = _camTransform.eulerAngles;

		_x = angles.y;
		_y = angles.x;

		playerController = player.GetComponent<PlayerController> ();

		_targetDistance = normalDistance;

		_camPos = player.position + new Vector3 (0, normalHeight, 0);  
    }

    // Update is called once per frame
    private void Update()
    {
		if (Input.GetButtonDown (PlayerInput.Cancel)) 
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = (CursorLockMode.Locked != Cursor.lockState);
		}

		if (Input.GetAxis (PlayerInput.Horizontal) != 0.0 || Input.GetAxis (PlayerInput.Vertical) != 0.0 || playerController.aim)
		{
			GoToOrbitMode (false);	
		}

		if (!orbit && playerController.idleTimer > 0.1)
		{
			GoToOrbitMode (true);
		}
    }

    public void LateUpdate()
    {
		_deltaTime = Time.deltaTime;

		GetInput ();

		RotatePlayer ();

		CameraMovement ();
    } 
    #endregion

    #region Custom Methods
	private void GoToOrbitMode(bool state)
	{
		orbit = state;

		playerController.idleTimer = 0.0f;
	}

	private void GetInput()
	{
		Vector2 a = playerController.aim ? aimSpeed : speed;

		_x += Mathf.Clamp (Input.GetAxis(PlayerInput.MouseX) * a.x, -maxSpeed.x, maxSpeed.x) * _deltaTime;

		_y -= Mathf.Clamp (Input.GetAxis(PlayerInput.MouseY) * a.y, -maxSpeed.y, maxSpeed.y) * _deltaTime;
		_y = ClampAngle (_y, yMinLimit, yMaxLimit);

	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360) 
		{
			angle += 360;
		}

		if (angle > 360)
		{
			angle -= 360;
		}

		return Mathf.Clamp (angle, min, max);
	}

	private void RotatePlayer()
	{
		if (!orbit) 
		{
			playerController.targetYRotation = _x;
		}
	}

	private void CameraMovement()
	{
		Camera _camera = GetComponent<Camera> ();
		if (playerController.aim) 
		{
			_camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, zoomFOV, _deltaTime * lerpSpeed);

			_camDir = (aimDirection.x * target.forward) + (aimDirection.z * target.right);
			_targetHeight = normalAimHeight;
			_targetDistance = normalAimDistance;
		}
		else
		{
			_camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, normalFOV, _deltaTime * lerpSpeed);

			_camDir = (normalDirection.x * target.forward) + (normalDirection.z * target.right);
			_targetHeight = normalHeight;
			_targetDistance = normalDistance;			
		}

		_camDir = _camDir.normalized;

		_camPos = player.position + new Vector3 (0, _targetHeight, 0);

		RaycastHit hit;

		if (Physics.Raycast (_camPos, _camDir, out hit, _targetDistance + 0.2f, hitLayer)) 
		{
			float t = hit.distance - 0.1f;
			t -= minDistance;
			t /= (_targetDistance - minDistance);

			_targetHeight = Mathf.Lerp (maxHeight, _targetHeight, Mathf.Clamp(t, 0.0f, 1.0f));
			_camPos = player.position + new Vector3 (0, _targetHeight, 0);

			_targetDistance = hit.distance - 0.1f;
		}

		Vector3 lookPoint = _camPos;
		lookPoint += (target.right * Vector3.Dot ((_camDir * _targetDistance), target.right));

		_camTransform.position = _camPos + (_camDir * _targetDistance);
		_camTransform.LookAt (lookPoint);

		target.position = _camPos;
		target.rotation = Quaternion.Euler (_y, _x, 0);
	}    
    #endregion
}
