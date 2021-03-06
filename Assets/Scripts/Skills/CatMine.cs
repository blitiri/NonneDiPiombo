﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMine : MonoBehaviour 
{
	private TweenColor tweenColor;
	private SphereCollider colliderCat;
	private MeshRenderer mesh;
	public GameObject explosionPrefab;
	public float timerToExplode=5.0f;
	public float radiusOfCollider;
	public float explosionDuration;

	void Awake () 
	{
		tweenColor = GetComponent<TweenColor> ();
		colliderCat = GetComponent<SphereCollider> ();
		mesh = GetComponent<MeshRenderer> ();
	}

	void Start()
	{
		StartCoroutine ("CatExplosion");
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject.tag.StartsWith ("Player") && Utility.GetPlayerIndexFromBullet (this.gameObject.tag) != Utility.GetPlayerIndex (other.gameObject.tag)) {
			StartCoroutine ("CatExplosionOnContact");
		}
	}

	IEnumerator CatExplosion()
	{
		tweenColor.enabled=true;
		yield return new WaitForSeconds (timerToExplode);
		StartCoroutine("CatExplosionOnContact");
	}

	IEnumerator CatExplosionOnContact(){
		colliderCat.radius = radiusOfCollider;
		mesh.enabled = false;

		GameObject explosion = Instantiate (explosionPrefab) as GameObject;
		explosion.transform.position = this.gameObject.transform.position + new Vector3 (0, 1, 0);
		explosion.transform.rotation = Quaternion.identity;
		explosion.tag = this.gameObject.tag;

		GameManager.instance.catCounter--;
		Destroy (explosion, explosionDuration);
		Destroy (this.gameObject,2.0f);
		yield return null;
	}
}