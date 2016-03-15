using UnityEngine;

using Globals;

public class NetworkCharacter2 : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

			PlayerController player = GetComponent<PlayerController>();
//			stream.SendNext((int)player._characterState);

			Animator animator = GetComponent<Animator> ();
			stream.SendNext (animator.GetFloat (AnimatorCondition.Speed));
			stream.SendNext (animator.GetFloat (AnimatorCondition.Direction));
			stream.SendNext (animator.GetFloat (AnimatorCondition.AirVelocity));

			stream.SendNext (animator.GetBool (AnimatorCondition.Grounded));
			stream.SendNext (animator.GetBool (AnimatorCondition.isRunning));
			stream.SendNext (player.isArmed);
        }
        else
        {
            // Network player, receive data
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();

			PlayerController player = GetComponent<PlayerController>();
//            player._characterState = (CharacterState)stream.ReceiveNext();

			Animator animator = GetComponent<Animator> ();

			animator.SetFloat (AnimatorCondition.Speed, (float)stream.ReceiveNext ());
			animator.SetFloat (AnimatorCondition.Direction, (float)stream.ReceiveNext ());
			animator.SetFloat (AnimatorCondition.AirVelocity, (float)stream.ReceiveNext ());

			animator.SetBool (AnimatorCondition.Grounded, (bool)stream.ReceiveNext ());
			animator.SetBool (AnimatorCondition.isRunning, (bool)stream.ReceiveNext ());

			bool isArmed = (bool)stream.ReceiveNext (); 
			animator.SetLayerWeight (AnimatorLayer.WeaponLayer, isArmed ? 1f : 0f);
			player.isArmed = isArmed;




        }
    }
}
