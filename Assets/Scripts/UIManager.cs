using UnityEngine;
using System.Collections;

/// <summary>
/// User interface manager.
/// </summary>
public class UIManager : MonoBehaviour
{
	/// <summary>
	/// Current UIManager instance.
	/// </summary>
	public static UIManager instance;
	/// <summary>
	/// Current player scores.
	/// </summary>
	public UILabel[] playerScores;
	/// <summary>
	/// The timer label for round countdown.
	/// </summary>
	public static UILabel timerLabel;
	/// <summary>
	/// The players life sliders. Important: respect players order (first player1 lifeSlider, then player2 lifeSlider and so on)
	/// </summary>
	public UISlider[] lifeSliders;
	/// <summary>
	/// The players stress sliders. Important: respect players order (first player1 stressSlider, then player2 stressSlider and so on)
	/// </summary>
	public UISlider[] stressSliders;
	/// <summary>
	/// The players max ammo labels. Important: respect players order (first player1 maxAmmoLabel, then player2 maxAmmoLabel and so on)
	/// </summary>
	public UILabel[] maxAmmoLabels;
	/// <summary>
	/// The players ammo labels. Important: respect players order (first player1 ammoLabel, then player2 ammoLabel and so on)
	/// </summary>
	public UILabel[] ammoLabels;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		instance = this;
		timerLabel = GameObject.Find ("Timer").GetComponent<UILabel> ();
	}

	/// <summary>
	/// Sets a player life.
	/// </summary>
	/// <param name="stress">Level of life (a value between 0 and 1).</param>
	/// <param name="playerId">Player number (a 1-based index).</param>
	public void SetLife (float life, int playerId)
	{
		lifeSliders [playerId].value = life;
	}

	/// <summary>
	/// Sets a player stress.
	/// </summary>
	/// <param name="stress">Level of stress (a value between 0 and 1).</param>
	/// <param name="playerId">Player number (a 1-based index).</param>
	public void SetStress (float stress, int playerId)
	{
		stressSliders [playerId].value = stress;
	}

	/// <summary>
	/// Sets a player max ammo.
	/// </summary>
	/// <param name="maxAmmo">Max ammo.</param>
	/// <param name="playerId">Player number (a 1-based index).</param>
	public void SetMaxAmmo (int maxAmmo, int playerId)
	{
		maxAmmoLabels [playerId].text = maxAmmo.ToString ();
	}

	/// <summary>
	/// Sets a player ammo.
	/// </summary>
	/// <param name="ammo">Ammo owned by player.</param>
	/// <param name="playerId">Player number (a 1-based index).</param>
	public void SetAmmo (int ammo, int playerId)
	{
		ammoLabels [playerId].text = ammo.ToString ();
	}

	/// <summary>
	/// Sets a player Score.
	/// </summary>
	public void SetScore ()
	{
		int playerIndex;

		for (playerIndex = 0; playerIndex < playerScores.Length; playerIndex++) {
			playerScores[playerIndex].text = "" + GameManager.instance.GetPlayerKills (playerIndex);
		}
	}
}
