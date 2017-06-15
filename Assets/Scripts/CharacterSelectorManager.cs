using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Rewired;

public class CharacterSelectorManager : MonoBehaviour
{
	/// <summary>
	/// It is needed to know whether countdown is running or not.
	/// </summary>
	private bool RunCountdownIsRunning;
	/// <summary>
	/// They are needed to slowly switch between button.
	/// </summary>
	private bool[] canSelect;
	/// <summary>
	/// An instance of SceneController class.
	/// </summary>
	private SceneController mySceneContr;
	/// <summary>
	/// The from color of selectors' tweens.
	/// </summary>
	private Color frameStartColor = Color.white;
	/// <summary>
	/// Number of ready players.
	/// </summary>
	public int readyCount;
	/// <summary>
	/// Seconds before the match starts.
	/// </summary>
	private const float countdownSeconds = 3;
	/// <summary>
	/// The count of seconds before the match starts.
	/// </summary>
	public float countdownCount;
	/// <summary>
	/// The countdown gameObject;
	/// </summary>
	public UILabel countdown;
	/// <summary>
	/// Defines if a player is redy or not.
	/// </summary>
	public bool[] readys;
	/// <summary>
	/// The indexes needed to switch between grannies. One for each player.
	/// </summary>
	public int[] indexes;
	/// <summary>
	/// Name of the net scene.
	/// </summary>
	public string nextSceneToLoadName = "LevelPostOffice";
	/// <summary>
	/// All the grannies names in the atlas.
	/// </summary>
	public string[] iconAtlasNames;
	/// <summary>
	/// All the grannies names.
	/// </summary>
	public string[] granniesNames;
	/// <summary>
	/// All the grannies names.
	/// </summary>
	public UILabel[] playersLabels;
	/// <summary>
	/// Tweens of all selectors.
	/// </summary>
	public TweenColor[] tweens;
	/// <summary>
	/// All the selectors.
	/// </summary>
	public GameObject[] selectors;
    /// <summary>
    /// Sprites of selectors.
    /// </summary>
    public UISprite[] selectorSprites;
    /// <summary>
    /// Selector's color when granny is clicked.
    /// </summary>
    public Color[] frameColors;
	/// <summary>
	/// All frames.
	/// </summary>
	public UISprite[] frames;
    /// <summary>
    /// The player's number.
    /// </summary>
    public UILabel[] playerNumbers;
    /// <summary>
    /// Arrows of right and left buttons. Two for each selector. First rights and then lefts.
    /// </summary>
    public UILabel[] buttonArrows;
	/// <summary>
	/// All playable grannies.
	/// </summary>
	public GameObject[] grannies;
	/// <summary>
	/// Grannies selected by each player.
	/// </summary>
	public GameObject[] selectedGrannies;
	/// <summary>
	/// Start button.
	/// </summary>
	public UIButton startButton;
	/// <summary>
	/// An icon for each selector.
	/// </summary>
	public UIButton[] centrals;
	/// <summary>
	/// All the players.
	/// </summary>
	public Player[] players;
	/// <summary>
	/// The instance of ChacaterSelectorManager.
	/// </summary>
	public static CharacterSelectorManager instance;

	private void Awake ()
	{
		instance = this;
		//DontDestroyOnLoad(gameObject);
		mySceneContr = new SceneController ();
	}

	private void Start ()
	{
		int numberOfPlayers;

		numberOfPlayers = Configuration.instance.GetNumberOfPlayers ();
		Debug.Log (numberOfPlayers);
		indexes = new int[numberOfPlayers];
		readys = new bool[numberOfPlayers];
		//ids = new int[numberOfPlayers];
		//ids = ReInput.players.GetPlayerIds();
		selectedGrannies = new GameObject[numberOfPlayers];
		SelectorsActivation ();
		players = new Player[numberOfPlayers];
		canSelect = new bool[numberOfPlayers];
        frameColors = new Color[selectors.Length];
		for (int i = 0; i < numberOfPlayers; i++) {
			players [i] = ReInput.players.GetPlayer (i);
		}
		for (int id = 0; id < numberOfPlayers; id++)
        {
            Color playerColor = Configuration.instance.playersColors[id];
            float h;
            float s;
            float v;

            frames [id].color = frameStartColor;
			tweens [id].from = frameStartColor;
			tweens [id].to = playerColor;
            playersLabels[id].color = playerColor;
            playerNumbers[id].color = playerColor;
            buttonArrows[id].color = playerColor;
            buttonArrows[id + buttonArrows.Length/2].color = playerColor;
            Color.RGBToHSV(playerColor, out h, out s, out v);
            frameColors[id] = Color.HSVToRGB(h, s / 3, v);
        }
	}

	private void Update ()
	{
		MoveControllerAxis ();
		PressControllerButton ();
		StartMatch ();
	}

	private void SelectorsActivation ()
	{
		int numberOfPlayers;

		numberOfPlayers = Configuration.instance.GetNumberOfPlayers ();
		for (int i = 0; i < numberOfPlayers; i++) {
			selectors [i].SetActive (true);
		}
		for (int i = numberOfPlayers; i < Configuration.maxNumberOfPlayers; i++) {
			selectors [i].SetActive (false);
		}
	}

	public void GrannieCreation ()
	{
		GameManager.instance.players = selectedGrannies;
		Destroy (gameObject);
	}

	public void ClickArrow (GameObject button)
	{
		string tag = button.tag;
		int id = int.Parse (button.transform.parent.tag);
		if (!readys [id]) {
			//Debug.Log(id);
			switch (tag) {
			case "Left":
				if (indexes [id] > 0) {
					indexes [id]--;
				} else {
					indexes [id] = grannies.Length - 1;
				}
				break;
			case "Right":
				if (indexes [id] < grannies.Length - 1) {
					indexes [id]++;
				} else {
					indexes [id] = 0;
				}
				break;
			}
			//Debug.Log(indexes[id]);
			centrals [id].normalSprite = iconAtlasNames [indexes [id]] + "PlayerIcon";
			selectedGrannies [id] = grannies [indexes [id]];
		}
	}

	public void MoveControllerAxis ()
	{
		//string tag = button.tag;
		//int id = int.Parse(button.transform.parent.tag);
		for (int id = 0; id < Configuration.instance.GetNumberOfPlayers (); id++) {
			if (!readys [id] && canSelect [id]) {
				if (players [id].GetAxis ("Move horizontal") < -0.2f || players [id].GetAxis ("Move horizontal") > 0.2f) {
					if (players [id].GetAxis ("Move horizontal") < -0.2f) {
						if (indexes [id] > 0) {
							indexes [id]--;
						} else {
							indexes [id] = grannies.Length - 1;
						}
					} else if (players [id].GetAxis ("Move horizontal") > 0.2f) {
						if (indexes [id] < grannies.Length - 1) {
							indexes [id]++;
						} else {
							indexes [id] = 0;
						}
					}
					selectedGrannies [id] = grannies [indexes [id]];
				}
				canSelect [id] = false;
				//Debug.Log(indexes[id]);
				centrals [id].normalSprite = iconAtlasNames [indexes [id]] + "PlayerIcon";
				playersLabels [id].text = granniesNames [indexes [id]];
			}

			if (!canSelect [id]) {
				if (players [id].GetAxis ("Move horizontal") >= -0.2f && players [id].GetAxis ("Move horizontal") <= 0.2f) {
					if (players [id].GetAxis ("Move vertical") >= -0.2f && players [id].GetAxis ("Move vertical") <= 0.2f) {
						canSelect [id] = true;
					}
				}
			}
		}
	}

	void PressControllerButton ()
	{
		for (int id = 0; id < Configuration.instance.GetNumberOfPlayers (); id++) {
			if (players [id].GetButtonDown ("Cross")) {
				//readyButtons[id].SetState(UIButtonColor.State.Pressed, true);
				//for (int i = 0; i < readyButtons[id].onClick.Count; i++)
				//{
				//    readyButtons[id].onClick[i].Execute();
				//}
				//readyButtons[id].SetState(UIButtonColor.State.Normal, false);
				//tweens[id].StopCoroutine(tweens[id].name);
				//tweens[id].from = frames[id].color;
				//if (!readys[id])
				//{
				//    tweens[id].PlayForward();
				//    readys[id] = true;
				//}
				//else
				//{
				//    tweens[id].PlayReverse();
				//    readys[id] = false;
				//}
				//tweens[id].from = tweenFromColor;
				//tweens[id].ResetToBeginning();
				Tweener (frameStartColor, Configuration.instance.playersColors [id], id);
				CheckReady ();
			}
		}
	}

	private void CheckReady ()
	{
		if (readyCount == Configuration.instance.GetNumberOfPlayers ()) {
			StartCountdown ();
		} else {
			StopCountdown ();
		}
	}

	/// <summary>
	/// It sets ready players as well.
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <param name="id"></param>
	public void Tweener (Color from, Color to, int id)
	{
		if (!readys [id]) {
			tweens [id].SetStartToCurrentValue ();
			tweens [id].to = to;
			tweens [id].PlayForward ();
			readys [id] = true;
			readyCount++;
		} else {
			tweens [id].from = from;
			tweens [id].SetEndToCurrentValue ();
			tweens [id].PlayReverse ();
			readys [id] = false;
			readyCount--;
		}
		tweens [id].ResetToBeginning ();
	}

	public void StartCountdown ()
	{
		if (countdownCount != countdownSeconds) {
			countdownCount = countdownSeconds;
		}
		if (!RunCountdownIsRunning) {
			StartCoroutine ("RunCountdown");
			RunCountdownIsRunning = true;
		}
		if (!countdown.gameObject.activeInHierarchy) {
			countdown.gameObject.SetActive (true);
		}
	}

	public IEnumerator RunCountdown ()
	{
		Debug.Log (countdownCount);
		// +1 to leave last number a while to screen
		while (countdownCount + 1 > 0) {
			countdownCount -= Time.deltaTime;
			if (countdownCount > 0) {
				countdown.text = countdownCount.ToString ("#");
			}
			yield return null;
		}
		LoadLevelScene (nextSceneToLoadName);
	}

	public void StopCountdown ()
	{
		if (RunCountdownIsRunning) {
			StopCoroutine ("RunCountdown");
			RunCountdownIsRunning = false;
		}
		if (countdown.gameObject.activeInHierarchy) {
			countdown.gameObject.SetActive (false);
		}
		if (countdownCount != countdownSeconds) {
			countdownCount = countdownSeconds;
		}
	}

	public void ClickCentral (GameObject button)
	{
		int id = int.Parse (button.transform.parent.tag);
		//if (indexes[id] < grannies.Length - 1)
		//{
		//    indexes[id]++;
		//}
		//else
		//{
		//    indexes[id] = 0;
		//}
		//centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
		selectedGrannies [id] = grannies [indexes [id]];
		Tweener (frameStartColor, frameColors[id], id);
		CheckReady ();
	}

	//public void UndoSelection(GameObject button)
	//{
	//    int id = controll
	//}

	//public void ClickReady(GameObject button)
	//{
	//    int id = int.Parse(button.transform.parent.tag);
	//    float frameLerpSpeed = 0.5f;
	//    float frameLerpFringe = 0.5f;
	//    if (!readys[id])
	//    {
	//        StopCoroutine(LerpFrameColor(frames[id].color, Configuration.instance.playersColors[id], frameLerpSpeed, frameLerpFringe));
	//        StartCoroutine(LerpFrameColor(frames[id].color, C, frameLerpSpeed, frameLerpFringe));
	//        readys[id] = true;
	//    }
	//    else
	//    {
	//        StopCoroutine(LerpFrameColor(frames[id].color, Configuration.instance.playersColors[id], frameLerpSpeed, frameLerpFringe));
	//        frames[id].color = Color.white;
	//        readys[id] = false;
	//    }
	//}

	//IEnumerator LerpFrameColor(Color a, Color b, float time, float fringe)
	//{
	//    float aRGB = a.r + a.g + a.b;
	//    float bRGB = b.r + b.g + b.b;
	//    if (aRGB > bRGB)
	//    {
	//        while (aRGB > bRGB + fringe)
	//        {
	//            Debug.Log("SSSSSSSSSSS");
	//            a = Color.Lerp(a, b, time);
	//            aRGB = a.r + a.g + a.b;
	//            yield return null;
	//        }
	//    }
	//    else if (aRGB < bRGB)
	//    {
	//        while (aRGB < bRGB - fringe)
	//        {
	//            a = Color.Lerp(a, b, time);
	//            aRGB = a.r + a.g + a.b;
	//            yield return null;
	//        }
	//    }
	//    else if (aRGB == bRGB)
	//    {
	//        yield break;
	//    }
	//    a = b;
	//}

	void StartMatch ()
	{
		foreach (Player player in players) {
			if (player.GetButtonDown ("Start")) {
				startButton.SetState (UIButtonColor.State.Pressed, true);
				for (int i = 0; i < startButton.onClick.Count; i++) {
					startButton.onClick [i].Execute ();
				}
			}
		}
	}

	public void LoadLevelScene (string scene)
	{
		if (Configuration.instance) {
			Configuration.instance.selectedGrannies = selectedGrannies;
		}
		mySceneContr.LoadSceneByName (scene);
	}
}