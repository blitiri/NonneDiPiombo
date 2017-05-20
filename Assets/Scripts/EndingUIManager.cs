using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Ending scene user interface manager.
/// </summary>
public class EndingUIManager : AbstractUIManager
{
	/// <summary>
	/// The player ranking position prefab.
	/// </summary>
	public GameObject[] playersRankingPositions;
	/// <summary>
	/// Scene to load on restart click.
	/// </summary>
	public string restartSceneName = "CharacterSelectionMenu";

	/// <summary>
	/// Start the script.
	/// </summary>
	void Start ()
	{
		InitUI ();
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
		string s;

		ranking = Statistics.instance.GetRanking ();
		for (playerIndex = 0, position = 0; playerIndex < ranking.Length; playerIndex++) {
			if ((playerIndex == 0) || (ranking [playerIndex].GetScore () != ranking [playerIndex - 1].GetScore ())) {
				position = (playerIndex + 1);
			}
			playerRankingPosition = playersRankingPositions [playerIndex];
			playerRankingPosition.SetActive (true);
			backgroundPlayerIcon = playerRankingPosition.GetComponent<UISprite> ();
			backgroundPlayerIcon.color = Configuration.instance.playersColors [playerIndex];
			foreach (Transform child in playerRankingPosition.transform) {
				if (child.tag.Equals ("Position")) {
					positionLabel = child.gameObject.GetComponent<UILabel> ();
					positionLabel.text = GetPositionString (position);
				} else if (child.tag.Equals ("PlayerIcon")) {
					playerIcon = child.gameObject.GetComponent<UISprite> ();
					playerIcon.spriteName = ranking [playerIndex].GetPlayerIcon ();
				} else if (child.tag.Equals ("Winner")) {
					winnerIcon = child.gameObject.GetComponent<UISprite> ();
					winnerIcon.enabled = (position == 1);
				} else if (child.tag.Equals ("Score")) {
					scoreLabel = child.gameObject.GetComponent<UILabel> ();
					scoreLabel.text = "Score: " + ranking [playerIndex].GetScore ();
				} else if (child.tag.Equals ("PlayerId")) {
					scoreLabel = child.gameObject.GetComponent<UILabel> ();
					scoreLabel.text = "P" + (ranking [playerIndex].GetPlayerId () + 1);
				}
			}
		}
		for (; playerIndex < Configuration.instance.GetNumberOfPlayers (); playerIndex++) {
			playersRankingPositions [playerIndex].SetActive (false);
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

	/// <summary>
	/// Restarts game loading characters selection scene.
	/// </summary>
	public void OnRestart ()
	{
		SceneController.instance.LoadSceneByName (restartSceneName);
	}

	/// <summary>
	/// Quits the game.
	/// </summary>
	public void OnQuit ()
	{
		Application.Quit ();
	}
}
