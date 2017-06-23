using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Configuration.
/// </summary>
public class Configuration : MonoBehaviour
{
    /// <summary>
    /// Configuration instance.
    /// </summary>
    public static Configuration instance;
    /// <summary>
    /// The max number of players.
    /// </summary>
    public const int maxNumberOfPlayers = 4;
    /// <summary>
    /// The minimum number of players.
    /// </summary>
    private const int minNumberOfPlayers = 2;
    /// <summary>
    /// The grannies prefabs.
    /// </summary>
    public GameObject[] granniesPrefabs;
    /// <summary>
    /// The default number of players.
    /// </summary>
    [Range(minNumberOfPlayers, maxNumberOfPlayers)]
    public int defaultNumberOfPlayers = maxNumberOfPlayers;
    /// <summary>
    /// The default sound volume.
    /// </summary>
    public float defaultSoundVolume = 1;
    /// <summary>
    /// The players colors.
    /// </summary>
    public Color[] playersColors = { Color.red, Color.green, Color.blue, Color.yellow };
    /// <summary>
    /// The color of the top locked.
    /// </summary>
    public Color topSelectedLockedColor = Color.gray;
    /// <summary>
    /// The color of the bottom locked.
    /// </summary>
    public Color bottomSelectedLockedColor = Color.black;
    /// <summary>
    /// The color of the top locked.
    /// </summary>
    public Color topUnselectedLockedColor = Color.white;
    /// <summary>
    /// The color of the bottom locked.
    /// </summary>
    public Color bottomUnselectedLockedColor = Color.gray;
    /// <summary>
    /// The color of the top selected.
    /// </summary>
    public Color topSelectedColor = Color.yellow;
    /// <summary>
    /// The color of the bottom selected.
    /// </summary>
    public Color bottomSelectedColor = Color.red;
    /// <summary>
    /// The color of the top unselected.
    /// </summary>
    public Color topUnselectedColor = Color.gray;
    /// <summary>
    /// The color of the bottom unselected.
    /// </summary>
    public Color bottomUnselectedColor = Color.black;
    /// <summary>
    /// The number of players.
    /// </summary>
    private int numberOfPlayers;
    /// <summary>
    /// The selected level.
    /// </summary>
    private string selectedLevel;
    /// <summary>
    /// The full screen.
    /// </summary>
    private bool fullScreen;
    /// <summary>
    /// The sound volume.
    /// </summary>
    private float soundVolume;
    /// <summary>
    /// The selected grannies.
    /// </summary>
    public IList<GameObject> selectedGrannies;
    /// <summary>
    /// Players on the basis of who pressed Cross button first.
    /// </summary>
    public List<Rewired.Player> players;

    /// <summary>
    /// Awake the script.
    /// </summary>
    void Awake()
    {
        int grannyIndex;

        if (instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            if (SceneController.IsLevelSelectionScene() || SceneController.IsLevelScene() || SceneController.IsCharacterSelectionScene() || SceneController.IsMenuScene() || SceneController.IsEndingScene())
            {
                numberOfPlayers = defaultNumberOfPlayers;
                selectedGrannies = new GameObject[numberOfPlayers];
                for (grannyIndex = 0; grannyIndex < numberOfPlayers; grannyIndex++)
                {
                    selectedGrannies[grannyIndex] = granniesPrefabs[grannyIndex % granniesPrefabs.Length];
                }
            }
            else
            {
                numberOfPlayers = 0;
                selectedGrannies = new List<GameObject>();
            }
            soundVolume = defaultSoundVolume;
            fullScreen = Screen.fullScreen;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the number of players. The value is clamped between 0 and maximum number of players
    /// </summary>
    /// <param name="numberOfPlayers">Number of players.</param>
    public void SetNumberOfPlayers(int numberOfPlayers)
    {
        this.numberOfPlayers = Mathf.Clamp(minNumberOfPlayers, 0, maxNumberOfPlayers);
        ResizeSelectedGrannies();
    }

    /// <summary>
    /// Resizes the selected grannies.
    /// </summary>
    private void ResizeSelectedGrannies()
    {
        // There are too many grannies
        while (selectedGrannies.Count > numberOfPlayers)
        {
            selectedGrannies.RemoveAt(selectedGrannies.Count - 1);
        }
        // There are too few grannies
        while (selectedGrannies.Count < numberOfPlayers)
        {
            selectedGrannies.Add(granniesPrefabs[0]);
        }
    }

    /// <summary>
    /// Gets the number of players.
    /// </summary>
    /// <returns>The number of players.</returns>
    public int GetNumberOfPlayers()
    {
        return numberOfPlayers;
    }

    /// <summary>
    /// Sets the selected level.
    /// </summary>
    /// <param name="selectedLevel">Selected level.</param>
    public void SetSelectedLevel(string selectedLevel)
    {
        this.selectedLevel = selectedLevel;
    }

    /// <summary>
    /// Gets the selected level.
    /// </summary>
    /// <returns>The selected level.</returns>
    public string GetSelectedLevel()
    {
        return selectedLevel;
    }

    /// <summary>
    /// Sets the sound volume.
    /// </summary>
    /// <param name="soundVolume">Sound volume.</param>
    public void SetSoundVolume(float soundVolume)
    {
        this.soundVolume = soundVolume;
    }

    /// <summary>
    /// Gets the sound volume.
    /// </summary>
    /// <returns>The sound volume.</returns>
    public float GetSoundVolume()
    {
        return soundVolume;
    }

    /// <summary>
    /// Sets the full screen.
    /// </summary>
    /// <param name="fullScreen">If set to <c>true</c> full screen.</param>
    public void SetFullScreen(bool fullScreen)
    {
        this.fullScreen = fullScreen;
    }

    /// <summary>
    /// Determines whether this instance is full screen.
    /// </summary>
    /// <returns><c>true</c> if this instance is full screen; otherwise, <c>false</c>.</returns>
    public bool IsFullScreen()
    {
        return fullScreen;
    }

    /// <summary>
    /// Sets the selected granny.
    /// </summary>
    /// <param name="granny">Granny.</param>
    /// <param name="playerIndex">Player index.</param>
    public void SetSelectedGranny(GameObject granny, int playerIndex)
    {
        selectedGrannies[playerIndex] = granny;
    }

    /// <summary>
    /// Gets the selected grannies.
    /// </summary>
    /// <returns>The selected grannies.</returns>
    public IList<GameObject> GetSelectedGrannies()
    {
        return selectedGrannies;
    }

    /// <summary>
    /// Validate the number of player.
    /// </summary>
    public void ValidateNumberOfPlayer()
    {
        if ((numberOfPlayers < minNumberOfPlayers) || (numberOfPlayers > maxNumberOfPlayers))
        {
            throw new Exception("Bad players number");
        }
    }

    /// <summary>
    /// Validates the selected level.
    /// </summary>
    public void ValidateSelectedLevel()
    {
        if ((selectedLevel == null) || (selectedLevel.Length == 0))
        {
            throw new Exception("Bad selected level");
        }
    }
}
