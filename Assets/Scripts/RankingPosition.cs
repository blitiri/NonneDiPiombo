﻿using System;
using UnityEngine;

/// <summary>
/// A player ranking position.
/// </summary>
public class RankingPosition : IComparable
{
	/// <summary>
	/// The player score.
	/// </summary>
	private int score;
	/// <summary>
	/// The player identifier.
	/// </summary>
	private int playerId;
	/// <summary>
	/// The player game object.
	/// </summary>
	private GameObject player;

	/// <summary>
	/// Initializes a new instance of the <see cref="RankingPosition"/> class.
	/// </summary>
	public RankingPosition ()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RankingPosition"/> class.
	/// </summary>
	/// <param name="player">Player game object.</param>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="score">Player score.</param>
	public RankingPosition (GameObject player, int playerId, int score)
	{
		this.player = player;
		this.playerId = playerId;
		this.score = score;
	}

	/// <summary>
	/// Compares two <see cref="RankingPosition"/> instances.
	/// </summary>
	/// <returns>< 0 if current instance is before other instance, 0 if equals, > 1 if after.</returns>
	/// <param name="obj">Other instance.</param>
	public int CompareTo (object obj)
	{
		RankingPosition other;

		other = (RankingPosition)obj;
		// Ordering from max to min
		return other.score - score;
	}

	/// <summary>
	/// Sets the player game object.
	/// </summary>
	/// <param name="player">Player game object.</param>
	public void SetPlayer (GameObject player)
	{
		this.player = player;
	}

	/// <summary>
	/// Gets the player game object.
	/// </summary>
	/// <returns>The player game object.</returns>
	public GameObject GetPlayer ()
	{
		return player;
	}

	/// <summary>
	/// Sets the player score.
	/// </summary>
	/// <param name="score">Player score.</param>
	public void SetScore (int score)
	{
		this.score = score;
	}

	/// <summary>
	/// Gets the player score.
	/// </summary>
	/// <returns>The player score.</returns>
	public int GetScore ()
	{
		return score;
	}

	/// <summary>
	/// Sets the player identifier.
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void SetPlayerId (int playerId)
	{
		this.playerId = playerId;
	}

	/// <summary>
	/// Gets the player identifier.
	/// </summary>
	/// <returns>The player identifier.</returns>
	public int GetPlayerId ()
	{
		return playerId;
	}
}
