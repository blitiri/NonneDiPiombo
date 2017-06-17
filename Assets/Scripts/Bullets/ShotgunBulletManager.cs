using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletManager : DefaultBulletManager {
	public float numberOfBullets;
	public float bulletDistance;
	public float spreadValue;
	private float timer;
	private ParticleSystem trail;
	private ParticleSystem.MainModule trailMain;

	private bool canSpread = true;
	protected override void Components(){
		bulletRb = GetComponent<Rigidbody> ();
		bulletMeshRender = GetComponent<MeshRenderer> ();
		bulletSphereCollider = GetComponent<SphereCollider> ();
		trail = GetComponentInChildren<ParticleSystem> ();
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
			if (timer < bulletDistance) {
				bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
				timer += Time.deltaTime;
			} else {
				Trigger ();
			}
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
	}
}
