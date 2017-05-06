using System;

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
		return int.Parse (playerId.Substring (playerIdPrefix.Length));
	}
}

