﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBazooka : MonoBehaviour {
	private BazookaBulletManager bazooka;
	private SphereCollider explosionCollider;

	public float initialRadius;
	public float maxRadius;
	public float radiusExpansion;

	public bool wallCollision;

	void Start(){
		explosionCollider = GetComponent<SphereCollider> ();
		explosionCollider.radius = initialRadius;
		initialRadius = 0;
	}

	void Update(){
		if (initialRadius < maxRadius && !wallCollision) {
			explosionCollider.radius = initialRadius;
			Debug.Log ("Expanding Radius");
			initialRadius += radiusExpansion;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Equals("Wall")){
			Debug.Log ("Collision With Wall");
			wallCollision = true;
		}
	}
}
