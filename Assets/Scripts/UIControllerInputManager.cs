using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UIControllerInputManager : MonoBehaviour
{
    /// <summary>
    /// It is needed to slowly switch between button.
    /// </summary>
    private bool canSelect;
    /// <summary>
    /// The number of players.
    /// </summary>
    public static int numberOfPlayers;
    /// <summary>
    /// Index for browsing between buttons.
    /// </summary>
    public int buttonIndex;
    /// <summary>
    /// This player's identifier.
    /// </summary>
    public int playerID;
    /// <summary>
    /// Tag needed to recognise the Start Button.
    /// </summary>
    private static string startButtonTag = "StartButton";
    /// <summary>
    /// all the players.
    /// </summary>
    public static Player[] players;
    public UIButton[] buttons;
    public UIPlayTween[] playTweens;
    public static UIButton startButton;

    private void Awake()
    {
		if (SceneController.IsMenuScene () || SceneController.IsCharacterSelectionScene () || SceneController.IsLevelSelectionScene ())
		{
			if (playerID == 0)
			{
				bool canAssignStart = true;
				foreach (UIButton button in buttons)
				{
					if (button.tag == startButtonTag)
						canAssignStart = false;
				}
				if (canAssignStart)
					startButton = GameObject.FindGameObjectWithTag (startButtonTag).GetComponent<UIButton> ();
			}
		}
        playTweens = new UIPlayTween[buttons.Length];
        for (int i = 0; i < playTweens.Length; i++)
        {
            playTweens[i] = buttons[i].gameObject.GetComponent<UIPlayTween>();
        }
        canSelect = true;
    }

    private void Start()
    {
        if (playerID == 0)
        {
            AssignPlayers();
        }

        if (buttons[buttonIndex].state != UIButtonColor.State.Hover)
            buttons[buttonIndex].SetState(UIButtonColor.State.Hover, true);
    }

    private void Update()
    {
        UpdateSelectedButtons();
        PressSelectedButton();
        if (playerID == 0)
            //Debug.Log("MINCHIA");
            PressStartButton();
    }

    void AssignPlayers()
    {
        numberOfPlayers = Configuration.instance.GetNumberOfPlayers();
        players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
    }

    void UpdateSelectedButtons()
    {
        if (canSelect)
        {
            if (players[playerID].GetAxis("Move horizontal") < -0.2f || players[playerID].GetAxis("Move vertical") > 0.2f)
            {
                if (buttons[buttonIndex].state != UIButtonColor.State.Normal)
                    buttons[buttonIndex].SetState(UIButtonColor.State.Normal, true);

                //playTweens[buttonIndex].playDirection = AnimationOrTween.Direction.Reverse;

                //Debug.Log("SSSSSS");
                if (buttonIndex <= 0)
                {
                    buttonIndex = buttons.Length - 1;
                }
                else
                {
                    buttonIndex -= 1;
                }

                if (buttons[buttonIndex].state != UIButtonColor.State.Hover)
                    buttons[buttonIndex].SetState(UIButtonColor.State.Hover, true);

                //playTweens[buttonIndex].Play(true);
            }
            else if (players[playerID].GetAxis("Move horizontal") > 0.2f || players[playerID].GetAxis("Move vertical") < -0.2f)
            {
                if (buttons[buttonIndex].state != UIButtonColor.State.Normal)
                    buttons[buttonIndex].SetState(UIButtonColor.State.Normal, true);

                if (buttonIndex >= buttons.Length - 1)
                {
                    buttonIndex = 0;
                }
                else
                {
                    buttonIndex += 1;
                }

                if (buttons[buttonIndex].state != UIButtonColor.State.Hover)
                    buttons[buttonIndex].SetState(UIButtonColor.State.Hover, true);
            }
            canSelect = false;
        }

        if (!canSelect)
        {
            if(players[playerID].GetAxis("Move horizontal") >= -0.2f && players[playerID].GetAxis("Move horizontal") <= 0.2f)
            {
                if (players[playerID].GetAxis("Move vertical") >= -0.2f && players[playerID].GetAxis("Move vertical") <= 0.2f)
                {
                    canSelect = true;
                }
            }
        }
    }

    void PressSelectedButton()
    {
        if (players[playerID].GetButtonDown("Cross"))
        {
            for(int i = 0; i < buttons[buttonIndex].onClick.Count; i++)
            {
                buttons[buttonIndex].SetState(UIButtonColor.State.Hover, true);
                buttons[buttonIndex].onClick[i].Execute();
            }
        }
    }

    void PressStartButton()
    {
        if (players[playerID].GetButtonDown("Start"))
        {
            if (startButton != null)
            {
                for (int i = 0; i < startButton.onClick.Count; i++)
                {
                    startButton.SetState(UIButtonColor.State.Pressed, true);
                    startButton.onClick[i].Execute();
                }
            }
            else
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.CheckGamePause();
                }
            }
        }
    }
}
