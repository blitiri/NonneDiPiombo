using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletManager : DefaultBulletManager {
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
		Destroy (this.gameObject, 0.03f);
	}
}
