using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// GameManager instance.
	/// </summary>
	public static GameManager instance;
	private const int maxNumberOfPlayers = 4;
	private bool isPaused = false;
	[Range (2, maxNumberOfPlayers)]
	public int numberOfPlayers = 2;
	public int killedMalus = 1;
	public int killerBonus = 2;
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
	private int[] playersKills;

	// array di transform per il respawn dei player
	public Transform[] playersRespawns;
	public Transform[] playersStartRespawns;

	public GameObject[] playersPrefabs;
	private GameObject[] players;

	private PlayerControl[] playersControls;

	void Awake ()
	{
		instance = this;
		Instantiate (rewiredInputManagerPrefab);
		InitPlayers ();
        restartButtonBoxCollider = restartButton.GetComponent<BoxCollider>();
        restartButtonUISprite = restartButton.GetComponent<UISprite>();
        restartButtonUILabel = restartButton.GetComponentInChildren<UILabel>();
	}

	void Start ()
	{
		UIManager.instance.InitUI ();
	}

	// Update is called once per frame
	void Update ()
	{
		CheckRespawnPlayers ();
		CheckGamePause ();
		TimerUpdate ();

		/*if (timerPostManSpawn < timerToSpawn) {
			timerPostManSpawn += Time.deltaTime;
            
		} else {
			PostManSpawn();
			timerPostManSpawn = 0.0f;
		}*/

	}

	private void InitPlayers ()
	{
		int playerIndex;

		players = new GameObject[numberOfPlayers];
		playersControls = new PlayerControl[numberOfPlayers];
		playersKills = new int[numberOfPlayers];
		for (playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++) {
			// Instantiate player
			players [playerIndex] = Instantiate (playersPrefabs [playerIndex % playersPrefabs.Length]) as GameObject;
			players [playerIndex].transform.position = playersStartRespawns [playerIndex].position;
			// Initialize PlayerControl script
			playersControls [playerIndex] = players [playerIndex].GetComponent<PlayerControl> ();
			playersControls [playerIndex].SetPlayerId (playerIndex);
			playersControls [playerIndex].SetAngleCorrection (cameraTransform.rotation.eulerAngles.y);
			playersKills [playerIndex] = 0;
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
			if ((playersControls [playerIndex].IsDead ()) && !playersControls [playerIndex].IsUnderAttack () || (playersControls [playerIndex].IsCollapsed ())) {
				StartCoroutine (RespawnPlayer (playerIndex));
			}
		}
	}

	public void PlayerKilled (int killedId, int killerId)
	{
		playersKills [killedId] -= killedMalus;
		playersKills [killerId] += killerBonus;
		UIManager.instance.SetScore (playersKills [killedId],killedId);
		UIManager.instance.SetScore (playersKills [killerId],killerId);
	}

	private IEnumerator RespawnPlayer (int playerIndex)
	{
		int spawnpointIndex;
		GameObject player;

		playersControls [playerIndex].ResetStatus ();
		player = players [playerIndex];
		SetMeshRendererEnabled (false, playerIndex);
		player.GetComponent<PlayerControl> ().stopInputPlayer = true;
        player.GetComponent<PlayerControl>().dead = true;
		player.GetComponent<BoxCollider> ().enabled = false;

        yield return new WaitForSeconds (maxTimerBeforeRespawn);

		spawnpointIndex = Random.Range (0, playersRespawns.Length);
		player.transform.position = playersRespawns [spawnpointIndex].position;
		SetMeshRendererEnabled (true, playerIndex);
		player.GetComponent<PlayerControl> ().stopInputPlayer = false;
        player.GetComponent<PlayerControl>().dead = false;
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
		roundTimer -= Time.deltaTime;
		UIManager.instance.SetTimer (roundTimer);
		if (roundTimer <= 0) {
			SceneManager.LoadScene ("Menu");
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

	public GameObject[] GetPlayers ()
	{
		return players;
	}
}
