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
	/// Sets a player stress.
	/// </summary>
	/// <param name="stress">Level of stress (a value between 0 and 1).</param>
	/// <param name="playerId">Player Id (a 0-based index).</param>
	public void SetStress (float stress, int playerId)
	{
		//stressLevels [playerId].value = Mathf.Clamp (stress, 0, 1);
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

	/// <summary>
	/// Clicking on resume button
	/// </summary>
	public void OnClickResumeButton ()
	{
		GameManager.instance.ResumePlay ();
	}

	/// <summary>
	/// Clicking on restart button
	/// </summary>
	public void OnClickRestartButton() {
		SceneController.instance.LoadActiveScene ();
	}

	/// <summary>
	/// Clicking on quit button
	/// </summary>
	public void OnClickQuitButton() {
		SceneController.instance.OnClickQuitButton ();
	}
}
