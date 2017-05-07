using UnityEngine;
using System.Collections;

/// <summary>
/// Menu user interface manager.
/// </summary>
public class MenuUIManager : AbstractUIManager
{
	/// <summary>
	/// The name of the next scene to load with start button.
	/// </summary>
	[Tooltip ("The name of the next scene to load with start button.")]
	public string nextSceneName = "CharacterSelection";

	/// <summary>
	/// Starts the game.
	/// </summary>
	public void OnStart ()
	{
		SceneController.instance.LoadSceneByName (nextSceneName);
	}

	public void OnOptions ()
	{
	}

	public void OnCredits ()
	{
	}

	/// <summary>
	/// Quits the game.
	/// </summary>
	public void OnQuit ()
	{
		Application.Quit ();
	}
}
