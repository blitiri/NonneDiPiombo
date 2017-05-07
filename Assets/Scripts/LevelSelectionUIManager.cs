using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level selection user interface manager.
/// </summary>
public class LevelSelectionUIManager : MonoBehaviour
{
	/// <summary>
	/// The levels list.
	/// </summary>
	public GameObject levelsList;
	/// <summary>
	/// The level screenshot.
	/// </summary>
	public UISprite levelScreenshot;
	/// <summary>
	/// The start button.
	/// </summary>
	public UIButton startButton;
	/// <summary>
	/// The level item managers.
	/// </summary>
	private LevelItemInfo[] levelItemManagers;

	/// <summary>
	/// Awake the script.
	/// </summary>
	void Awake ()
	{
		levelItemManagers = levelsList.GetComponentsInChildren<LevelItemInfo> ();
	}

	/// <summary>
	/// Start the script.
	/// </summary>
	void Start ()
	{
		// Select the default level
		foreach (LevelItemInfo levelItemManager in levelItemManagers) {
			InitLevelItem (levelItemManager);
			if (levelItemManager.defaultLevel) {
				OnClick (levelItemManager.gameObject);
			}
		}
	}

	/// <summary>
	/// Raises the click event.
	/// </summary>
	/// <param name="levelItem">Level item.</param>
	public void OnClick (GameObject levelItem)
	{
		LevelItemInfo selectedLevelItemInfo = null;

		foreach (LevelItemInfo levelItemManager in levelItemManagers) {
			if (levelItem.gameObject == levelItemManager.gameObject) {
				selectedLevelItemInfo = levelItemManager;
			} else {
				// Deselect all other level items
				Deselect (levelItemManager);
			}
		}
		if (selectedLevelItemInfo != null) {
			// Select the clicked level item
			Select (selectedLevelItemInfo);
			levelScreenshot.spriteName = selectedLevelItemInfo.levelName;
		}
		// TODO non è la proprietà giusta per disabilitare il bottone
		startButton.enabled = (Configuration.instance.GetSelectedLevel () != null);
	}

	/// <summary>
	/// Inits a level item.
	/// </summary>
	/// <param name="levelItemManager">Level item manager to init.</param>
	private void InitLevelItem (LevelItemInfo levelItemManager)
	{
		levelItemManager.lockedIcon.enabled = levelItemManager.locked;
		if (levelItemManager.locked) {
			levelItemManager.levelLabel.gradientTop = Configuration.instance.topUnselectedLockedColor;
			levelItemManager.levelLabel.gradientBottom = Configuration.instance.bottomUnselectedLockedColor;
		} else {
			levelItemManager.levelLabel.gradientTop = Configuration.instance.topUnselectedColor;
			levelItemManager.levelLabel.gradientBottom = Configuration.instance.bottomUnselectedColor;
		}
		if (levelItemManager.locked && levelItemManager.defaultLevel) {
			throw new UnityException ("A locked level can't be the default level");
		}
	}

	/// <summary>
	/// Select a level item.
	/// </summary>
	/// <param name="levelItemManager">Level item manager to select.</param>
	public void Select (LevelItemInfo levelItemManager)
	{
		if (levelItemManager.locked) {
			levelItemManager.levelLabel.gradientTop = Configuration.instance.topSelectedLockedColor;
			levelItemManager.levelLabel.gradientBottom = Configuration.instance.bottomSelectedLockedColor;
		} else {
			Configuration.instance.SetSelectedLevel (levelItemManager.levelName);
			levelItemManager.levelLabel.gradientTop = Configuration.instance.topSelectedColor;
			levelItemManager.levelLabel.gradientBottom = Configuration.instance.bottomSelectedColor;
		}
	}

	/// <summary>
	/// Deselect a level item.
	/// </summary>
	/// <param name="levelItemManager">Level item manager to deselect.</param>
	public void Deselect (LevelItemInfo levelItemManager)
	{
		if (levelItemManager.locked) {
			levelItemManager.levelLabel.gradientTop = Configuration.instance.topUnselectedLockedColor;
			levelItemManager.levelLabel.gradientBottom = Configuration.instance.bottomUnselectedLockedColor;
		} else {
			Configuration.instance.SetSelectedLevel (null);
			levelItemManager.levelLabel.gradientTop = Configuration.instance.topUnselectedColor;
			levelItemManager.levelLabel.gradientBottom = Configuration.instance.bottomUnselectedColor;
		}
	}

	/// <summary>
	/// Start the game.
	/// </summary>
	public void OnStart ()
	{
		SceneController.instance.LoadSelectedScene ();
	}
}
