using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Rewired;

public class CharacterSelectorManager : MonoBehaviour
{
    /// <summary>
    /// They are needed to slowly switch between button.
    /// </summary>
    private bool[] canSelect;

    private bool[] hasJoined;
    /// <summary>
    /// the maximum number of possible players.
    /// </summary>
    public const int maxNumberOfPlayers = 4;
    /// <summary>
    /// The true number of players.
    /// </summary>
    private int numberOfPlayers;
    /// <summary>
    /// An instance of SceneController class.
    /// </summary>
    private SceneController mySceneContr;
    /// <summary>
    /// The indexes needed to switch between grannies. One for each player.
    /// </summary>
    public int[] indexes;

    int?[] orderID;
    //public int[] ids;
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
    /// All the selectors.
    /// </summary>
    public GameObject[] selectors;
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

    private IList<Player> playersInOrder = new List<Player>();
    /// <summary>
    /// The instance of ChacaterSelectorManager.
    /// </summary>
    public static CharacterSelectorManager instance;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        mySceneContr = new SceneController();
        ReInput.ControllerConnectedEvent += OnControllerConnectedEvent;
    }

    private void Start()
    {
        //numberOfPlayers = Configuration.instance.GetNumberOfPlayers();
        //Debug.Log(numberOfPlayers);
        RefreshNumberOfPlayers();
        SetVariables();
        //ids = new int[numberOfPlayers];
        //ids = ReInput.players.GetPlayerIds();
        SelectorsActivation();
    }

    private void Update()
    {
        MoveControllerAxis();
        PressControllerButton();
        StartMatch();
    }

    private void OnControllerConnectedEvent(ControllerStatusChangedEventArgs args)
    {
        RefreshNumberOfPlayers();
        SelectorsActivation();
    }

    private void SetVariables()
    {
        indexes = new int[maxNumberOfPlayers];
        hasJoined = new bool[maxNumberOfPlayers];
        orderID = new int?[maxNumberOfPlayers];
        for (int i = 0; i < orderID.Length; i++)
        {
            orderID[i] = null;
        }
        selectedGrannies = new GameObject[maxNumberOfPlayers];
        canSelect = new bool[maxNumberOfPlayers];
    }

    private void RefreshNumberOfPlayers()
    {
        numberOfPlayers = ReInput.controllers.joystickCount;
        players = new Player[maxNumberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
        //Configuration.instance.SetNumberOfPlayers(numberOfPlayers);
    }

    //private void OnControllerConnectedEvent(ControllerStatusChangedEventArgs args)
    //{
    //    numberOfPlayers++;
    //}

    private void SelectorsActivation()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            selectors[i].SetActive(true);
        }
        for (int i = numberOfPlayers; i < maxNumberOfPlayers; i++)
        {
            selectors[i].SetActive(false);
        }
    }

    public void GrannieCreation()
    {
        GameManager.instance.players = selectedGrannies;
        Destroy(gameObject);
    }

    public void ClickArrow(GameObject button)
    {
        string tag = button.tag;
        int id = int.Parse(button.transform.parent.tag);
        //Debug.Log(id);
        switch (tag)
        {
            case "Left":
                if (indexes[id] > 0)
                {
                    indexes[id]--;
                }
                else
                {
                    indexes[id] = grannies.Length - 1;
                }
                break;
            case "Right":
                if (indexes[id] < grannies.Length - 1)
                {
                    indexes[id]++;
                }
                else
                {
                    indexes[id] = 0;
                }
                break;
        }
        //Debug.Log(indexes[id]);
        centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
    }

    public void MoveControllerAxis()
    {
        //string tag = button.tag;
        //int id = int.Parse(button.transform.parent.tag);
        for (int id = 0; id < numberOfPlayers; id++)
        {
            if (hasJoined[id] && canSelect[id])
            {
                if (players[id].GetAxis("Move horizontal") < -0.2f)
                {
                    if (indexes[id] > 0)
                    {
                        indexes[id]--;
                    }
                    else
                    {
                        indexes[id] = grannies.Length - 1;
                    }
                }
                else if (players[id].GetAxis("Move horizontal") > 0.2f)
                {
                    if (indexes[id] < grannies.Length - 1)
                    {
                        indexes[id]++;
                    }
                    else
                    {
                        indexes[id] = 0;
                    }
                }
                canSelect[id] = false;
                //Debug.Log(indexes[id]);
                int orderIDint = (int)orderID[id];
                centrals[orderIDint].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
                playersLabels[orderIDint].text = granniesNames[indexes[id]];
                //selectedGrannies[]
            }

            if (!canSelect[id])
            {
                if (players[id].GetAxis("Move horizontal") >= -0.2f && players[id].GetAxis("Move horizontal") <= 0.2f)
                {
                    if (players[id].GetAxis("Move vertical") >= -0.2f && players[id].GetAxis("Move vertical") <= 0.2f)
                    {
                        canSelect[id] = true;
                    }
                }
            }
        }
    }

    void PressControllerButton()
    {
        for (int id = 0; id < maxNumberOfPlayers; id++)
        {
            if (players[id] != null)
            {
                if (players[id].GetButtonDown("Cross"))
                {
                    Debug.Log(hasJoined[id]);
                    if (hasJoined[id])
                    {
                        int orderIDint = (int)orderID[id];
                        centrals[orderIDint].SetState(UIButtonColor.State.Pressed, true);
                        selectedGrannies[id] = grannies[indexes[id]];
                        centrals[orderIDint].SetState(UIButtonColor.State.Normal, false);
                    }
                    else
                    {

                        Debug.Log("PASS2");
                        //numberOfPlayers++;
                        playersInOrder.Add(players[id]);
                        if (orderID[id] == null)
                        {
                            for (int i = 0; i < playersInOrder.Count; i++)
                            {
                                if (players[id] == playersInOrder[i])
                                {
                                    orderID[id] = i;
                                }
                            }
                        }
                        //numberOfPlayers++;
                        //RefreshNumberOfPlayers();
                        hasJoined[id] = true;
                    }
                }
            }
        }
    }

    public void ClickCentral(GameObject button)
    {
        int id = int.Parse(button.transform.parent.tag);
        //if (indexes[id] < grannies.Length - 1)
        //{
        //    indexes[id]++;
        //}
        //else
        //{
        //    indexes[id] = 0;
        //}
        //centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
        selectedGrannies[id] = grannies[indexes[id]];
    }

    //public void UndoSelection(GameObject button)
    //{
    //    int id = controll
    //}

    void StartMatch()
    {
        foreach (Player player in players)
        {
            if (player != null)
            {
                if (player.GetButtonDown("Start"))
                {
                    for (int i = 0; i < startButton.onClick.Count; i++)
                    {
                        startButton.SetState(UIButtonColor.State.Pressed, true);
                        startButton.onClick[i].Execute();
                    }
                }
            }
        }
    }

    public void LoadLevelScene()
    {
        if (Configuration.instance)
        {
            Configuration.instance.selectedGrannies = selectedGrannies;
        }
        mySceneContr.LoadSceneByName(nextSceneToLoadName);
    }
}