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

	void Awake ()
	{
		instance = this;
	}

	/// <summary>
	/// Determines if is level scene.
	/// </summary>
	/// <returns><c>true</c> if is level scene; otherwise, <c>false</c>.</returns>
	public static bool IsLevelScene ()
	{
		Scene activeScene;
		bool levelScene;

		levelScene = false;
		activeScene = SceneManager.GetActiveScene ();
		if (activeScene.name.StartsWith (levelPrefix)) {
			levelScene = true;
		}
		return levelScene;
	}

	public static bool IsEndingScene ()
	{
		Scene activeScene;
		bool levelScene;

		levelScene = false;
		activeScene = SceneManager.GetActiveScene ();
		if (activeScene.name.Equals ("Ending1")) {
			levelScene = true;
		}
		return levelScene;
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
	/// Loads a scene.
	/// </summary>
	/// <param name="button">Button that require scene loading.</param>
	public void LoadScene (GameObject button)
	{
//		if (!SceneManager.GetActiveScene().name.StartsWith(levelPrefix))
//        {
		LoadScene (button.tag);
//        }
	}

	/// <summary>
	/// Loads a scene.
	/// </summary>
	/// <param name="sceneName">Scene name to load.</param>
	public void LoadScene (string sceneName)
	{
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
	}

	/// <summary>
	/// Clicking on Quit button
	/// </summary>
	public void OnClickQuitButton ()
	{
		Application.Quit ();
	}
}
