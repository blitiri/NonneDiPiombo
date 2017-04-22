using UnityEngine;
using System.Collections;

/// <summary>
/// Menu user interface manager.
/// </summary>
public class MenuUIManager : AbstractUIManager
{
	/// <summary>
	/// The players labels.
	/// </summary>
	public UILabel[] playersLabels;

	void Start ()
	{
		InitUI ();
	}

	/// <summary>
	/// Inits the UI.
	/// </summary>
	public void InitUI ()
	{
		int playerIndex;

		for (playerIndex = 0; playerIndex < playersLabels.Length; playerIndex++) {
			playersLabels [playerIndex].color = playersColors [playerIndex];
		}
	}
}
