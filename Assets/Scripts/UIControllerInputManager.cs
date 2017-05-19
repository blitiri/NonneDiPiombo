using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UIControllerInputManager : MonoBehaviour
{
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

    private static string startButtonTag = "StartButton";
    /// <summary>
    /// all the players.
    /// </summary>
    public static Player[] players;
    public UIButton[] buttons;
    public static UIButton startButton;

    //private void Awake()
    //{
    //    uiCamera = FindObjectOfType<UICamera>();
    //}

    private void Awake()
    {
        if (SceneController.IsMenuScene() || SceneController.IsCharacterSelectionScene() || SceneController.IsLevelSelectionScene() || SceneController.IsEndingScene())
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
        }
    }

    private void Start()
    {
        if (playerID == 0)
        {
            //if (ReInput)
            //    Debug.Log("SSSS");
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

    //void AssignPlayersAndAxises()
    //{
    //    numberOfPlayers = Configuration.instance.GetNumberOfPlayers();
    //    axises = new float[Configuration.instance.GetNumberOfPlayers()];
    //    players = new Player[numberOfPlayers];
    //    for (int i = 0; i < numberOfPlayers; i++)
    //    {
    //        players[i] = ReInput.players.GetPlayer(i);
    //    }
    //}

    //void UpdateAxis()
    //{
    //    for(int i = 0; i < axises.Length; i++)
    //    {
    //        axises[i] = players[i].GetAxis("Horizontal");
    //        uicamera.horizontalAxisName = axises[0];
    //    }
    //}

    void AssignPlayers()
    {
        numberOfPlayers = Configuration.instance.GetNumberOfPlayers();
        //Debug.Log(numberOfPlayers);
        players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
    }

    //void TakeDefaultColor()
    //{
    //    for (int i = 0; i < buttons.Length; i++)
    //    {
    //        buttons[i].defaultColor = buttons[i].;
    //    }
    //}

    void UpdateSelectedButtons()
    {
        buttons[buttonIndex].SetState(UIButtonColor.State.Normal, true);
        //Debug.Log(players.Length);
        //Debug.Log(players[playerID].GetAxis("Move horizontal"));
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
        buttons[buttonIndex].SetState(UIButtonColor.State.Hover, true);
        //Debug.Log(buttonIndex);
    }

    void PressSelectedButton()
    {
        //Debug.Log(players[playerID].GetButtonDown("Cross"));
        if (players[playerID].GetButtonDown("Cross"))
        {
            //Debug.Log("AndreaInculaMichele");
            for(int i = 0; i < buttons[buttonIndex].onClick.Count; i++)
            {
                //Debug.Log("!AndreaTrombaMichele");
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
