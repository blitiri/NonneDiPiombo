using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;
	//array di transform per il respawn dei player
	public Transform[] respawnPlayer;
	public GameObject[] players;
	private PlayerControl[] playersControls;
	public GameObject pauseScreen;
	public GameObject pauseButton;
	public GameObject quitButton;
    public float maxTimerBeforeRespawn = 1.0f;
    public SkinnedMeshRenderer[] mesh;
    public int player1Kills;	
	public int player2Kills;
	public bool isPaused = false;
	public float roundTimer;

	void Awake ()
	{
		int playerIndex;

		instance = this;
		playersControls = new PlayerControl[players.Length];
		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			playersControls [playerIndex] = players [playerIndex].GetComponent<PlayerControl> ();
			playersControls [playerIndex].playerId = playerIndex + 1;
		}
	}

	// Update is called once per frame
	void Update ()
	{
        CheckRespawnPlayers();
        Pause ();
		TimerSetUp ();
		UIManager.instance.SetScore ();

	}

    public void CheckRespawnPlayers()
    {

        int playerIndex;

        for (playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {

            if ((playersControls[playerIndex].GetLife() <= 0) && !playersControls[playerIndex].IsUnderAttack() || (playersControls[playerIndex].GetStress() >= 100))
            {
                StartCoroutine(RespawnPlayer(playerIndex));
                /*player.GetComponentInChildren<SkinnedMeshRenderer> (false);
                spawnpointIndex = Random.Range (0, respawnPlayer.Length);
                player.transform.position = respawnPlayer [spawnpointIndex].position;
                playersControls [playerIndex].ResetStatus ();
                player.GetComponentInChildren<SkinnedMeshRenderer> (true);*/
                if (playerIndex == 0)
                {
                    player2Kills += 1;

                }

                if (playerIndex == 1)
                {
                    player1Kills += 1;
                }

            }
        }
    }

    IEnumerator RespawnPlayer(int playerIndex)
    {
        int spawnpointIndex;
        GameObject player;



        playersControls[playerIndex].ResetStatus();
        player = players[playerIndex];

        for (int i = 0; i < 2; i++)
        {
            player.transform.GetChild(i).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        yield return new WaitForSeconds(maxTimerBeforeRespawn);
        spawnpointIndex = Random.Range(0, respawnPlayer.Length);
        player.transform.position = respawnPlayer[spawnpointIndex].position;
        //playersControls [playerIndex].ResetStatus ();
        for (int i = 0; i < 2; i++)
        {

            player.transform.GetChild(i).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;

        }
    }



	
	
	/// <summary>
	//Countdown for round duration, back to menu at the end
	/// </summary>
	void TimerSetUp(){
		roundTimer -= Time.deltaTime;
		int minutes = Mathf.FloorToInt (roundTimer / 60f);
		int seconds = Mathf.FloorToInt (roundTimer - minutes * 60);

		string realTime = string.Format ("{0:0}:{1:00}", minutes, seconds);
		UIManager.timerLabel.text = realTime;

		if (seconds == 0) {
			Application.LoadLevel ("Menu");
		}
	}

	void Pause(){
		/// <summary>
		//Pause is on/off
		/// </summary>
		if (isPaused == true) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isPaused = false;
				Destroy (pauseScreen.GetComponent<TweenAlpha> ());
				pauseScreen.GetComponent<UISprite> ().enabled = false;
				pauseButton.GetComponent<BoxCollider> ().enabled = false;
				pauseButton.GetComponent<UISprite> ().enabled = false;
				pauseButton.GetComponentInChildren<UILabel> ().enabled = false;
				quitButton.GetComponent<BoxCollider> ().enabled = false;
				quitButton.GetComponent<UISprite> ().enabled = false;
				quitButton.GetComponentInChildren<UILabel> ().enabled = false;
			}

			Time.timeScale = 0;
		} else if (isPaused == false) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isPaused = true;
				AddTween ();
				pauseScreen.GetComponent<UISprite> ().enabled = true;
				pauseButton.GetComponent<BoxCollider> ().enabled = true;
				pauseButton.GetComponent<UISprite> ().enabled = true;
				pauseButton.GetComponentInChildren<UILabel> ().enabled = true;
				quitButton.GetComponent<BoxCollider> ().enabled = true;
				quitButton.GetComponent<UISprite> ().enabled = true;
				quitButton.GetComponentInChildren<UILabel> ().enabled = true;
			}
			
			Time.timeScale = 1;
		}
	}
	/// <summary>
	//Adding tween to screen transition
	/// </summary>
	void AddTween(){
		pauseScreen.AddComponent<TweenAlpha> ().from = 0;
		pauseScreen.GetComponent<TweenAlpha> ().to = 1;
		pauseScreen.GetComponent<TweenAlpha> ().duration = 1.7f;
	}
	/// <summary>
	//Clicking on Pause button
	/// </summary>
	public void OnClickPauseButton(){
		isPaused = false;
		Destroy (pauseScreen.GetComponent<TweenAlpha> ());
		pauseScreen.GetComponent<UISprite> ().enabled = false;
		pauseButton.GetComponent<BoxCollider> ().enabled = false;
		pauseButton.GetComponent<UISprite> ().enabled = false;
		pauseButton.GetComponentInChildren<UILabel> ().enabled = false;
		quitButton.GetComponent<BoxCollider> ().enabled = false;
		quitButton.GetComponent<UISprite> ().enabled = false;
		quitButton.GetComponentInChildren<UILabel> ().enabled = false;
	}

	/// <summary>
	//Clicking on Quit button
	/// </summary>
	public void OnClickQuitButton(){
		Application.Quit ();
	}
}
