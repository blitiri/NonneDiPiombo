using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Level user interface manager.
/// </summary>
public class LevelUIManager : AbstractUIManager
{
	/// <summary>
	/// LevelUIManager instance.
	/// </summary>
	public static LevelUIManager instance;
	/// <summary>
	/// The timer for round countdown.
	/// </summary>
	public UILabel timer;
	/// <summary>
	/// The players infos.
	/// </summary>
	public UIPanel[] playersInfos;
	/// <summary>
	/// Players ammo counters. Important: respect players order (first player1 ammoCounter, then player2 ammoCounter and so on)
	/// </summary>
	public UILabel[] ammoCounters;
	/// <summary>
	/// Players life level. Important: respect players order (first player1 lifeLevel, then player2 lifeLevel and so on)
	/// </summary>
	public UISlider[] lifeLevels;
	/// <summary>
	/// Players stress level. Important: respect players order (first player1 stressLevel, then player2 stressLevel and so on)
	/// </summary>
	public UISlider[] stressLevels;
	/// <summary>
	/// The players icons.
	/// </summary>
	public UISprite[] playersIcons;
	/// <summary>
	/// The players icons backgrounds.
	/// </summary>
	public UISprite[] playersIconsBackgrounds;
	/// <summary>
	/// Players scores.
	/// </summary>
	public UILabel[] scores;
	/// <summary>
	/// The best player symbols.
	/// </summary>
	public UISprite[] bestPlayers;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		instance = this;
	}

	/// <summary>
	/// Sets the timer.
	/// </summary>
	/// <param name="timer">Timer.</param>
	public void SetTimer (float timer)
	{
		int minutes;
		int seconds;

		minutes = Mathf.FloorToInt (timer / 60f);
		seconds = Mathf.FloorToInt (timer - minutes * 60);
		this.timer.text = string.Format ("{0:0}:{1:00}", minutes, seconds);
	}

	/// <summary>
	/// Sets a player life.
	/// </summary>
	/// <param name="stress">Level of life (a value between 0 and 1).</param>
	/// <param name="playerId">Player Id (a 0-based index).</param>
	public void SetLife (float life, int playerId)
	{
		lifeLevels [playerId].value = Mathf.Clamp (life, 0, 1);
	}

	/// <summary>
	/// Sets a player stress.
	/// </summary>
	/// <param name="stress">Level of stress (a value between 0 and 1).</param>
	/// <param name="playerId">Player Id (a 0-based index).</param>
	public void SetStress (float stress, int playerId)
	{
		stressLevels [playerId].value = Mathf.Clamp (stress, 0, 1);
	}

	/// <summary>
	/// Sets infinite ammo to a player.
	/// </summary>
	/// <param name="playerId">Player Id (a 0-based index).</param>
	public void SetInfiniteAmmo (int playerId)
	{
		ammoCounters [playerId].text = "--";
	}

	/// <summary>
	/// Sets a player ammo.
	/// </summary>
	/// <param name="ammo">Ammo owned by player (a value &gt;= 0).</param>
	/// <param name="playerId">Player Id (a 0-based index).</param>
	public void SetAmmo (int ammo, int playerId)
	{
		ammoCounters [playerId].text = "x" + (ammo < 0 ? 0 : ammo);
	}

	/// <summary>
	/// Sets a player score.
	/// </summary>
	/// <param name="score">Player's score.</param>
	/// <param name="playerId">Player Id (a 0-based index).</param>
	public void SetScore (int score, int playerId)
	{
		scores [playerId].text = score.ToString ();
	}

	/// <summary>
	/// Sets the best player.
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	private void SetBestPlayer (int playerId)
	{
		int playerIndex;

		for (playerIndex = 0; playerIndex < bestPlayers.Length; playerIndex++) {
			bestPlayers [playerIndex].enabled = (playerIndex == playerId);
		}
	}

	/// <summary>
	/// Inits the UI.
	/// </summary>
	public void InitUI ()
	{
		GameObject[] players;
		SpriteRenderer playerIdentifierRenderer;
		string playerIcon;
		int playerIndex;

		players = GameManager.instance.GetPlayers ();
		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			scores [playerIndex].text = "0";
			playerIcon = Utility.GetPlayerIcon (players [playerIndex]);
			Statistics.instance.SetPlayerIcon (playerIndex, playerIcon);
			playersIcons [playerIndex].spriteName = playerIcon;
			playersIconsBackgrounds [playerIndex].color = Configuration.instance.playersColors [playerIndex];
			playerIdentifierRenderer = players [playerIndex].GetComponentInChildren<SpriteRenderer> ();
			if (playerIdentifierRenderer != null) {
				playerIdentifierRenderer.color = Configuration.instance.playersColors [playerIndex];
			}
			bestPlayers [playerIndex].enabled = false;
			playersInfos [playerIndex].enabled = true;
		}
		for (; playerIndex < playersInfos.Length; playerIndex++) {
			playersInfos [playerIndex].enabled = false;
		}
	}
}
