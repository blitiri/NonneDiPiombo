using UnityEngine;
using System.Collections;

public class DefaultBulletManager : MonoBehaviour
{
	protected MeshRenderer bulletMeshRender;
	protected Rigidbody bulletRb;
	protected CapsuleCollider bulletCapsuleCollider;
	protected SphereCollider bulletSphereCollider;
	protected bool isMoving = true;
	protected Transform reflectedPosition;
	public float bulletSpeed;

	public GameObject sparkVFX;

	void Awake ()
	{
		Components ();
	}

	void FixedUpdate ()
	{
		DefaultMovement ();
	}

	void OnCollisionEnter (Collision other)
	{
		GameObject spark = Instantiate (sparkVFX, transform.position, transform.rotation) as GameObject;
		Destroy (spark, 0.5f);
		if (other.gameObject.tag.Equals ("Wall") || other.gameObject.tag.Equals ("LevelWall")) {
			Trigger ();
		} else if (other.collider.GetType () == typeof(SphereCollider) && other.gameObject.tag.StartsWith ("Player")) {
			transform.forward = Vector3.Reflect (transform.forward, other.contacts [0].normal);
		} else if (other.gameObject.tag.StartsWith ("Player") && Utility.GetPlayerIndexFromBullet (this.gameObject.tag) != Utility.GetPlayerIndex (other.gameObject.tag)) {
			Trigger ();
		}       
	}


	protected virtual void DefaultMovement ()
	{
		if (isMoving) {
			bulletRb.velocity = bulletRb.transform.up * bulletSpeed;
		}
	}

	protected virtual void Components ()
	{
		bulletRb = GetComponent<Rigidbody> ();
		bulletMeshRender = GetComponent<MeshRenderer> ();
		bulletCapsuleCollider = GetComponent<CapsuleCollider> ();
	}

	protected virtual void Trigger ()
	{
		isMoving = false;

		bulletMeshRender.enabled = false;
		bulletCapsuleCollider.enabled = false;
		bulletRb.velocity = Vector3.zero;
		Destroy (this.gameObject, 1f);
	}
}
