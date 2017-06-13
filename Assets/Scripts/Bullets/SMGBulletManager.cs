using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGBulletManager : DefaultBulletManager {
	public float numberOfBullets;
	public float spreadValue;
	public float shootInterval;
	public bool canSpread = true;
	private GameObject initialPosition;
	private WeaponControl weapon;
	private GameObject player;
	private string playerTag;
	private Quaternion yValue;

	void Awake(){
		Components ();

	}

	void Start(){
		playerTag = "Player" + Utility.GetPlayerIndexFromBullet (this.gameObject.tag);
		player = GameObject.FindGameObjectWithTag (playerTag);
		weapon = player.GetComponentInChildren<WeaponControl> ();

		Quaternion yValue = Quaternion.Euler (0, 0, Random.Range (-spreadValue, spreadValue));
		bulletRb.transform.rotation = bulletRb.transform.rotation * yValue;
	}

	protected override void Components(){
		bulletRb = GetComponent<Rigidbody> ();
		bulletMeshRender = GetComponent<MeshRenderer> ();
		bulletCapsuleCollider = GetComponent<CapsuleCollider> ();
	}

	protected override void Trigger(){
		isMoving = false;
		bulletMeshRender.enabled = false;
		bulletCapsuleCollider.enabled = false;
		bulletRb.velocity = Vector3.zero;
		Destroy (this.gameObject, 1f);
	}

	protected override void DefaultMovement(){
		if (isMoving) {
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
		}
	
		if (canSpread) {
				StartCoroutine (SMGBulletsBehaviour ());
				canSpread = false;
		}
	}

	protected IEnumerator SMGBulletsBehaviour(){
		for (int i = 0; i < numberOfBullets; i++) {
			yield return new WaitForSeconds (shootInterval);
			weapon.ShootSMG ("Player" + Utility.GetPlayerIndexFromBullet (this.gameObject.tag).ToString (), spreadValue);
		}
		canSpread = false;
	}

//	protected IEnumerator SMGBulletsBehaviour(){
//		
//			Quaternion yValue = Quaternion.Euler (0, 0, Random.Range (-spreadValue, spreadValue));
//			Debug.Log ("Shoot");
//			GameObject proj = Instantiate (this.gameObject, initialPosition.transform.position, initialPosition.transform.rotation * yValue);
//
//			Rigidbody projRb = proj.GetComponent<Rigidbody> ();
//			CapsuleCollider projCollider = proj.GetComponent<CapsuleCollider> ();
//			MeshRenderer projRenderer = proj.GetComponent<MeshRenderer> ();
//			SMGBulletManager projBulletManager = proj.GetComponent<SMGBulletManager> ();
//
//			projRenderer.enabled = true;
//			projCollider.enabled = true;
//			projBulletManager.canSpread = false;
//
//			yield return  null;
//			canSpread = false;
//	}
}
