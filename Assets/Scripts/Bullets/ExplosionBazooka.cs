using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBazooka : MonoBehaviour {
	private BazookaBulletManager bazooka;
	private SphereCollider explosionCollider;

	public float minRadius;
	public float maxRadius;
	private float timer;
	public float explosionTime;
	[HideInInspector]
	public bool wallCollision;

	void Start(){
		explosionCollider = GetComponent<SphereCollider> ();
	}

	void Update(){
		if (timer < explosionTime) {
			timer += Time.deltaTime;
		} else {
			explosionCollider.enabled = false;
			timer = 0;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Equals("Wall")){
			Debug.Log ("Collision With Wall");
			explosionCollider.radius = minRadius;
			wallCollision = true;
		} else if (other.gameObject.tag.Equals("LevelWall") || other.gameObject.tag.StartsWith("Player")){
			explosionCollider.radius = maxRadius;
			wallCollision = false;
		}
	}
}
