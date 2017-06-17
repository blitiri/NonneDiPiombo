﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// GameManager instance.
	/// </summary>
	public static GameManager instance;
	/// <summary>
	/// Skill Counter.
	/// </summary>
	public int catCounter;

	public bool isPaused = false;
	public float timerToSpawn = 5.0f;
	private float timerPostManSpawn;
	public float maxTimerBeforeRespawn = 1.0f;
	public float roundTimer;

	public Transform cameraTransform;

	public GameObject rewiredInputManagerPrefab;
	public GameObject pauseMenu;
    public TweenAlpha pauseMenuTweenAlpha;

	// array di transform per il respawn dei player
	public Transform[] playersRespawns;
	public Transform[] playersStartRespawns;

	public GameObject[] players;
	private PlayerControl[] playersControls;
	public GameObject[] weapons;

	public GameObject spawnVFXPrefab;

	void Awake ()
	{
		instance = this;
		Statistics.instance.Reset ();
        Instantiate (rewiredInputManagerPrefab);
		InitPlayers ();
	}

	void Start ()
	{
		LevelUIManager.instance.InitUI ();
	}

	// Update is called once per frame
	void Update ()
	{
		//CheckRespawnPlayers ();
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CheckGamePause ();
		}
		if (!isPaused) {
			TimerUpdate ();
		}
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
			if (Configuration.instance.selectedGrannies [playerIndex] != null) {
				players [playerIndex] = Instantiate (Configuration.instance.selectedGrannies [playerIndex]) as GameObject;
				players [playerIndex].transform.position = playersStartRespawns [playerIndex].position;
			} else {
				players [playerIndex] = Instantiate (Configuration.instance.granniesPrefabs [Random.Range (0, Configuration.instance.granniesPrefabs.Length)]) as GameObject;
				players [playerIndex].transform.position = playersStartRespawns [playerIndex].position;
			}
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
			if ((playersControls [playerIndex].isDead) || (playersControls [playerIndex].IsCollapsed ())) {
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
		LevelUIManager.instance.SetScore (Statistics.instance.getPlayersKills () [killedId], killedId);
		LevelUIManager.instance.SetScore (Statistics.instance.getPlayersKills () [killerId], killerId);
	}

	private IEnumerator RespawnPlayer (int playerIndex)
	{
		int spawnpointIndex;
		GameObject player;

		player = players [playerIndex];
		SetMeshRendererEnabled (false, playerIndex);



		PlayerControl playerControl;
		playerControl = player.GetComponent<PlayerControl> ();
		playerControl.ResetStatus ();
		playerControl.stopInputPlayer = true;

		BoxCollider playerCollider;
		playerCollider = player.GetComponent<BoxCollider> ();
		playerCollider.enabled = false;

		yield return new WaitForSeconds (maxTimerBeforeRespawn);


		spawnpointIndex = UnityEngine.Random.Range (0, playersRespawns.Length);
		player.transform.position = playersRespawns [spawnpointIndex].position;
		GameObject spawnVFX = Instantiate (spawnVFXPrefab, player.transform.position + new Vector3 (0, 1, 0), spawnVFXPrefab.transform.rotation) as GameObject;
		Destroy (spawnVFX, 3f);
		SetMeshRendererEnabled (true, playerIndex);
		playerControl.stopInputPlayer = false;
		playerControl.isDead = false;
		playerCollider.enabled = true;

		//playerControl.isImmortal = true;
		yield return new WaitForSeconds (playerControl.immortalTime);
		playerControl.isImmortal = false;
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

	private void SetPauseMenu (bool enabled)
	{
		pauseMenu.SetActive (enabled);
	}

	/// <summary>
	/// Pause is on/off
	/// </summary>
	public void CheckGamePause ()
	{
		if (isPaused) {
			ResumeGame ();
		} else {
			PauseGame ();
		}
	}

	/// <summary>
	/// Resumes the play.
	/// </summary>
	public void ResumeGame ()
	{
        pauseMenuTweenAlpha.from = 0;
        pauseMenuTweenAlpha.SetEndToCurrentValue();
        pauseMenuTweenAlpha.PlayReverse();
        isPaused = false;
		SetPauseMenu (false);
	}

	public void PauseGame ()
	{
		isPaused = true;
		SetPauseMenu (true);
        pauseMenuTweenAlpha.SetStartToCurrentValue();
        pauseMenuTweenAlpha.to = 1;
        pauseMenuTweenAlpha.PlayForward();

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
