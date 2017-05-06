using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level item manager.
/// </summary>
public class LevelItemManager : MonoBehaviour
{
	/// <summary>
	/// The locked icon.
	/// </summary>
	public UISprite lockedIcon;
	/// <summary>
	/// The level screenshot.
	/// </summary>
	public UISprite levelScreenshot;
	/// <summary>
	/// The level label.
	/// </summary>
	public UILabel levelLabel;
	/// <summary>
	/// The name of the level.
	/// </summary>
	public string levelName;
	/// <summary>
	/// Flag to indicate level is locked.
	/// </summary>
	public bool locked;
	/// <summary>
	/// Flag to indicate this is the default level.
	/// </summary>
	public bool defaultLevel;
	/// <summary>
	/// The color of the top locked.
	/// </summary>
	public Color topLockedColor = Color.white;
	/// <summary>
	/// The color of the bottom locked.
	/// </summary>
	public Color bottomLockedColor = Color.gray;
	/// <summary>
	/// The color of the top selected.
	/// </summary>
	public Color topSelectedColor = Color.yellow;
	/// <summary>
	/// The color of the bottom selected.
	/// </summary>
	public Color bottomSelectedColor = Color.red;

	/// <summary>
	/// Start script.
	/// </summary>
	void Start ()
	{
		lockedIcon.enabled = locked;
		if (locked) {
			levelLabel.gradientTop = topLockedColor;
			levelLabel.gradientBottom = bottomLockedColor;
		}
		if (locked && defaultLevel) {
			throw new UnityException ("A locked level can't be the default level");
		}
		if (defaultLevel) {
			OnClick ();
		}
	}

	/// <summary>
	/// Raises the click event.
	/// </summary>
	public void OnClick ()
	{
		levelScreenshot.spriteName = levelName;
		if (!locked) {
			Configuration.instance.SetSelectedLevel (levelName);
			levelLabel.gradientTop = topSelectedColor;
			levelLabel.gradientBottom = bottomSelectedColor;
		}
	}
}
