﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public float timerToSpawn = 5.0f;
	public float maxTimerBeforeRespawn = 1.0f;
	public float roundTimer;

	public Transform cameraTransform;

	public GameObject rewiredInputManagerPrefab;

	// array di transform per il respawn dei player
	public Transform[] playersRespawns;
	public Transform[] playersStartRespawns;

	public GameObject[] players;

	public GameObject[] weapons;

	public GameObject spawnVFXPrefab;

	private bool isPaused = false;
	private float timerPostManSpawn;
	private PlayerControl[] playersControls;

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
			isPaused = !isPaused;
			LevelUIManager.instance.SetPauseMenuVisible (isPaused);
		}
		if (!isPaused) {
			TimerUpdate ();
		}
	}

	/// <summary>
	/// Sets game pause.
	/// </summary>
	/// <param name="pause">If set to <c>true</c> pause.</param>
	public void SetPause (bool pause)
	{
		isPaused = pause;
	}

	public void InvertPause ()
	{
		isPaused = !isPaused;
	}

	public bool IsPaused ()
	{
		return isPaused;
	}

    private void InitPlayers()
    {
        int playerIndex;
        int numberOfPlayers;

        numberOfPlayers = Configuration.instance.GetNumberOfPlayers();
        Debug.Log(numberOfPlayers);
        players = new GameObject[numberOfPlayers];
        playersControls = new PlayerControl[numberOfPlayers];
        for (playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++)
        {
            // Instantiate player
            if (Configuration.instance.selectedGrannies[playerIndex] != null)
            {
                players[playerIndex] = Instantiate(Configuration.instance.selectedGrannies[playerIndex]) as GameObject;
                players[playerIndex].transform.position = playersStartRespawns[playerIndex].position;
            }
            else
            {
                Debug.Log("fuckingbitch");
                players[playerIndex] = Instantiate(Configuration.instance.granniesPrefabs[Random.Range(0, Configuration.instance.granniesPrefabs.Length)]) as GameObject;
                players[playerIndex].transform.position = playersStartRespawns[playerIndex].position;
            }
            // Initialize PlayerControl script
            playersControls[playerIndex] = players[playerIndex].GetComponent<PlayerControl>();
            playersControls[playerIndex].SetPlayerId(playerIndex);
            //foreach(Rewired.Player player in Configuration.instance.players)
            //{
            //    Debug.Log(player.id);
            //}
            //playersControls[playerIndex].SetPlayerId(Configuration.instance.players[playerIndex].id);
            playersControls[playerIndex].SetAngleCorrection(cameraTransform.rotation.eulerAngles.y);
            //playersControls[playerIndex].myPlayer = Configuration.instance.players[playerIndex];
            if (Configuration.instance.players.Count > 0)
            {
                playersControls[playerIndex].controllerId = Configuration.instance.players[playerIndex].id;
            }
            else
            {
                playersControls[playerIndex].controllerId = playerIndex;
            }
            players[playerIndex].SetActive(true);
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
		ParticleSystem[] particleSystems;
		ParticleSystem.MainModule main;
		BoxCollider playerCollider;
		PlayerControl playerControl;

		player = players [playerIndex];
		SetMeshRendererEnabled (false, playerIndex);
		playerControl = player.GetComponent<PlayerControl> ();
		playerControl.ResetStatus ();
		playerControl.stopInputPlayer = true;
	    playerCollider = player.GetComponent<BoxCollider> ();
		playerCollider.enabled = false;

		spawnpointIndex = UnityEngine.Random.Range (0, playersRespawns.Length);
		GameObject spawnVFX = Instantiate (spawnVFXPrefab, playersRespawns [spawnpointIndex].position + new Vector3 (0, 1, 0), spawnVFXPrefab.transform.rotation) as GameObject;
		particleSystems = spawnVFX.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem particles in particleSystems) 
		{
			main = particles.main;
			main.startColor = Configuration.instance.playersColors [playersControls [playerIndex].playerId];
		}
		Destroy (spawnVFX, 3f);
		player.transform.position = playersRespawns[spawnpointIndex].position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return new WaitForSeconds (maxTimerBeforeRespawn);

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

	/// <summary>
	/// Gets the players.
	/// </summary>
	/// <returns>The players.</returns>
	public GameObject[] GetPlayers ()
	{
		return players;
	}
}
