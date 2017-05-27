using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGBulletManager : DefaultBulletManager {
	public float numberOfBullets;
	public float spreadValue;
	public float shootInterval;
	private bool canSpread = true;
	private GameObject initialPosition;


	void Start(){
		initialPosition = GameObject.FindGameObjectWithTag ("SMGBulletSpawn");
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
		}
	}

	protected IEnumerator SMGBulletsBehaviour(){
		for (int i = 0; i < numberOfBullets; i++) {
			Quaternion yValue = Quaternion.Euler (0, 0, Random.Range (-spreadValue, spreadValue));
			yield return new WaitForSeconds (shootInterval * i);
			GameObject proj = Instantiate (this.gameObject, initialPosition.transform.position, initialPosition.transform.rotation * yValue);
			Rigidbody projRb = proj.GetComponent<Rigidbody> ();
			CapsuleCollider projCollider = proj.GetComponent<CapsuleCollider> ();
			MeshRenderer projRenderer = proj.GetComponent<MeshRenderer> ();

			SMGBulletManager projBulletManager = proj.GetComponent<SMGBulletManager> ();

			projRenderer.enabled = true;
			projCollider.enabled = true;
			projBulletManager.canSpread = false;
		}
		canSpread = false;
	}
}
