﻿using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// GameManager instance.
	/// </summary>
	public static GameManager instance;

	private bool isPaused = false;
	public float timerToSpawn = 5.0f;
	private float timerPostManSpawn;
	public float maxTimerBeforeRespawn = 1.0f;
	public float roundTimer;

	public BoxCollider restartButtonBoxCollider;
	public UISprite restartButtonUISprite;
	public UILabel restartButtonUILabel;
	public Transform cameraTransform;

	public GameObject postManPrefab;
	public GameObject rewiredInputManagerPrefab;
	public GameObject pauseScreen;
	public GameObject pauseButton;
	public GameObject restartButton;
	public GameObject quitButton;

	// array di transform per il respawn dei player
	public Transform[] playersRespawns;
	public Transform[] playersStartRespawns;

	private GameObject[] players;
	private PlayerControl[] playersControls;
	public GameObject[] weapons;

	void Awake ()
	{
		instance = this;
		Statistics.instance.Reset ();
		Instantiate (rewiredInputManagerPrefab);
		InitPlayers ();
		restartButtonBoxCollider = restartButton.GetComponent<BoxCollider> ();
		restartButtonUISprite = restartButton.GetComponent<UISprite> ();
		restartButtonUILabel = restartButton.GetComponentInChildren<UILabel> ();
	}

	void Start ()
	{
		LevelUIManager.instance.InitUI ();
	}

	// Update is called once per frame
	void Update ()
	{
		CheckRespawnPlayers ();
		CheckGamePause ();
		TimerUpdate ();
	}

	private void InitPlayers ()
	{
		int playerIndex;
		int numberOfPlayers;

		numberOfPlayers = Configuration.instance.GetNumberOfPlayers ();
		players = new GameObject[numberOfPlayers];
		playersControls = new PlayerControl[numberOfPlayers];
		for (playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++) {
			// Instantiate player
			players [playerIndex] = Instantiate (Configuration.instance.granniesPrefabs [playerIndex % Configuration.instance.granniesPrefabs.Length]) as GameObject;
			players [playerIndex].transform.position = playersStartRespawns [playerIndex].position;
			// Initialize PlayerControl script
			playersControls [playerIndex] = players [playerIndex].GetComponent<PlayerControl> ();
			playersControls [playerIndex].SetPlayerId (playerIndex);
			playersControls [playerIndex].SetAngleCorrection (cameraTransform.rotation.eulerAngles.y);
			players [playerIndex].SetActive (true);
		}
	}

	/// <summary>
	/// Determines if round ended.
	/// </summary>
	/// <returns><c>true</c> if round ended; otherwise, <c>false</c>.</returns>
	public bool IsRoundEnded ()
	{
		return roundTimer <= 0;
	}

	public void CheckRespawnPlayers ()
	{

		int playerIndex;

		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			if ((playersControls [playerIndex].IsDead ()) || (playersControls [playerIndex].IsCollapsed ())) {
				StartCoroutine (RespawnPlayer (playerIndex));
			}
		}
	}

	/// <summary>
	/// Notify a player is killed.
	/// </summary>
	/// <param name="killedId">Killed identifier.</param>
	/// <param name="killerId">Killer identifier.</param>
	public void PlayerKilled (int killedId, int killerId)
	{
		Statistics.instance.PlayerKilled (killedId, killerId);
	}

	private IEnumerator RespawnPlayer (int playerIndex)
	{
		int spawnpointIndex;
		GameObject player;

		playersControls [playerIndex].ResetStatus ();
		player = players [playerIndex];
		SetMeshRendererEnabled (false, playerIndex);
		player.GetComponent<PlayerControl> ().stopInputPlayer = true;
		player.GetComponent<PlayerControl> ().dead = true;
		player.GetComponent<BoxCollider> ().enabled = false;

		yield return new WaitForSeconds (maxTimerBeforeRespawn);

		spawnpointIndex = UnityEngine.Random.Range (0, playersRespawns.Length);
		player.transform.position = playersRespawns [spawnpointIndex].position;
		SetMeshRendererEnabled (true, playerIndex);
		player.GetComponent<PlayerControl> ().stopInputPlayer = false;
		player.GetComponent<PlayerControl> ().dead = false;
		player.GetComponent<BoxCollider> ().enabled = true;
	}

	private void SetMeshRendererEnabled (bool enabled, int playerIndex)
	{
		Renderer meshRenderer;
		GameObject player;

		player = players [playerIndex];
		player.GetComponent<Renderer> ().enabled = enabled;

		foreach (Transform child in player.transform) {
			meshRenderer = child.gameObject.GetComponent<Renderer> ();
			if (meshRenderer != null) {
				meshRenderer.enabled = enabled;
			}
		}
	}

	/// <summary>
	/// Spawn of the PostMan
	/// </summary>
	private void PostManSpawn ()
	{
		GameObject postMan;
	
		postMan = Instantiate (postManPrefab) as GameObject;
	}

	/// <summary>
	/// Update round countdown, back to menu at the end
	/// </summary>
	private void TimerUpdate ()
	{
		roundTimer = Mathf.Clamp (roundTimer - Time.deltaTime, 0, float.MaxValue);
		LevelUIManager.instance.SetTimer (roundTimer);
		if (roundTimer == 0) {
			SceneController.instance.LoadSceneByName ("Ending");
			LevelUIManager.instance.InitUI ();
		}
	}

	private void SetButtonsEnabled (bool enabled)
	{
		pauseScreen.GetComponent<UISprite> ().enabled = enabled;
		pauseButton.GetComponent<BoxCollider> ().enabled = enabled;
		pauseButton.GetComponent<UISprite> ().enabled = enabled;
		pauseButton.GetComponentInChildren<UILabel> ().enabled = enabled;
		restartButtonBoxCollider.enabled = enabled;
		restartButtonUISprite.enabled = enabled;
		restartButtonUILabel.enabled = enabled;
		quitButton.GetComponent<BoxCollider> ().enabled = enabled;
		quitButton.GetComponent<UISprite> ().enabled = enabled;
		quitButton.GetComponentInChildren<UILabel> ().enabled = enabled;
	}

	/// <summary>
	/// Pause is on/off
	/// </summary>
	private void CheckGamePause ()
	{
		if (isPaused) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isPaused = false;
				Destroy (pauseScreen.GetComponent<TweenAlpha> ());
				SetButtonsEnabled (false);
			}
			Time.timeScale = 0;
		} else {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isPaused = true;
				AddTween ();
				SetButtonsEnabled (true);
			}
			Time.timeScale = 1;
		}
	}

	/// <summary>
	/// Adding tween to screen transition
	/// </summary>
	private void AddTween ()
	{
		pauseScreen.AddComponent<TweenAlpha> ().from = 0;
		pauseScreen.GetComponent<TweenAlpha> ().to = 1;
		pauseScreen.GetComponent<TweenAlpha> ().duration = 1.7f;
	}

	/// <summary>
	/// Clicking on Pause button
	/// </summary>
	public void OnClickPauseButton ()
	{
		isPaused = false;
		Destroy (pauseScreen.GetComponent<TweenAlpha> ());
		SetButtonsEnabled (false);
	}

	/// <summary>
	/// Gets the players.
	/// </summary>
	/// <returns>The players.</returns>
	public GameObject[] GetPlayers ()
	{
		return players;
	}
}
