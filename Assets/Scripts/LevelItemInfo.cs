using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level item manager.
/// </summary>
public class LevelItemInfo : MonoBehaviour
{
	/// <summary>
	/// The locked icon.
	/// </summary>
	public UISprite lockedIcon;
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
}
