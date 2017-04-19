using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Ending scene user interface manager.
/// </summary>
public class EndingUIManager : AbstractUIManager
{
	/// <summary>
	/// EndingUIManager instance.
	/// </summary>
	public static EndingUIManager instance;
	/// <summary>
	/// The player ranking position prefab.
	/// </summary>
	public GameObject[] playersRankingPositions;
	/// <summary>
	/// Enables the debug mode.
	/// </summary>
	public bool debugMode = true;
	/// <summary>
	/// The debug test case to execute.
	/// </summary>
	[Range (1, 11)]
	public int debugTestCase = 1;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		instance = this;
	}

	/// <summary>
	/// Inits the UI.
	/// </summary>
	public void InitUI ()
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

	public void Restart ()
	{
		//Debug.Log ("Restart pressed: loadin scene " + SceneController.instance.GetLastLevelSceneLoaded ());
		//SceneController.instance.LoadSceneByName (SceneController.instance.GetLastLevelSceneLoaded ());
		SceneController.instance.LoadSceneByName ("CharacterSelection");
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
}
