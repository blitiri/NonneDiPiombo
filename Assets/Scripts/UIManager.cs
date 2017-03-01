﻿using UnityEngine;
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
	}

	/// <summary>
	/// Sets a player life.
	/// </summary>
	/// <param name="stress">Level of life (a value between 0 and 1).</param>
	/// <param name="playerNumber">Player number (a 1-based index).</param>
	public void SetLife (float life, int playerNumber)
	{
		lifeSliders [playerNumber - 1].value = life;
	}

	/// <summary>
	/// Sets a player stress.
	/// </summary>
	/// <param name="stress">Level of stress (a value between 0 and 1).</param>
	/// <param name="playerNumber">Player number (a 1-based index).</param>
	public void SetStress (float stress, int playerNumber)
	{
		stressSliders [playerNumber - 1].value = stress;
	}

	/// <summary>
	/// Sets a player max ammo.
	/// </summary>
	/// <param name="maxAmmo">Max ammo.</param>
	/// <param name="playerNumber">Player number (a 1-based index).</param>
	public void SetMaxAmmo (int maxAmmo, int playerNumber)
	{
		maxAmmoLabels [playerNumber - 1].text = maxAmmo.ToString ();
	}

	/// <summary>
	/// Sets a player ammo.
	/// </summary>
	/// <param name="ammo">Ammo owned by player.</param>
	/// <param name="playerNumber">Player number (a 1-based index).</param>
	public void SetAmmo (int ammo, int playerNumber)
	{
		ammoLabels [playerNumber - 1].text = ammo.ToString ();
	}
}