using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;

namespace XR702.Characters.Player
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
	[RequireComponent(typeof (AudioSource))]
    public class UserControl : MonoBehaviour
    {
		[Serializable]
		public class MovementSettings
		{
			public float ForwardSpeed = 8.0f;   // Speed when walking forward
			public float BackwardSpeed = 4.0f;  // Speed when walking backwards
			public float StrafeSpeed = 4.0f;    // Speed when walking sideways
			public float RunMultiplier = 2.0f;   // Speed when sprinting
			public KeyCode RunKey = KeyCode.LeftShift;
			public float JumpForce = 30f;
			public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
			[HideInInspector] public float CurrentTargetSpeed = 8f;

			#if !MOBILE_INPUT
			private bool m_Running;
			#endif

			public void UpdateDesiredTargetSpeed(Vector2 input)
			{
				if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0)
				{
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0)
				{
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed;
				}
				#if !MOBILE_INPUT
				if (Input.GetKey(RunKey))
				{
					CurrentTargetSpeed *= RunMultiplier;
					m_Running = true;
				}
				else
				{
					m_Running = false;
				}
				#endif
			}

			#if !MOBILE_INPUT
			public bool Running
			{
				get { return m_Running; }
			}
			#endif
		}

        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

		public MovementSettings movementSettings = new MovementSettings();
		private Vector3 m_GroundContactNormal;


		[SerializeField] private MouseLook m_MouseLook;
		[SerializeField] private float m_StepInterval;
		[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
		[SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
		[SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

		private AudioSource m_AudioSource;

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();

			m_MouseLook.Init(transform , m_Cam);
			m_AudioSource = GetComponent<AudioSource>();
        }


        private void Update()
        {
			RotateView();
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
				if (m_Jump) {
					PlayJumpSound ();
				}
            }
        }

		private void RotateView()
		{
			m_MouseLook.LookRotation (transform, m_Cam);
		}

		private void PlayJumpSound()
		{
			m_AudioSource.clip = m_JumpSound;
			m_AudioSource.Play();
		}

		private Vector2 GetInput()
		{
			Vector2 input = new Vector2
			{
				x = CrossPlatformInputManager.GetAxis("Horizontal"),
				y = CrossPlatformInputManager.GetAxis("Vertical")
			};
			movementSettings.UpdateDesiredTargetSpeed(input);
			return input;
		}


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
			Vector2 input = GetInput();
//            float h = CrossPlatformInputManager.GetAxis("Horizontal");
//            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
				Vector3 desiredMove = m_Cam.transform.forward*input.y + m_Cam.transform.right*input.x;

//				Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

//				desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
//				desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
//				desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;

				m_Character.Move (desiredMove, crouch, m_Jump, movementSettings.CurrentTargetSpeed);

                // calculate camera relative direction to move:
//                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
//                m_Move = v*m_CamForward + h*m_Cam.right;
            }
//            else
//            {
//                // we use world-relative directions in the case of no main camera
//                m_Move = v*Vector3.forward + h*Vector3.right;
//            }
//#if !MOBILE_INPUT
//			// walk speed multiplier
//	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
//#endif

            // pass all parameters to the character control script
//            m_Character.Move(m_Move, crouch, m_Jump);

            m_Jump = false;
        }
    }
}
