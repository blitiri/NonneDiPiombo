using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Statistics.
/// </summary>
public class Statistics : MonoBehaviour
{
	/// <summary>
	/// Statistics instance.
	/// </summary>
	public static Statistics instance;
	/// <summary>
	/// The killed malus.
	/// </summary>
	public int killedMalus = 1;
	/// <summary>
	/// The killer bonus.
	/// </summary>
	public int killerBonus = 2;
	/// <summary>
	/// The debug test case to execute.
	/// </summary>
	[Range (1, 11)]
	public int debugTestCase = 11;
	/// <summary>
	/// The players kills.
	/// </summary>
	private int[] playersKills;
	/// <summary>
	/// The players icons.
	/// </summary>
	private string[] playersIcons;

	/// <summary>
	/// Awake the script.
	/// </summary>
	void Awake ()
	{
		if (instance == null) {
			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	/// <summary>
	/// Update statistics after a player is killed.
	/// </summary>
	/// <param name="killedId">Killed identifier.</param>
	/// <param name="killerId">Killer identifier.</param>
	public void PlayerKilled (int killedId, int killerId)
	{
		playersKills [killedId] -= killedMalus;
		playersKills [killerId] += killerBonus;

	}

	public void PlayerSuicide(int killedId){
		playersKills [killedId] -= killedMalus * 2;
	}

	/// <summary>
	/// Gets the players kills.
	/// </summary>
	/// <returns>The players kills.</returns>
	public int[] getPlayersKills ()
	{
		return playersKills;
	}

	/// <summary>
	/// Reset statistics.
	/// </summary>
	public void Reset ()
	{
		playersKills = new int[Configuration.instance.GetNumberOfPlayers ()];
		playersIcons = new string[Configuration.instance.GetNumberOfPlayers ()];
	}

	/// <summary>
	/// Sets the player icon.
	/// </summary>
	/// <param name="playerIndex">Player index.</param>
	/// <param name="playerIcon">Player icon.</param>
	public void SetPlayerIcon (int playerIndex, string playerIcon)
	{
		playersIcons [playerIndex] = playerIcon;
	}

	/// <summary>
	/// Gets the players' ranking.
	/// </summary>
	/// <returns>The ranking.</returns>
	public RankingPosition[] GetRanking ()
	{
		RankingPosition[] ranking;
		int playerIndex;

		if (playersKills != null) {
			ranking = new RankingPosition[Configuration.instance.GetNumberOfPlayers ()];
			for (playerIndex = 0; playerIndex < Configuration.instance.GetNumberOfPlayers (); playerIndex++) {
				ranking [playerIndex] = new RankingPosition (playerIndex, playersKills [playerIndex], playersIcons [playerIndex]);
			}
		} else {
			// Execution is out of normal game flow, we are in a testing session
			Debug.LogWarning ("Statistics: Execution is out of normal game flow, testing session is assumed! Test case " + debugTestCase + " executed.");
			ranking = TestUtilities.GetDummyRanking (debugTestCase);
		}
		Array.Sort (ranking);
		return ranking;
	}
}
