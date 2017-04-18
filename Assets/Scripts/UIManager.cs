using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// User interface manager.
/// </summary>
public class UIManager : MonoBehaviour
{
	/// <summary>
	/// UIManager instance.
	/// </summary>
	public static UIManager instance;
	/// <summary>
	/// The players colors.
	/// </summary>
	public Color[] playersColors = { Color.red, Color.green, Color.blue, Color.yellow };
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
	/// The player ranking position prefab.
	/// </summary>
	public GameObject[] playersRankingPositions;
	public bool debugMode = true;
	public int debugTestCase = 1;

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
	/// Inits the Level UI.
	/// </summary>
	public void InitLevelUI ()
	{
		GameObject[] players;
		SpriteRenderer playerIdentifierRenderer;
		int playerIndex;

		players = GameManager.instance.GetPlayers ();
		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			scores [playerIndex].text = "0";
			//Debug.Log ("spriteName = " + players [playerIndex].name.Replace("(Clone)", "") + "PlayerIcon");
			playersIcons [playerIndex].spriteName = GetPlayerIcon (players [playerIndex]);
			playersIconsBackgrounds [playerIndex].color = playersColors [playerIndex];
			playerIdentifierRenderer = players [playerIndex].GetComponentInChildren<SpriteRenderer> ();
			if (playerIdentifierRenderer != null) {
				playerIdentifierRenderer.color = playersColors [playerIndex];
			}
			bestPlayers [playerIndex].enabled = false;
			playersInfos [playerIndex].enabled = true;
		}
		for (; playerIndex < playersInfos.Length; playerIndex++) {
			playersInfos [playerIndex].enabled = false;
		}
	}

	/// <summary>
	/// Inits the ending UI.
	/// </summary>
	public void InitEndingUI ()
	{
		GameObject playerRankingPosition;
		RankingPosition[] ranking;
		UILabel positionLabel;
		UILabel scoreLabel;
		UISprite playerIcon;
		UISprite backgroundPlayerIcon;
		UISprite winnerIcon;
		int playerIndex;
		int position;

		if (debugMode) {
			ranking = GetDummyRanking ();
		} else {
			ranking = GameManager.instance.GetRanking ();
		}
		for (playerIndex = 0, position = 0; playerIndex < ranking.Length; playerIndex++) {
			if ((playerIndex == 0) || (ranking [playerIndex].GetScore () != ranking [playerIndex - 1].GetScore ())) {
				position = (playerIndex + 1);
			}
			playerRankingPosition = playersRankingPositions [playerIndex];
			playerRankingPosition.SetActive (true);
			backgroundPlayerIcon = playerRankingPosition.GetComponent<UISprite> ();
			backgroundPlayerIcon.color = playersColors [playerIndex];
			foreach (Transform child in playerRankingPosition.transform) {
				if (child.tag.Equals ("Position")) {
					positionLabel = child.gameObject.GetComponent<UILabel> ();
					positionLabel.text = GetPositionString (position);
				} else if (child.tag.Equals ("PlayerIcon")) {
					playerIcon = child.gameObject.GetComponent<UISprite> ();
					if (debugMode) {
						playerIcon.spriteName = "ChefAgataPlayerIcon";
					} else {
						playerIcon.spriteName = GetPlayerIcon (ranking [playerIndex].GetPlayer ());
					}
				} else if (child.tag.Equals ("Winner")) {
					winnerIcon = child.gameObject.GetComponent<UISprite> ();
					winnerIcon.enabled = (position == 1);
				} else if (child.tag.Equals ("Score")) {
					scoreLabel = child.gameObject.GetComponent<UILabel> ();
					scoreLabel.text = "Score: " + ranking [playerIndex].GetScore ();
				}
			}
		}
		if (debugMode) {
			for (; playerIndex < 4; playerIndex++) {
				playersRankingPositions [playerIndex].SetActive (false);
			}
		} else {
			for (; playerIndex < GameManager.instance.numberOfPlayers; playerIndex++) {
				playersRankingPositions [playerIndex].SetActive (false);
			}
		}
	}

	/// <summary>
	/// Produce a dummy ranking.
	/// </summary>
	/// <returns>The dummy ranking.</returns>
	private RankingPosition[] GetDummyRanking ()
	{
		RankingPosition[] ranking;

		switch (debugTestCase) {
		case 1:
			// Two players
			ranking = new RankingPosition[2];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, -5);
			break;
		case 2:
			// Two players with same score
			ranking = new RankingPosition[2];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, 10);
			break;
		case 3:
			// Three players
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, -5);
			ranking [2] = new RankingPosition (null, 2, 7);
			break;
		case 4:
			// Three players with two with same score
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, 7);
			ranking [2] = new RankingPosition (null, 2, 10);
			break;
		case 5:
			// Three players with two with same score
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (null, 0, 7);
			ranking [1] = new RankingPosition (null, 1, 10);
			ranking [2] = new RankingPosition (null, 2, 7);
			break;
		case 6:
			// Three players with same score
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (null, 0, 7);
			ranking [1] = new RankingPosition (null, 1, 7);
			ranking [2] = new RankingPosition (null, 2, 7);
			break;
		case 7:
			// Four players with two with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, 10);
			ranking [2] = new RankingPosition (null, 2, 7);
			ranking [3] = new RankingPosition (null, 3, 0);
			break;
		case 8:
			// Four players with three with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, 10);
			ranking [2] = new RankingPosition (null, 2, 10);
			ranking [3] = new RankingPosition (null, 3, 0);
			break;
		case 9:
			// Four players with three with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, 10);
			ranking [2] = new RankingPosition (null, 2, 10);
			ranking [3] = new RankingPosition (null, 3, 22);
			break;
		case 10:
			// Four players with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, 10);
			ranking [2] = new RankingPosition (null, 2, 10);
			ranking [3] = new RankingPosition (null, 3, 10);
			break;
		case 11:
		default:
			// Four players
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (null, 0, 10);
			ranking [1] = new RankingPosition (null, 1, -5);
			ranking [2] = new RankingPosition (null, 2, 7);
			ranking [3] = new RankingPosition (null, 3, 0);
			break;
		}
		Array.Sort (ranking);
		return ranking;
	}

	/// <summary>
	/// Gets the player icon.
	/// </summary>
	/// <returns>The player icon.</returns>
	/// <param name="player">Player.</param>
	private string GetPlayerIcon (GameObject player)
	{
		return player.name.Replace ("(Clone)", "") + "PlayerIcon";
	}

	/// <summary>
	/// Gets the position in string format.
	/// </summary>
	/// <returns>The position in string format.</returns>
	/// <param name="position">Position.</param>
	private string GetPositionString (int position)
	{
		string positionSuffix;

		switch (position) {
		case 1:
			positionSuffix = "st";
			break;
		case 2:
			positionSuffix = "nd";
			break;
		case 3:
			positionSuffix = "rd";
			break;
		default:
			positionSuffix = "th";
			break;
		}
		return position + positionSuffix;
	}
}
