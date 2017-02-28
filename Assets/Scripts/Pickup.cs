using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
	public Transform transformPickUp;
	public int ammoBonus = 10;
	public int lifeBonus = 10;

	// Update is called once per frame
	void Update ()
	{
		if (gameObject.tag == "BulletPickUp") {
			transform.RotateAround (transform.position, Vector3.up, 5.0f);
		}
	}

	//effetti dei PickUp
	public void OnTriggerEnter (Collider other)
	{
		PlayerControl playerControl;

		if (other.gameObject.tag.StartsWith ("Player")) {
			playerControl = other.gameObject.GetComponent<PlayerControl> ();
			if (gameObject.tag.Equals ("BulletPickUp")) {
				playerControl.addAmmo (ammoBonus);
			} else if (gameObject.tag.Equals ("Medikit")) {
				playerControl.addLife (lifeBonus);
			}
		}
	}
}
