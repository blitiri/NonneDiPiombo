﻿using UnityEngine;
using System.Collections;

public class DefaultBulletManager : MonoBehaviour {
	
	protected MeshRenderer bulletMeshRender;
	protected Rigidbody bulletRb;
	protected CapsuleCollider bulletCapsuleCollider;
	protected SphereCollider bulletSphereCollider;
	protected bool isMoving = true;
	public float bulletSpeed;

	void Awake(){
		Components ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag.Equals ("Wall")) {
			Trigger ();
		}
	}

	void FixedUpdate(){
		DefaultMovement ();
	}


	protected virtual void DefaultMovement(){
		if (isMoving) {
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
		}
	}

	protected virtual void Components(){
		bulletRb = GetComponent<Rigidbody> ();
		bulletMeshRender = GetComponent<MeshRenderer> ();
		bulletCapsuleCollider = GetComponent<CapsuleCollider> ();
	}

	protected virtual void Trigger(){
		isMoving = false;
		bulletMeshRender.enabled = false;
		bulletCapsuleCollider.enabled = false;
		bulletRb.velocity = Vector3.zero;
		Destroy (this.gameObject, 1f);
	}
}
