﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBulletManager : DefaultBulletManager {
	public float bulletAccel;
	public float explosionDelay = 1f;
	private SphereCollider explosionCollider;
	private GameObject explosion;

	public GameObject explosionPrefab;

	protected override void DefaultMovement(){
		if (isMoving) {
			bulletSpeed += (bulletSpeed * bulletAccel) * Time.deltaTime;
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
			Destroy (this.gameObject, 5.0f);
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
