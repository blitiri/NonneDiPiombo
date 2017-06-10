using UnityEngine;
using System.Collections;

public class DefaultBulletManager : MonoBehaviour {
	protected MeshRenderer bulletMeshRender;
	protected Rigidbody bulletRb;
	protected CapsuleCollider bulletCapsuleCollider;
	protected SphereCollider bulletSphereCollider;
	protected bool isMoving = true;
	protected Vector3 oldVelocity;
	public float bulletSpeed;

	void Awake(){
		Components ();
	}

	void FixedUpdate(){
		DefaultMovement ();
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag.Equals ("Wall") || other.gameObject.tag.StartsWith("Player") || other.gameObject.tag.Equals("LevelWall")) {
			Trigger ();
		}
        else if(other.gameObject.tag == "Pan")
        {
			/*ContactPoint contact = other.contacts [0];

			Vector3 reflectedVelocity = Vector3.Reflect (bulletRb.velocity, contact.normal);
			bulletRb.velocity = reflectedVelocity;

			Quaternion rotation = Quaternion.FromToRotation (oldVelocity,reflectedVelocity);
			transform.rotation = rotation * transform.rotation;*/
        }
	}


	protected virtual void DefaultMovement(){
		if (isMoving) {
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
			oldVelocity = bulletRb.velocity;
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
