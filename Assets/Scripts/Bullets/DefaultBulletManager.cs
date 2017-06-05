using UnityEngine;
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

	void FixedUpdate(){
		DefaultMovement ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag.Equals ("Wall") || other.gameObject.tag.StartsWith("Player") || other.gameObject.tag.Equals("LevelWall")) {
			Trigger ();
		}
        else if(other.gameObject.tag=="Pan")
        {
            Debug.Log("kappa");

            transform.up = -transform.up;
            bulletRb.velocity = -bulletRb.velocity*(bulletSpeed/2);
        }
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
