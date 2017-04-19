using UnityEngine;

/// <summary>
/// Generic User interface manager.
/// </summary>
public abstract class AbstractUIManager : MonoBehaviour
{
	/// <summary>
	/// The players colors.
	/// </summary>
	public Color[] playersColors = { Color.red, Color.green, Color.blue, Color.yellow };

	/// <summary>
	/// Gets the player icon.
	/// </summary>
	/// <returns>The player icon.</returns>
	/// <param name="player">Player.</param>
	protected string GetPlayerIcon (GameObject player)
	{
		return player.name.Replace ("(Clone)", "") + "PlayerIcon";
	}
}
