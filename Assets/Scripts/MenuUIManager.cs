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
	public string nextSceneName = "CharacterSelectionMenu";
	/// <summary>
	/// The volume slider.
	/// </summary>
	public UISlider volumeSlider;
	/// <summary>
	/// The checked sprite.
	/// </summary>
	public UISprite checkedSprite;
	/// <summary>
	/// The options window.
	/// </summary>
	public UIPanel optionsWindow;
	/// <summary>
	/// The credits window.
	/// </summary>
	public UIPanel creditsWindow;

	/// <summary>
	/// Start the script.
	/// </summary>
	void Start ()
	{
		optionsWindow.gameObject.SetActive (false);
		creditsWindow.gameObject.SetActive (false);
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	public void OnStart ()
	{
		SceneController.instance.LoadSceneByName (nextSceneName);
	}

	/// <summary>
	/// Opens options window.
	/// </summary>
	public void OnOptions ()
	{
		Debug.Log ("Open - Fullscreen: " + Configuration.instance.IsFullScreen ());
		Debug.Log ("Open - Sound volume: " + Configuration.instance.GetSoundVolume ());
		checkedSprite.enabled = Configuration.instance.IsFullScreen ();
		volumeSlider.value = Configuration.instance.GetSoundVolume ();
		optionsWindow.gameObject.SetActive (true);
	}

	/// <summary>
	/// Opens credits windows.
	/// </summary>
	public void OnCredits ()
	{
		creditsWindow.gameObject.SetActive (true);
	}

	/// <summary>
	/// Quits the game.
	/// </summary>
	public void OnQuit ()
	{
		Application.Quit ();
	}

	/// <summary>
	/// Set fullscreen flag.
	/// </summary>
	public void OnFullscreen ()
	{
		checkedSprite.enabled = !checkedSprite.enabled;
	}

	/// <summary>
	/// Closes a window.
	/// </summary>
	/// <param name="window">Window to close.</param>
	public void OnClose (GameObject window)
	{
		window.SetActive (false);
		if (window.name.Equals ("OptionsWindow")) {
			Configuration.instance.SetFullScreen (checkedSprite.enabled);
			Configuration.instance.SetSoundVolume (volumeSlider.value); 
			Debug.Log ("Close - Fullscreen: " + Configuration.instance.IsFullScreen ());
			Debug.Log ("Close - Sound volume: " + Configuration.instance.GetSoundVolume ());
		}
	}
}
