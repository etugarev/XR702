using UnityEngine;
using System.Collections;

using Globals;

public class PlayerAnimator : MonoBehaviour
{

    #region Public Fields & Properties

    
    #endregion

    #region Private Fields & Properties
	private float airVelocity;

	private Animator animator;
	private PlayerController playerController;

    
    #endregion

    #region Getters & Setters

    #endregion

    #region System Methods
    // Use this for initialization
    private void Start()
    {
		animator = GetComponent<Animator> ();
		playerController = GetComponent<PlayerController> ();
    }

    // Update is called once per frame
    private void Update()
    {

		float speedMult = playerController.isRunning ? 1f : 0.5f;
		animator.SetFloat (AnimatorCondition.Speed, Input.GetAxis(PlayerInput.Vertical) * speedMult);
		animator.SetFloat (AnimatorCondition.Direction, Input.GetAxis(PlayerInput.Horizontal));

		animator.SetBool (AnimatorCondition.Grounded, playerController.grounded);  
		animator.SetBool (AnimatorCondition.isRunning, playerController.isRunning);  

		if (playerController.grounded) 
		{
			airVelocity = 0;
		} else 
		{
			airVelocity -= Time.time;
		}

		animator.SetFloat (AnimatorCondition.AirVelocity, airVelocity);

		float isArmed = playerController.isArmed ? 1f : 0f; 

		animator.SetLayerWeight (AnimatorLayer.WeaponLayer, isArmed);

//		if (playerController.isArmed) 
//		{
//			animator.SetLayerWeight (AnimatorLayer.WeaponLayer, 1f);
//		} else {
//			animator.SetLayerWeight (AnimatorLayer.WeaponLayer, 0f);			
//		}

		//Debug.Log (Input.GetAxis(PlayerInput.Vertical));

    }
    #endregion

    #region Custom Methods

    #endregion
}
