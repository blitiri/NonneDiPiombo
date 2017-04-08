using UnityEngine;
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
	/// The summary players infos.
	/// </summary>
	public UIPanel[] summaryPlayersInfos;
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
	/// The placeholders.
	/// </summary>
	public UISprite[] placeholders;
	/// <summary>
	/// The placeholders.
	/// </summary>
	public UISprite[] placeholdersBackgrounds;
	/// <summary>
	/// Players scores.
	/// </summary>
	public UILabel[] scores;
	/// <summary>
	/// The players identifiers.
	/// </summary>
	public UILabel[] playersIds;
	/// <summary>
	/// The summary placeholders.
	/// </summary>
	public UISprite[] summaryPlaceholdersBackgrounds;
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
	/// <param name="numberOfPlayers">Number of players.</param>
	/// <param name="maxNumberOfPlayers">Max number of players.</param>
	public void InitUI (int numberOfPlayers, int maxNumberOfPlayers)
	{
		UILabel[] activePlayersIds;
		UILabel[] activeScores;
		UISprite[] activeSummaryPlaceholdersBackgrounds;
		UISprite[] activeBestPlayers;
		int playerIndex;
		int firstActivePlayerSummaryInfoIndex;

		firstActivePlayerSummaryInfoIndex = (maxNumberOfPlayers - numberOfPlayers) / 2;
		// Select summary info components to use
		activePlayersIds = new UILabel[numberOfPlayers];
		activeScores = new UILabel[numberOfPlayers];
		activeSummaryPlaceholdersBackgrounds = new UISprite[numberOfPlayers];
		activeBestPlayers = new UISprite[numberOfPlayers];
		for (playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++) {
			activePlayersIds [playerIndex] = playersIds [firstActivePlayerSummaryInfoIndex + playerIndex];
			activeScores [playerIndex] = scores [firstActivePlayerSummaryInfoIndex + playerIndex];
			activeSummaryPlaceholdersBackgrounds [playerIndex] = summaryPlaceholdersBackgrounds [firstActivePlayerSummaryInfoIndex + playerIndex];
			activeBestPlayers [playerIndex] = bestPlayers [firstActivePlayerSummaryInfoIndex + playerIndex];
		}
		foreach (UIPanel summaryPlayersInfo in summaryPlayersInfos) {
			summaryPlayersInfo.enabled = false;
		}
		for (playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++) {
			activePlayersIds [playerIndex].text = "P" + (playerIndex + 1);
			activeBestPlayers [playerIndex].enabled = false;
			activeScores [playerIndex].text = "0";
			activeSummaryPlaceholdersBackgrounds [playerIndex].color = playersColors [playerIndex];
			activeSummaryPlaceholdersBackgrounds [playerIndex].parent.GetComponent <UIPanel> ().enabled = true;
			placeholdersBackgrounds [playerIndex].color = playersColors [playerIndex];
			playersInfos [playerIndex].enabled = true;
		}
		for (; playerIndex < playersInfos.Length; playerIndex++) {
			playersInfos [playerIndex].enabled = false;
		}
	}
}
