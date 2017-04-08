using UnityEngine;
using System.Collections;
using Rewired;

/// <summary>
/// Input manager for a player.
/// </summary>
public class InputManager
{
	/// <summary>
	/// The angle correction respect to the camera.
	/// </summary>
	private float angleCorrection;
	/// <summary>
	/// The player transform.
	/// </summary>
	private Transform playerTransform;
	private Plane aimPlane;
	/// <summary>
	/// The Rewired Player.
	/// </summary>
	public Player player;

	/// <summary>
	/// Initializes a new instance of the <see cref="InputManager"/> class.
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="playerTransform">Player transform.</param>
	/// <param name="angleCorrection">Angle correction respect to the camera.</param>
	public InputManager (int playerId, Transform playerTransform, float angleCorrection)
	{
		aimPlane = new Plane (Vector3.up, Vector3.zero);
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

		moveVector = new Vector3 (player.GetAxis ("Move Vertical"), 0, -player.GetAxis ("Move horizontal"));
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
		if (HasMouse ()) {
			Ray aimRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			float targetDistance;
			if (aimPlane.Raycast (aimRay, out targetDistance)) {
				aimVector = aimRay.GetPoint (targetDistance);
			}
		} else {
			aimVector.z = -player.GetAxis ("Aim horizontal");
			aimVector.x = player.GetAxis ("Aim vertical");
			aimVector = CorrectAngle (aimVector);
		}
		return aimVector;
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
		return player.GetButton ("Shoot");
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
	public Vector3 CorrectAngle (Vector3 direction)
	{
		return CorrectAngle (direction, angleCorrection);
	}

	public Vector3 CorrectAngle (Vector3 direction, float angleCorrection)
	{
		return Quaternion.AngleAxis (angleCorrection , Vector3.up) * direction;
	}
}
