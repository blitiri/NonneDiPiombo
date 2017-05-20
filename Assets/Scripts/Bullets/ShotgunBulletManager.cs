using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletManager : DefaultBulletManager {
	public float numberOfBullets;
	public float bulletDistance;
	public float spreadValue;

	private bool canSpread = true;
	protected override void Components(){
		bulletRb = GetComponent<Rigidbody> ();
		bulletMeshRender = GetComponent<MeshRenderer> ();
		bulletSphereCollider = GetComponent<SphereCollider> ();
	}

	protected override void Trigger(){
		isMoving = false;
		bulletMeshRender.enabled = false;
		bulletSphereCollider.enabled = false;
		bulletRb.velocity = Vector3.zero;
		Destroy (this.gameObject, 1f);
	}

	protected override void DefaultMovement(){
		if (isMoving) {
			
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
		}
		if (canSpread) {
			for (int i = 0; i < numberOfBullets; i++) {
				Quaternion yValue = Quaternion.Euler (0, 0, Random.Range (-spreadValue, spreadValue));
				GameObject proj = Instantiate (this.gameObject, transform.position, transform.rotation * yValue);
				Rigidbody projRb = proj.GetComponent<Rigidbody> ();

				ShotgunBulletManager projBulletManager = proj.GetComponent<ShotgunBulletManager> ();
				projBulletManager.canSpread = false;

				canSpread = false;
			}
		}
		
		Destroy (this.gameObject, bulletDistance);
	}
}
