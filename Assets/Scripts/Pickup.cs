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
	/// The stress bonus.
	/// </summary>
	public int stressBonus = -10;
	/// <summary>
	/// The pickup transform.
	/// </summary>
	public Transform transformPickUp;

	/// <summary>
	/// Detects a trigger with a player and increases statitics with the related bonus
	/// </summary>
	/// <param name="other">Other.</param>
	public void OnTriggerEnter (Collider other)
	{
		PlayerControl playerControl;

		if (other.gameObject.tag.StartsWith ("Player")) {
			playerControl = other.gameObject.GetComponent<PlayerControl> ();
		/* if (gameObject.tag.Equals ("Stress") && playerControl.IsStressed ()) {
				Destroy (this.gameObject);
				playerControl.AddStress (stressBonus);
			}*/
		}

	}
}
