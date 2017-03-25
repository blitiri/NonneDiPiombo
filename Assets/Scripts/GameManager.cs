using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	//array di transform per il respawn dei player
	public Transform[] respawnPlayer;
    private float timerToSpawn = 30.0f;
    private float timerPostManSpawn;
	private int postManRandomStart;
    public GameObject postManPrefab;
	public GameObject[] players;
	private PlayerControl[] playersControls;
	public GameObject pauseScreen;
	public GameObject pauseButton;
	public GameObject quitButton;
	public float maxTimerBeforeRespawn = 1.0f;
	public SkinnedMeshRenderer[] mesh;
	public int[] playersKills;
	public bool isPaused = false;
	public float roundTimer;
	public int killerBonus = 2;
	public int killedMalus = 1;

	void Awake ()
	{
		int playerIndex;

		instance = this;
		playersControls = new PlayerControl[players.Length];
		playersKills = new int[players.Length];
		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			playersControls [playerIndex] = players [playerIndex].GetComponent<PlayerControl> ();
			playersControls [playerIndex].SetPlayerId(playerIndex);
			playersKills [playerIndex] = 0;
        }
	}

	// Update is called once per frame
	void Update ()
	{
		CheckRespawnPlayers ();
		Pause ();
		TimerSetUp ();

        if (timerPostManSpawn < timerToSpawn)
        {
            timerPostManSpawn += Time.deltaTime;
            
        }
        else
        {
            PostManSpawn();
            timerPostManSpawn = 0.0f;
        }

        UIManager.instance.SetScore ();
	}

	public void CheckRespawnPlayers ()
	{

		int playerIndex;

		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			if ((playersControls [playerIndex].GetLife () <= 0) && !playersControls [playerIndex].IsUnderAttack () || (playersControls [playerIndex].GetStress () >= 100)) {
				StartCoroutine (RespawnPlayer (playerIndex));
			}
		}
	}

	public void PlayerKilled(int killedId, int killerId) {
		Debug.Log ("killer: " + killerId + " - killed: " + killedId);
		playersKills [killedId] += killedMalus;
		playersKills [killerId] -= killerBonus;
	}

	public int GetPlayerKills(int playerIndex) {
		return playersKills [playerIndex];
	}

	IEnumerator RespawnPlayer (int playerIndex)
	{
		int spawnpointIndex;
		GameObject player;

		playersControls [playerIndex].ResetStatus ();
		player = players [playerIndex];
		SetMeshRendererEnabled (false);
		yield return new WaitForSeconds (maxTimerBeforeRespawn);
		spawnpointIndex = Random.Range (0, respawnPlayer.Length);
		player.transform.position = respawnPlayer [spawnpointIndex].position;
		SetMeshRendererEnabled (true);
	}

	private void SetMeshRendererEnabled(bool enabled) {
		SkinnedMeshRenderer meshRenderer;

		foreach(Transform child in transform) {
			meshRenderer = child.gameObject.GetComponent<SkinnedMeshRenderer> ();
			if (meshRenderer != null) {
				meshRenderer.enabled = enabled;
			}
		}
	}
    /// <summary>
    /// Spawn of the PostMan
    /// </summary>
    void PostManSpawn()
	{
		GameObject PostMan = Instantiate (postManPrefab) as GameObject;		        
    }
	
	/// <summary>
	//Countdown for round duration, back to menu at the end
	/// </summary>
	void TimerSetUp ()
	{
		roundTimer -= Time.deltaTime;
		int minutes = Mathf.FloorToInt (roundTimer / 60f);
		int seconds = Mathf.FloorToInt (roundTimer - minutes * 60);

		string realTime = string.Format ("{0:0}:{1:00}", minutes, seconds);
		UIManager.timerLabel.text = realTime;

		if (seconds == 0) {
			SceneManager.LoadScene("Menu");
		}
	}

	void SetButtonsEnabled (bool enabled)
	{
		pauseScreen.GetComponent<UISprite> ().enabled = enabled;
		pauseButton.GetComponent<BoxCollider> ().enabled = enabled;
		pauseButton.GetComponent<UISprite> ().enabled = enabled;
		pauseButton.GetComponentInChildren<UILabel> ().enabled = enabled;
		quitButton.GetComponent<BoxCollider> ().enabled = enabled;
		quitButton.GetComponent<UISprite> ().enabled = enabled;
		quitButton.GetComponentInChildren<UILabel> ().enabled = enabled;
	}

	void Pause ()
	{
		/// <summary>
		//Pause is on/off
		/// </summary>
		if (isPaused == true) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isPaused = false;
				Destroy (pauseScreen.GetComponent<TweenAlpha> ());
				SetButtonsEnabled (false);
			}

			Time.timeScale = 0;
		} else if (isPaused == false) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isPaused = true;
				AddTween ();
				SetButtonsEnabled (true);
			}
			
			Time.timeScale = 1;
		}
	}

	/// <summary>
	//Adding tween to screen transition
	/// </summary>
	void AddTween ()
	{
		pauseScreen.AddComponent<TweenAlpha> ().from = 0;
		pauseScreen.GetComponent<TweenAlpha> ().to = 1;
		pauseScreen.GetComponent<TweenAlpha> ().duration = 1.7f;
	}

	/// <summary>
	//Clicking on Pause button
	/// </summary>
	public void OnClickPauseButton ()
	{
		isPaused = false;
		Destroy (pauseScreen.GetComponent<TweenAlpha> ());
		SetButtonsEnabled (false);
	}

	/// <summary>
	//Clicking on Quit button
	/// </summary>
	public void OnClickQuitButton ()
	{
		Application.Quit ();
	}
}
