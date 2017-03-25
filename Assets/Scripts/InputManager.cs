using UnityEngine;
using System.Collections;
using Rewired;

/// <summary>
/// Input manager for a player.
/// </summary>
public class InputManager
{
	/// <summary>
	/// The Rewired Player.
	/// </summary>
	private Player player;
	/// <summary>
	/// The angle correction respect to the camera.
	/// </summary>
	private float angleCorrection;
	/// <summary>
	/// The player transform.
	/// </summary>
	private Transform playerTransform;

	/// <summary>
	/// Initializes a new instance of the <see cref="InputManager"/> class.
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="playerTransform">Player transform.</param>
	/// <param name="angleCorrection">Angle correction respect to the camera.</param>
	public InputManager (int playerId, Transform playerTransform, float angleCorrection)
	{
		player = ReInput.players.GetPlayer (playerId);
		this.playerTransform = playerTransform;
		this.angleCorrection = angleCorrection;
	}

	/// <summary>
	/// Determines if mouse is assigned to player.
	/// </summary>
	/// <returns><c>true</c> if mouse is assigned to player; otherwise, <c>false</c>.</returns>
	public bool HasMouse ()
	{
		return player.controllers.hasMouse;
	}

	/// <summary>
	/// Gets the player's move vector.
	/// </summary>
	/// <returns>The move vector.</returns>
	public Vector3 GetMoveVector ()
	{
		Vector3 moveVector;

		moveVector = new Vector3 (player.GetAxis ("Move vertical"), 0, -player.GetAxis ("Move horizontal"));
		return CorrectAngle (moveVector);
	}

	/// <summary>
	/// Gets the player's aim vector.
	/// </summary>
	/// <returns>The aim vector.</returns>
	public Vector3 GetAimVector ()
	{
		Vector3 aimVector;

		aimVector = Vector3.zero;
		if (!HasMouse ()) {
			aimVector.z = -player.GetAxis ("Aim horizontal");
			aimVector.x = player.GetAxis ("Aim vertical");
		}
		return CorrectAngle (aimVector);
	}

	/// <summary>
	/// Gets the player's aim angle.
	/// </summary>
	/// <returns>The aim angle.</returns>
	public float GetAimAngle ()
	{
		Vector3 playerScreenPosition;
		Vector3 forwardDirection;
		float aimAngle;

		aimAngle = 0;
		if (HasMouse ()) {
			playerScreenPosition = Camera.main.WorldToScreenPoint (playerTransform.position);
			forwardDirection = Input.mousePosition - playerScreenPosition;
			aimAngle = -Mathf.Atan2 (forwardDirection.y, forwardDirection.x) * Mathf.Rad2Deg + angleCorrection + 110 + Camera.main.transform.rotation.eulerAngles.y;
		}
		return aimAngle;
	}

	/// <summary>
	/// Determines if player mast dash.
	/// </summary>
	public bool Dash ()
	{
		return player.GetButtonDown ("Dash");
	}

	/// <summary>
	/// Determines if player mast drop owned weapon.
	/// </summary>
	public bool Drop ()
	{
		return player.GetButtonDown ("Drop");
	}

	/// <summary>
	/// Determines if player mast pick a weapon.
	/// </summary>
	public bool Pick ()
	{
		return player.GetButtonDown ("Pick");
	}

	/// <summary>
	/// Determines if player mast shoot.
	/// </summary>
	public bool Shoot ()
	{
		return player.GetButtonDown ("Shoot");
	}

	/// <summary>
	/// Determines if player mast melee.
	/// </summary>
	public bool Melee ()
	{
		return player.GetButtonDown ("Melee");
	}

	/// <summary>
	/// Corrects the direction angle respect to the camera.
	/// </summary>
	/// <returns>Corrected direction.</returns>
	/// <param name="direction">Direction to correct.</param>
	private Vector3 CorrectAngle (Vector3 direction)
	{
		return Quaternion.AngleAxis (angleCorrection, Vector3.up) * direction;
	}
}
