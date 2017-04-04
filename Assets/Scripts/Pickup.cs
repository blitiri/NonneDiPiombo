using UnityEngine;
using System.Collections;

/// <summary>
/// Pickup control.
/// </summary>
public class Pickup : MonoBehaviour
{


    /// <summary>
    /// The ammo bonus.
    /// </summary>
    public int ammoBonus = 10;
    /// <summary>
    /// The life bonus.
    /// </summary>
    public int lifeBonus = 10;
    /// <summary>
	/// The stress bonus.
	/// </summary>
    public int stressBonus = -10;

    /// <summary>
    /// The pickup transform.
    /// </summary>
    public Transform transformPickUp;

    
	/// <summary>
	/// Updates the pickup instance.
	/// </summary>
	void Update ()
	{
		if (gameObject.tag == "BulletPickUp") {
			transform.RotateAround (transform.position, Vector3.up, 5.0f);
		}
	}

	/// <summary>
	/// Detects a trigger with a player and increases statitics with the related bonus
	/// </summary>
	/// <param name="other">Other.</param>
	public void OnTriggerEnter (Collider other)
	{
		PlayerControl playerControl;

		if (other.gameObject.tag.StartsWith ("Player")  ) {
			playerControl = other.gameObject.GetComponent<PlayerControl> ();
			if (gameObject.tag.Equals ("BulletPickUp")) {
				Destroy (this.gameObject);
				playerControl.AddAmmo (ammoBonus);
			} else if (gameObject.tag.Equals ("Medikit") && playerControl.GetLife() < playerControl.maxLifeValue ) {
				Destroy (this.gameObject);
				playerControl.AddLife (lifeBonus);
			}else if(gameObject.tag.Equals("Stress") && playerControl.GetStress() > 0 ) {
                Destroy(this.gameObject);
                playerControl.AddStress(stressBonus);
            }
		}

	}
}
