using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene controller.
/// </summary>
public class SceneController : MonoBehaviour
{
	/// <summary>
	/// Current SceneController instance.
	/// </summary>
	public static SceneController instance;
	/// <summary>
	/// The level scene prefix.
	/// </summary>
	private const string levelPrefix = "Level";
	/// <summary>
	/// The last level scene loaded (default is LevelPostOffice2).
	/// </summary>
	private string lastLevelSceneLoaded = "LevelPostOffice2";

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	/// <summary>
	/// Determines if is level scene.
	/// </summary>
	/// <returns><c>true</c> if is level scene; otherwise, <c>false</c>.</returns>
	public static bool IsLevelScene ()
	{
		Scene activeScene;

		activeScene = SceneManager.GetActiveScene ();
		//Debug.Log ("Active scene: " + activeScene.name);
		return IsLevelScene (activeScene.name);
	}

	public static bool IsLevelScene (string sceneName)
	{
		//Debug.Log (sceneName + ".StartsWith ('" + levelPrefix + "'): " + sceneName.StartsWith (levelPrefix));
		return (sceneName.StartsWith (levelPrefix));
	}

	/// <summary>
	/// Determines if ending scene is loaded.
	/// </summary>
	/// <returns><c>true</c> if ending scene is loaded; otherwise, <c>false</c>.</returns>
	public static bool IsEndingScene ()
	{
		return IsScene ("Ending");
	}

	/// <summary>
	/// Determines if is character selection scene.
	/// </summary>
	/// <returns><c>true</c> if is character selection scene; otherwise, <c>false</c>.</returns>
	public static bool IsCharacterSelectionScene ()
	{
		return IsScene ("CharacterSelection");
	}

	/// <summary>
	/// Determines if is level selection scene.
	/// </summary>
	/// <returns><c>true</c> if is level selection scene; otherwise, <c>false</c>.</returns>
	public static bool IsLevelSelectionScene ()
	{
		return IsScene ("LevelSelection");
	}

	/// <summary>
	/// Determines if is scene the specified sceneName.
	/// </summary>
	/// <returns><c>true</c> if is scene the specified sceneName; otherwise, <c>false</c>.</returns>
	/// <param name="sceneName">Scene name.</param>
	public static bool IsScene (string sceneName)
	{
		Scene activeScene;
		bool isScene;

		isScene = false;
		activeScene = SceneManager.GetActiveScene ();
		if (activeScene.name.Equals (sceneName)) {
			isScene = true;
		}
		return isScene;
	}

	/// <summary>
	/// Gets the name of the current scene.
	/// </summary>
	/// <returns>The current scene name.</returns>
	public static string GetCurrentSceneName ()
	{
		return SceneManager.GetActiveScene ().name;
	}

	/// <summary>
	/// Loads the selected scene.
	/// </summary>
	public void LoadSelectedScene ()
	{
		LoadSceneByName (Configuration.instance.GetSelectedLevel ());
	}

	/// <summary>
	/// Loads a scene.
	/// </summary>
	/// <param name="button">Button that require scene loading.</param>
	public void LoadScene (GameObject button)
	{
//		if (!SceneManager.GetActiveScene().name.StartsWith(levelPrefix))
//        {
		LoadSceneByName (button.tag);
//        }
	}

	/// <summary>
	/// Loads a scene.
	/// </summary>
	/// <param name="sceneName">Scene name to load.</param>
	public void LoadSceneByName (string sceneName)
	{
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
		if (IsLevelScene (sceneName)) {
			lastLevelSceneLoaded = sceneName;
		}
	}

	/// <summary>
	/// Clicking on Quit button
	/// </summary>
	public void OnClickQuitButton ()
	{
		Application.Quit ();
	}

	/// <summary>
	/// Gets the last level scene loaded.
	/// </summary>
	/// <returns>The last level scene loaded.</returns>
	public string GetLastLevelSceneLoaded ()
	{
		return lastLevelSceneLoaded;
	}
}
