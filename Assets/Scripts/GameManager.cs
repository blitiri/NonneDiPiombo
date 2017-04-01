using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	[Range(2, 4)]
	public int numberOfPlayers = 2;
	public GameObject[] playersPrefabs;
	public GameObject postManPrefab;
	public GameObject rewiredInputManagerPrefab;
	public GameObject pauseScreen;
	public GameObject pauseButton;
	public GameObject quitButton;
	public Transform cameraTransform;
	// array di transform per il respawn dei player
	public Transform[] playersRespawns;
	public Transform[] playersStartRespawns;
	public float timerToSpawn = 5.0f;
	private float timerPostManSpawn;
	private PlayerControl[] playersControls;
	[SerializeField]
	private float maxTimerBeforeRespawn = 1.0f;
	private int[] playersKills;
	private bool isPaused = false;
	[SerializeField]
	private float roundTimer;
	[SerializeField]
	private int killedMalus = 1;
	[SerializeField]
	private int killerBonus = 2;
	private GameObject[] players;

	public static bool active;
	void Awake ()
	{
		int playerIndex;

		instance = this;
		Instantiate (rewiredInputManagerPrefab);
		players = new GameObject[numberOfPlayers];
		playersControls = new PlayerControl[numberOfPlayers];
		playersKills = new int[numberOfPlayers];
		for (playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++) {
			players [playerIndex] = Instantiate (playersPrefabs [playerIndex % playersPrefabs.Length]) as GameObject;
			players [playerIndex].transform.position = playersStartRespawns [playerIndex].position;
//			Debug.Log ("Player Position" + players [playerIndex].transform.position);
			playersControls [playerIndex] = players [playerIndex].GetComponent<PlayerControl> ();
			playersControls [playerIndex].SetPlayerId (playerIndex);
			playersControls [playerIndex].SetAngleCorrection (cameraTransform.rotation.eulerAngles.y);
			playersKills [playerIndex] = 0;
			players [playerIndex].SetActive (true);
		}


	}

	// Update is called once per frame
	void Update ()
	{
		CheckRespawnPlayers ();
		Pause ();
		TimerSetUp ();

		if (timerPostManSpawn < timerToSpawn) {
			timerPostManSpawn += Time.deltaTime;
            
		} else {
			PostManSpawn();
			timerPostManSpawn = 0.0f;
		}

		UIManager.instance.SetScore ();
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
			if ((playersControls [playerIndex].GetLife () <= 0) && !playersControls [playerIndex].IsUnderAttack () || (playersControls [playerIndex].GetStress () >= 100)) {
				StartCoroutine (RespawnPlayer (playerIndex));
			}
		}
	}

	public void PlayerKilled (int killedId, int killerId)
	{
		Debug.Log ("killer: " + killerId + " - killed: " + killedId);
		playersKills [killedId] += killedMalus;
		playersKills [killerId] -= killerBonus;
	}

	public int GetPlayerKills (int playerIndex)
	{
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
		spawnpointIndex = Random.Range (0, playersRespawns.Length);
		player.transform.position = playersRespawns [spawnpointIndex].position;
		SetMeshRendererEnabled (true);
	}

	private void SetMeshRendererEnabled (bool enabled)
	{
		SkinnedMeshRenderer meshRenderer;

		foreach (Transform child in transform) {
			meshRenderer = child.gameObject.GetComponent<SkinnedMeshRenderer> ();
			if (meshRenderer != null) {
				meshRenderer.enabled = enabled;
			}
		}
	}

	/// <summary>
	/// Spawn of the PostMan
	/// </summary>
	void PostManSpawn ()
	{
		GameObject postMan;
	
		postMan = Instantiate (postManPrefab) as GameObject;
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

		if (roundTimer <= 0) {
			SceneManager.LoadScene ("Menu");
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

	public GameObject[] GetPlayers(){
		return players;
	}
}
