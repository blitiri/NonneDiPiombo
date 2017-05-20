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
    private int buttonIndex;
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
    public static UIButton startButton;

    private void Awake()
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
                startButton = GameObject.FindGameObjectWithTag(startButtonTag).GetComponent<UIButton>();
        }
        canSelect = true;
    }

    private void Start()
    {
        if (playerID == 0)
        {
            AssignPlayers();
        }
    }

    private void Update()
    {
        UpdateSelectedButtons();
        PressSelectedButton();
        if (startButton != null && playerID == 0)
            StartMatch();
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
        buttons[buttonIndex].SetState(UIButtonColor.State.Normal, true);
        if (canSelect)
        {
            if (players[playerID].GetAxis("Move horizontal") < -0.2f)
            {
                Debug.Log("SSSSSS");
                if (buttonIndex <= 0)
                {
                    buttonIndex = buttons.Length - 1;
                }
                else
                {
                    buttonIndex -= 1;
                }
            }
            else if (players[playerID].GetAxis("Move horizontal") > 0.2f)
            {
                if (buttonIndex >= buttons.Length - 1)
                {
                    buttonIndex = 0;
                }
                else
                {
                    buttonIndex += 1;
                }
            }
            if (players[playerID].GetAxis("Move vertical") < -0.2f)
            {
                Debug.Log("SSSSSS");
                if (buttonIndex <= 0)
                {
                    buttonIndex = buttons.Length - 1;
                }
                else
                {
                    buttonIndex -= 1;
                }
            }
            else if (players[playerID].GetAxis("Move vertical") > 0.2f)
            {
                if (buttonIndex >= buttons.Length - 1)
                {
                    buttonIndex = 0;
                }
                else
                {
                    buttonIndex += 1;
                }
            }
            canSelect = false;
        }

        buttons[buttonIndex].SetState(UIButtonColor.State.Hover, true);

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
                buttons[buttonIndex].onClick[i].Execute();
            }
        }
    }

    void StartMatch()
    {
        if (players[playerID].GetButtonDown("Start"))
        {
            for (int i = 0; i < startButton.onClick.Count; i++)
            {
                startButton.SetState(UIButtonColor.State.Pressed, true);
                startButton.onClick[i].Execute();
            }
        }
    }
}
