using UnityEngine;
using System.Collections;

public class DefaultBulletManager : MonoBehaviour {
	
	private MeshRenderer bulletMeshRender;
	private Rigidbody bulletRb;
	private CapsuleCollider bulletCollider;
	private bool isMoving = true;
	public float bulletSpeed;

	void Awake(){
		Components ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag.StartsWith ("Player") || other.gameObject.tag.Equals ("Wall")) {
			Trigger ();
		}
	}

	void FixedUpdate(){
		DefaultMovement (bulletSpeed);
	}


	protected virtual void DefaultMovement(float bulletSpeed){
		if (isMoving) {
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
		}
	}

	protected virtual void Components(){
		bulletRb = GetComponent<Rigidbody> ();
		bulletMeshRender = GetComponent<MeshRenderer> ();
		bulletCollider = GetComponent<CapsuleCollider> ();
	}

	protected virtual void Trigger(){
		isMoving = false;
		bulletMeshRender.enabled = false;
		bulletCollider.enabled = false;
		bulletRb.velocity = bulletRb.transform.up * 0;
		Destroy (this.gameObject, 1f);
	}
}
