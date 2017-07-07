using UnityEngine;
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
    public float countdownCount;
    public float timerToSpawn = 5.0f;
    public float maxTimerBeforeRespawn = 1.0f;
    public float roundTimer;

    public bool countdownIsRunning;

    public string readyString;

    public string fightString;

    public Transform cameraTransform;

    public UILabel countdown;

    public TweenAlpha countdownTA;

    public TweenScale countdownTS;

    public GameObject rewiredInputManagerPrefab;

    // array di transform per il respawn dei player
    public Transform[] playersRespawns;
    public Transform[] playersStartRespawns;

    public GameObject[] players;

    public GameObject[] weapons;

    public GameObject spawnVFXPrefab;

    private EventDelegate eventDelegate;

    public bool isPaused = false;
    private float timerPostManSpawn;
    private PlayerControl[] playersControls;

    void Awake()
    {
        instance = this;
        Statistics.instance.Reset();
        Instantiate(rewiredInputManagerPrefab);
        InitPlayers();
    }

    void Start()
    {
        LevelUIManager.instance.InitUI();
        TimerUpdate();
        StartCoroutine("RunCountdown");
    }

    // Update is called once per frame
    void Update()
    {
        //CheckRespawnPlayers ();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (!isPaused)
        {
            TimerUpdate();
        }
    }

    public void PauseGame()
    {
        if (!countdownIsRunning && LevelUIManager.instance.canPause)
        {
            Time.timeScale = 0.0f;
            //Debug.Log("MAMMAMIA");
            LevelUIManager.instance.canPause = false;
            InvertPause();
            LevelUIManager.instance.SetPauseMenuVisible(isPaused);
        }
    }

    /// <summary>
    /// Sets game pause.
    /// </summary>
    /// <param name="pause">If set to <c>true</c> pause.</param>
    public void SetPause(bool pause)
    {
        isPaused = pause;
    }

    public void InvertPause()
    {
        if (LevelUIManager.instance.pauseMenu.gameObject.activeInHierarchy)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }

    //public bool IsPaused()
    //{
    //    return isPaused;
    //}

    private void InitPlayers()
    {
        int playerIndex;
        int numberOfPlayers;
        int setPlayers = 0;
        bool arePlayers = false;

        numberOfPlayers = Configuration.instance.GetNumberOfPlayers();
        //Debug.Log(numberOfPlayers);
        players = new GameObject[Configuration.maxNumberOfPlayers];
        playersControls = new PlayerControl[Configuration.maxNumberOfPlayers];

        for(int i = 0; i < Configuration.instance.selectedGrannies.Count; i++)
        {
            if (Configuration.instance.selectedGrannies[i] != null)
            {
                arePlayers = true;
                break;
            }
        }

        if (arePlayers)
        {
            //Debug.Log("Cazzo");
            for (playerIndex = 0; playerIndex < Configuration.maxNumberOfPlayers; playerIndex++)
            {
                // Instantiate player
                if (setPlayers < numberOfPlayers)
                {
                    if (Configuration.instance.selectedGrannies[playerIndex] != null)
                    {
                        //Debug.Log(playerIndex);
                        players[playerIndex] = Instantiate(Configuration.instance.selectedGrannies[playerIndex]) as GameObject;
                        players[playerIndex].transform.position = playersStartRespawns[playerIndex].position;
                        playersControls[playerIndex] = players[playerIndex].GetComponent<PlayerControl>();
                        playersControls[playerIndex].SetPlayerId(playerIndex);
                        playersControls[playerIndex].SetAngleCorrection(cameraTransform.rotation.eulerAngles.y);
                        if (Configuration.instance.players.Count > 0)
                        {
                            playersControls[playerIndex].controllerId = Configuration.instance.players[playerIndex].id;
                        }
                        players[playerIndex].SetActive(true);
                        setPlayers++;
                    }
                }
            }
        }
        else
        {
            for (playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++)
            {
                //Debug.Log("ESRESRSREESRESESESRERSERSERERSSRD");
                players[playerIndex] = Instantiate(Configuration.instance.granniesPrefabs[Random.Range(0, Configuration.instance.granniesPrefabs.Length)]) as GameObject;
                players[playerIndex].transform.position = playersStartRespawns[playerIndex].position;
                playersControls[playerIndex] = players[playerIndex].GetComponent<PlayerControl>();
                playersControls[playerIndex].SetPlayerId(playerIndex);
                playersControls[playerIndex].SetAngleCorrection(cameraTransform.rotation.eulerAngles.y);
                playersControls[playerIndex].controllerId = playerIndex;
            }
        }
            // Initialize PlayerControl script
            //playersControls[playerIndex] = players[playerIndex].GetComponent<PlayerControl>();
            //playersControls[playerIndex].SetPlayerId(playerIndex);
            //playersControls[playerIndex].SetAngleCorrection(cameraTransform.rotation.eulerAngles.y);
            //if (Configuration.instance.players.Count <= 0)
            //{
            //    //while (Configuration.instance.players[playerIndex] == null)
            //    //{
            //    //    playerIndex++;
            //    //    Debug.Log("playerIndex: " + playerIndex);
            //    //}
            //    //playersControls[playerIndex].controllerId = Configuration.instance.players[playerIndex].id;
            //    playersControls[playerIndex].controllerId = playerIndex;
            //}
            //players[playerIndex].SetActive(true);
    }

    /// <summary>
    /// Determines if round ended.
    /// </summary>
    /// <returns><c>true</c> if round ended; otherwise, <c>false</c>.</returns>
    public bool IsRoundEnded()
    {
        return roundTimer <= 0;
    }

    public void CheckRespawnPlayers()
    {
        int playerIndex;

        for (playerIndex = 0; playerIndex < playersControls.Length; playerIndex++)
        {
            //Debug.Log(playerIndex);
            if (playersControls[playerIndex] != null)
            {
                if ((playersControls[playerIndex].isDead) || (playersControls[playerIndex].IsCollapsed()))
                {
                    StartCoroutine(RespawnPlayer(playerIndex));
                }
            }
        }
    }

    /// <summary>
    /// Notify a player is killed.
    /// </summary>
    /// <param name="killedId">Killed identifier.</param>
    /// <param name="killerId">Killer identifier.</param>
    public void PlayerKilled(int killedId, int killerId)
    {
        Statistics.instance.PlayerKilled(killedId, killerId);
        LevelUIManager.instance.SetScore(Statistics.instance.getPlayersKills()[killedId], killedId);
        LevelUIManager.instance.SetScore(Statistics.instance.getPlayersKills()[killerId], killerId);
    }

    private IEnumerator RespawnPlayer(int playerIndex)
    {
        int spawnpointIndex;
        GameObject player;
        ParticleSystem[] particleSystems;
        ParticleSystem.MainModule main;
        BoxCollider playerCollider;
        PlayerControl playerControl;

        player = players[playerIndex];
        SetMeshRendererEnabled(false, playerIndex);
        playerControl = player.GetComponent<PlayerControl>();
        playerControl.ResetStatus();
        playerControl.stopInputPlayer = true;
        playerCollider = player.GetComponent<BoxCollider>();
        playerCollider.enabled = false;

        spawnpointIndex = UnityEngine.Random.Range(0, playersRespawns.Length);
        GameObject spawnVFX = Instantiate(spawnVFXPrefab, playersRespawns[spawnpointIndex].position + new Vector3(0, 1, 0), spawnVFXPrefab.transform.rotation) as GameObject;
        particleSystems = spawnVFX.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particles in particleSystems)
        {
            main = particles.main;
            main.startColor = Configuration.instance.playersColors[playersControls[playerIndex].playerId];
        }
        Destroy(spawnVFX, 3f);
        player.transform.position = playersRespawns[spawnpointIndex].position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return new WaitForSeconds(maxTimerBeforeRespawn);

        SetMeshRendererEnabled(true, playerIndex);
        playerControl.stopInputPlayer = false;
        playerControl.isDead = false;
        playerCollider.enabled = true;

        //playerControl.isImmortal = true;
        yield return new WaitForSeconds(playerControl.immortalTime);
        playerControl.isImmortal = false;
    }

    private void SetMeshRendererEnabled(bool enabled, int playerIndex)
    {
        Renderer meshRenderer;
        GameObject player;

        player = players[playerIndex];
        player.GetComponent<Renderer>().enabled = enabled;

        foreach (Transform child in player.transform)
        {
            meshRenderer = child.gameObject.GetComponent<Renderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = enabled;
            }
        }
    }

    /// <summary>
    /// Update round countdown, back to menu at the end
    /// </summary>
    private void TimerUpdate()
    {
        roundTimer = Mathf.Clamp(roundTimer - Time.deltaTime, 0, float.MaxValue);
        LevelUIManager.instance.SetTimer(roundTimer);
        if (roundTimer == 0)
        {
            SceneController.instance.LoadSceneByName("Ending");
            LevelUIManager.instance.InitUI();
        }
    }

    /// <summary>
    /// Gets the players.
    /// </summary>
    /// <returns>The players.</returns>
    public GameObject[] GetPlayers()
    {
        return players;
    }

    public IEnumerator RunCountdown()
    {
        countdownIsRunning = true;

        //Debug.Log("SSSSS");
        countdown.gameObject.SetActive(true);
        isPaused = true;
        //foreach(PlayerControl player in playersControls)
        //{
        //    player.stopInputPlayer = true;
        //}
        //stopInputPlayer = true;
        float maxCountdown = countdownCount;
        // +1 to leave last number a while to screen
        while (countdownCount > 0)
        {

            if (countdownCount < maxCountdown && countdownCount > 0.5f)
            {

                if (countdown.text != readyString)
                {
                    countdown.text = readyString;
                    //countdown.alpha = countdownTA.from;
                    countdown.transform.localScale = countdownTS.from;
                    //countdownTA.PlayForward();
                    countdownTS.PlayForward();
                    //onfinishedTweens, reset countdown initial alpha and scale.
                }
            }
            if (countdownCount <= 0.5f)
            {
                if (countdown.text != fightString)
                {
                    //Debug.Log("Puttana");
                    countdown.text = fightString;
                    //countdown.alpha = countdownTA.from;
                    countdown.transform.localScale = countdownTS.from;
                    //countdownTA.ResetToBeginning();
                    countdownTS.ResetToBeginning();
                    //countdownTA.PlayForward();
                    countdownTS.PlayForward();
                    eventDelegate = new EventDelegate(this, "OnFinishedCountdownTSPlayReverse");
                    EventDelegate.Set(countdownTS.onFinished, eventDelegate);
                    //onfinishedTweens, reset countdown initial alpha and scale.
                }
            }
            countdownCount -= Time.deltaTime;
            yield return null;
        }
        //foreach (PlayerControl player in playersControls)
        //{
        //    player.stopInputPlayer = false;
        //}
        //stopInpitPlayer = false;
    }

    public void OnFinishedCountdownTSPlayReverse()
    {
        eventDelegate = new EventDelegate(this, "OnFinishedCountdownTSSetIBools");
        EventDelegate.Set(countdownTS.onFinished, eventDelegate);

        countdownTS.delay = 0.5f;
        countdownTS.from = countdown.transform.localScale;
        countdownTS.to = new Vector3(0, 0, 0);
        countdownTS.animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        countdownTS.ResetToBeginning();
        countdownTS.PlayForward();
    }

    public void OnFinishedCountdownTSSetIBools()
    {
        isPaused = false;
        countdownIsRunning = false;
    }
}
