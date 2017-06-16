﻿using System;
using UnityEngine;

/// <summary>
/// Generic utilites.
/// </summary>
public class Utility
{
	/// <summary>
	/// The player identifier prefix.
	/// </summary>
	private const string playerIdPrefix = "Player";
	/// <summary>
	/// The player icon suffix.
	/// </summary>
	private const string playerIconSuffix = "PlayerIcon";
	/// <summary>
	/// Game object clone suffix.
	/// </summary>
	private const string cloneSuffix = "(Clone)";

	private const string bulletPrefix = "Bullet";

	private const string bulletTag = "BulletPlayer";

	private const string catTag = "Cat";
	/// <summary>
	/// Gets the player identifier from player index.
	/// </summary>
	/// <returns>The player identifier.</returns>
	/// <param name="playerIndex">Player index.</param>
	public static string GetPlayerId (int playerIndex)
	{
		return playerIdPrefix + playerIndex;
	}

	/// <summary>
	/// Gets the index of the player from player identifier.
	/// </summary>
	/// <returns>The player index.</returns>
	/// <param name="playerId">Player identifier.</param>
	public static int GetPlayerIndex (string playerId)
	{
		//Debug.Log ("PlayerID: " + playerId);
		return int.Parse (playerId.Substring (playerIdPrefix.Length));
	}

	public static int GetPlayerIndexFromBullet (string bulletId)
	{
		return int.Parse (bulletId.Substring (bulletTag.Length));
	}

	public static int GetPlayerIndexFromMine (string mineId)
	{
		return int.Parse (mineId.Substring (catTag.Length));
	}

	/// <summary>
	/// Gets the player icon.
	/// </summary>
	/// <returns>The player icon.</returns>
	/// <param name="player">Player.</param>
	public static string GetPlayerIcon (GameObject player)
	{
		return player.name.Replace (cloneSuffix, "") + playerIconSuffix;
	}
}

