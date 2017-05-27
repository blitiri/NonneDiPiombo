using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBulletManager : DefaultBulletManager {
	public float bulletAccel;
	public float initialRadius;
	public float maxRadius;
	public float radiusExplansion;
	public float explosionDelay = 1f;
	private SphereCollider explosionCollider;
	private GameObject explosion;
//	[HideInInspector]
	public bool wallCollision;
	public GameObject explosionPrefab;

	protected override void DefaultMovement(){
		if (isMoving) {
			bulletSpeed += bulletAccel * Time.deltaTime;
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
		}
	}

	protected override void Trigger(){
		isMoving = false;

		explosion = Instantiate (explosionPrefab, transform.position, Quaternion.identity) as GameObject;
		explosion.tag = this.gameObject.tag;


		bulletMeshRender.enabled = false;
		bulletCapsuleCollider.enabled = false;
		bulletRb.velocity = Vector3.zero;
		Destroy (this.gameObject, 1f);

		Destroy (explosion, explosionDelay); 
	}

}
