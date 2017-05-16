using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UIControllerInputManager : MonoBehaviour
{
    public int index;
    /// <summary>
    /// The number of players.
    /// </summary>
    public static int numberOfPlayers;
    /// <summary>
    /// index for browsing between buttons.
    /// </summary>
    private int buttonIndex;
    ///// <summary>
    ///// Affected buttons.
    ///// </summary>
    //private IList<UIButton> buttons;
    /// <summary>
    /// This player.
    /// </summary>
    public static Player[] players;
    //private float[] axises;
    //public UICamera uiCamera;

    public int playerID;
    public int buttonsIndex;
    public UIButton[] buttons;

    //private void Awake()
    //{
    //    uiCamera = FindObjectOfType<UICamera>();
    //}

    private void Start()
    {
        AssignPlayers();
    }

    private void Update()
    {
        UpdateSelectedButtons();
        PressSelectedButton();
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
        players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
    }

    void UpdateSelectedButtons()
    {
        if (players[playerID].GetAxis("Horizontal") < -0.5f)
        {
            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
            }
            else
            {
                buttonIndex -= 1;
            }
        }
        else if (players[playerID].GetAxis("Horizontal") > 0.5f)
        {
            if (buttonIndex > buttons.Length - 1)
            {
                buttonIndex = 0;
            }
            else
            {
                buttonIndex += 1;
            }
        }
        Debug.Log(buttonIndex);
        UICamera.selectedObject = buttons[buttonIndex].gameObject;
    }

    void PressSelectedButton()
    {
        if (players[playerID].GetButtonDown("Cross"))
        {
            for(int i = 0; i < buttons[buttonIndex].onClick.Count - 1; i++)
            {
                buttons[buttonIndex].onClick[i].Execute();
            }
        }
    }
}
