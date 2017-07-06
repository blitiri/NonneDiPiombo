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
	/// Tween alpha delle opzioni
	/// </summary>
	public TweenAlpha optionsTweenAlpha;
	/// <summary>
	/// Tween alpha delle crediti
	/// </summary>
	public TweenAlpha creditsTweenAlpha;
	/// <summary>
	/// The audio source.
	/// </summary>
	public AudioSource audioSource;
	/// <summary>
	/// Default screen width
	/// </summary>
	private int defWidth;
	/// <summary>
	/// Default screen height
	/// </summary>
	private int defHeight;

	/// <summary>
	/// Awake the script.
	/// </summary>
	void Awake ()
	{
		defWidth = Screen.width; 
		defHeight = Screen.height; 
	}

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
		Debug.Log ("Open - Sound volume: " + Configuration.instance.GetSoundVolume ());
		checkedSprite.enabled = Configuration.instance.IsFullScreen ();
		audioSource.volume = Configuration.instance.GetSoundVolume ();
		Utility.OpenPopup (optionsWindow, optionsTweenAlpha);
	}

	/// <summary>
	/// Opens credits windows.
	/// </summary>
	public void OnCredits ()
	{
		Utility.OpenPopup (creditsWindow, creditsTweenAlpha);
		CreditsScrollManager.instance.StartScroll ();
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
		SetFullScreen (checkedSprite.enabled);
	}

	/// <summary>
	/// Update sound volume.
	/// </summary>
	public void OnValueChange ()
	{
		audioSource.volume = volumeSlider.value;
	}

	/// <summary>
	/// Closes a window.
	/// </summary>
	/// <param name="window">Window to close.</param>
	public void OnClose (GameObject window)
	{
		//window.SetActive (false);
		if (window.name.Equals ("OptionsWindow")) {
			Configuration.instance.SetFullScreen (checkedSprite.enabled);
			Configuration.instance.SetSoundVolume (volumeSlider.value); 
			audioSource.volume = Configuration.instance.GetSoundVolume ();
			Debug.Log ("Close - Sound volume: " + Configuration.instance.GetSoundVolume ());
			Utility.FadeOut (optionsTweenAlpha, this, "Close" + window.name);
		} else if (window.name.Equals ("CreditsWindow")) {
			Utility.FadeOut (creditsTweenAlpha, this, "Close" + window.name);
			CreditsScrollManager.instance.StopScroll ();
		}
	}

	/// <summary>
	/// Closes the options window.
	/// </summary>
	private void CloseOptionsWindow ()
	{
		optionsWindow.gameObject.SetActive (false);
	}

	/// <summary>
	/// Closes the credits window.
	/// </summary>
	private void CloseCreditsWindow ()
	{
		creditsWindow.gameObject.SetActive (false);
	}

	/// <summary>
	/// Sets the full screen.
	/// </summary>
	/// <param name="fullScreen">If set to <c>true</c> full screen.</param>
	private void SetFullScreen (bool fullScreen)
	{
		Screen.fullScreen = fullScreen;
		if (fullScreen) {
			Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true);
		} else {
			Screen.SetResolution (defWidth, defHeight, false);
		}
	}
}
