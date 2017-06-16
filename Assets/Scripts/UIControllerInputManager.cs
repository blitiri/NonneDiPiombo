using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UIControllerInputManager : MonoBehaviour
{
    /// <summary>
    /// The instance of this class. It is needed to check whether the gameobject containing this component has "Main" or "Secondary" tag.
    /// </summary>
    private static UIControllerInputManager instance;
    /// <summary>
    /// The tag of the current active UIControllerInputManager.
    /// </summary>
    public static string currentTag;
    /// <summary>
    /// The tag for Main UI Controller Input Manager containing gameobject.
    /// </summary>
    private const string mainTag = "Main";
    /// <summary>
    /// The tag for Secondary UI Controller Input Manager containing gameobject.
    /// </summary>
    private const string secondaryTag = "Secondary";
    /// <summary>
    /// Component attached to the first menu in the current scene.
    /// </summary>
    private static UIControllerInputManager main;
	/// <summary>
	/// It is needed to slowly switch between buttons.
	/// </summary>
	private bool canSelect;
	/// <summary>
	/// Index for browsing between buttons.
	/// </summary>
	private int selectedButtonIndex;
	/// <summary>
	/// This player's identifier.
	/// </summary>
	public int playerID;
	/// <summary>
	/// Tag needed to recognise the Start Button.
	/// </summary>
	private const string startButtonTag = "StartButton";
	/// <summary>
	/// all the players.
	/// </summary>
	private Player[] players;
	public UIButton[] buttons;
	private UIPlayTween[] playTweens;
    //private UIButton startButton;

    private void OnEnable()
    {
        //Debug.Log(1);
        instance = this;
    }

    private void OnDisable()
    {
        //Debug.Log(2);
        instance = main;
    }

    private void Awake ()
	{
        //GameObject startButtonGameObject;

        //startButtonGameObject = GameObject.FindGameObjectWithTag (startButtonTag);
        //if (startButtonGameObject) {
        //	startButton = startButtonGameObject.GetComponent<UIButton> ();
        //}
        playTweens = new UIPlayTween[buttons.Length];
		for (int i = 0; i < playTweens.Length; i++)
        {
			playTweens [i] = buttons [i].gameObject.GetComponent<UIPlayTween> ();
		}
		canSelect = true;
	}

	private void Start ()
	{
        //Debug.Log(3);

        if (gameObject.tag == mainTag)
        {
            instance = this;
            main = this;
        }
        if (playerID == 0) {
			AssignPlayers ();
		}

		if (buttons.Length > 0)
        {
			if (buttons [selectedButtonIndex].state != UIButtonColor.State.Hover)
				buttons [selectedButtonIndex].SetState (UIButtonColor.State.Hover, true);

            if (playTweens[selectedButtonIndex] != null)
                playTweens [selectedButtonIndex].Play (true);
		}
	}

	private void Update ()
	{
        if (instance == this)
        {
            UpdateSelectedButtons();
            PressSelectedButton();
            if (playerID == 0)
                //Debug.Log("MINCHIA");
                PressStartButton();
        }
	}

	void AssignPlayers ()
	{
		int numberOfPlayers = Configuration.instance.GetNumberOfPlayers ();

		players = new Player[numberOfPlayers];
		for (int i = 0; i < numberOfPlayers; i++) {
			players [i] = ReInput.players.GetPlayer (i);
		}
	}

	void UpdateSelectedButtons ()
	{
		if (buttons.Length > 0) {
			if (canSelect) {
				if (players [playerID].GetAxis ("Move horizontal") < -0.4f || players [playerID].GetAxis ("Move vertical") > 0.4f) {
					if (buttons [selectedButtonIndex].state != UIButtonColor.State.Normal)
						buttons [selectedButtonIndex].SetState (UIButtonColor.State.Normal, true);

					if (playTweens [selectedButtonIndex] != null)
						playTweens [selectedButtonIndex].Play (false);

					//playTweens[buttonIndex].playDirection = AnimationOrTween.Direction.Reverse;

					//Debug.Log("SSSSSS");
					if (selectedButtonIndex <= 0) {
						selectedButtonIndex = buttons.Length - 1;
					} else {
						selectedButtonIndex -= 1;
					}

					if (buttons [selectedButtonIndex].state != UIButtonColor.State.Hover)
						buttons [selectedButtonIndex].SetState (UIButtonColor.State.Hover, true);

					if (playTweens [selectedButtonIndex] != null)
						playTweens [selectedButtonIndex].Play (true);
				} else if (players [playerID].GetAxis ("Move horizontal") > 0.4f || players [playerID].GetAxis ("Move vertical") < -0.4f) {
					if (buttons [selectedButtonIndex].state != UIButtonColor.State.Normal)
						buttons [selectedButtonIndex].SetState (UIButtonColor.State.Normal, true);

					if (playTweens [selectedButtonIndex] != null)
						playTweens [selectedButtonIndex].Play (false);

					if (selectedButtonIndex >= buttons.Length - 1) {
						selectedButtonIndex = 0;
					} else {
						selectedButtonIndex += 1;
					}

					if (buttons [selectedButtonIndex].state != UIButtonColor.State.Hover)
						buttons [selectedButtonIndex].SetState (UIButtonColor.State.Hover, true);

					if (playTweens [selectedButtonIndex] != null)
						playTweens [selectedButtonIndex].Play (true);
				}
				canSelect = false;
			}

			if (!canSelect) {
				if (players [playerID].GetAxis ("Move horizontal") >= -0.4f && players [playerID].GetAxis ("Move horizontal") <= 0.4f) {
					if (players [playerID].GetAxis ("Move vertical") >= -0.4f && players [playerID].GetAxis ("Move vertical") <= 0.4f) {
						canSelect = true;
					}
				}
			}
		}
	}

	void PressSelectedButton ()
	{
		if (buttons.Length > 0) {
			if (players [playerID].GetButtonDown ("Cross")) {
				for (int i = 0; i < buttons [selectedButtonIndex].onClick.Count; i++) {
					buttons [selectedButtonIndex].SetState (UIButtonColor.State.Pressed, true);
					buttons [selectedButtonIndex].onClick [i].Execute ();
					//buttons[buttonIndex].SetState(UIButtonColor.State.Normal, true);
				}
			}
		}
	}

	void PressStartButton ()
	{
		//if (startButton != null)
  //      {
		if (players [playerID].GetButtonDown ("Start"))
        {
			//	for (int i = 0; i < startButton.onClick.Count; i++)
   //             {
			//		startButton.SetState (UIButtonColor.State.Pressed, true);
			//		startButton.onClick [i].Execute ();
			//		//startButton.SetState(UIButtonColor.State.Normal, true);
			//	}
			//}
   //         else
        //{
			if (GameManager.instance != null)
                {
					GameManager.instance.CheckGamePause ();
				}
	    }
		//}
	}
}
