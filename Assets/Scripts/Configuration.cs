using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configuration : MonoBehaviour
{
	public static Configuration instance;
	private const int maxNumberOfPlayers = 4;
	private const int minNumberOfPlayers = 2;
	public GameObject[] granniesPrefabs;
	[Range (minNumberOfPlayers, maxNumberOfPlayers)]
	public int defaultNumberOfPlayers = maxNumberOfPlayers;
	public bool defaultFullScreen = true;
	public float defaultSoundVolume = 1;
	private int numberOfPlayers;
	private string selectedLevel;
	private bool fullScreen;
	private float soundVolume;
	private IList<GameObject> selectedGrannies;

	void Awake ()
	{
		int grannyIndex;

		if (instance == null) {
			DontDestroyOnLoad (transform.gameObject);
			if (SceneController.IsLevelSelectionScene () || SceneController.IsLevelScene ()) {
				numberOfPlayers = defaultNumberOfPlayers;
				selectedGrannies = new GameObject[numberOfPlayers];
				for (grannyIndex = 0; grannyIndex < numberOfPlayers; grannyIndex++) {
					selectedGrannies [grannyIndex] = granniesPrefabs [grannyIndex % granniesPrefabs.Length];
				}
			} else {
				numberOfPlayers = 0;
				selectedGrannies = new List<GameObject> ();
			}
			soundVolume = defaultSoundVolume;
			fullScreen = defaultFullScreen;
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	/// <summary>
	/// Sets the number of players. The value is clamped between 0 and maximum number of players
	/// </summary>
	/// <param name="numberOfPlayers">Number of players.</param>
	public void SetNumberOfPlayers (int numberOfPlayers)
	{
		this.numberOfPlayers = Mathf.Clamp (numberOfPlayers, 0, maxNumberOfPlayers);
		ResizeSelectedGrannies ();
	}

	/// <summary>
	/// Resizes the selected grannies.
	/// </summary>
	private void ResizeSelectedGrannies ()
	{
		// There are too many grannies
		while (selectedGrannies.Count > numberOfPlayers) {
			selectedGrannies.RemoveAt (selectedGrannies.Count - 1);
		}
		// There are too few grannies
		while (selectedGrannies.Count < numberOfPlayers) {
			selectedGrannies.Add (granniesPrefabs [0]);
		}
	}

	/// <summary>
	/// Gets the number of players.
	/// </summary>
	/// <returns>The number of players.</returns>
	public int GetNumberOfPlayers ()
	{
		return numberOfPlayers;
	}

	/// <summary>
	/// Sets the selected level.
	/// </summary>
	/// <param name="selectedLevel">Selected level.</param>
	public void SetSelectedLevel (string selectedLevel)
	{
		this.selectedLevel = selectedLevel;
	}

	/// <summary>
	/// Gets the selected level.
	/// </summary>
	/// <returns>The selected level.</returns>
	public string GetSelectedLevel ()
	{
		return selectedLevel;
	}

	/// <summary>
	/// Sets the sound volume.
	/// </summary>
	/// <param name="soundVolume">Sound volume.</param>
	public void SetSoundVolume (float soundVolume)
	{
		this.soundVolume = soundVolume;
	}

	/// <summary>
	/// Gets the sound volume.
	/// </summary>
	/// <returns>The sound volume.</returns>
	public float GetSoundVolume ()
	{
		return soundVolume;
	}

	/// <summary>
	/// Sets the full screen.
	/// </summary>
	/// <param name="fullScreen">If set to <c>true</c> full screen.</param>
	public void SetFullScreen (bool fullScreen)
	{
		this.fullScreen = fullScreen;
	}

	/// <summary>
	/// Determines whether this instance is full screen.
	/// </summary>
	/// <returns><c>true</c> if this instance is full screen; otherwise, <c>false</c>.</returns>
	public bool IsFullScreen ()
	{
		return fullScreen;
	}

	/// <summary>
	/// Sets the selected granny.
	/// </summary>
	/// <param name="granny">Granny.</param>
	/// <param name="playerIndex">Player index.</param>
	public void SetSelectedGranny (GameObject granny, int playerIndex)
	{
		selectedGrannies [playerIndex] = granny;
	}

	/// <summary>
	/// Gets the selected grannies.
	/// </summary>
	/// <returns>The selected grannies.</returns>
	public IList<GameObject> GetSelectedGrannies ()
	{
		return selectedGrannies;
	}

	/// <summary>
	/// Validate the number of player.
	/// </summary>
	public void ValidateNumberOfPlayer ()
	{
		if ((numberOfPlayers < minNumberOfPlayers) || (numberOfPlayers > maxNumberOfPlayers)) {
			throw new Exception ("Bad players number");
		}
	}

	/// <summary>
	/// Validates the selected level.
	/// </summary>
	public void ValidateSelectedLevel ()
	{
		if ((selectedLevel == null) || (selectedLevel.Length == 0)) {
			throw new Exception ("Bad selected level");
		}
	}
}
