using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Rewired;

public class CharacterSelectorManager : MonoBehaviour
{
    /// <summary>
    /// They are needed to slowly switch between button.
    /// </summary>
    private bool[] canSelect;
    /// <summary>
    /// the maximum number of possible players.
    /// </summary>
    public const int maxNumberOfPlayers = 4;
    /// <summary>
    /// The true number of players.
    /// </summary>
    private int numberOfPlayers;
    /// <summary>
    /// Speed of the color of the specified frame.
    /// </summary>
    private float frameLerpSpeed = 0.5f;
    /// <summary>
    /// The fringe in which the color stops lerping.
    /// </summary>
    private float frameLerpFringe = 0.5f;
    /// <summary>
    /// An instance of SceneController class.
    /// </summary>
    private SceneController mySceneContr;
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
    /// All the selectors.
    /// </summary>
    public GameObject[] selectors;
    /// <summary>
    /// All frames.
    /// </summary>
    public UISprite[] frames;
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
    /// All the raedy buttons.
    /// </summary>
    public UIButton[] readyButtons;
    /// <summary>
    /// All the players.
    /// </summary>
    public Player[] players;
    /// <summary>
    /// The instance of ChacaterSelectorManager.
    /// </summary>
    public static CharacterSelectorManager instance;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        mySceneContr = new SceneController();
    }

    private void Start()
    {
        numberOfPlayers = Configuration.instance.GetNumberOfPlayers();
        Debug.Log(numberOfPlayers);
        indexes = new int[numberOfPlayers];
        readys = new bool[numberOfPlayers];
        //ids = new int[numberOfPlayers];
        //ids = ReInput.players.GetPlayerIds();
        selectedGrannies = new GameObject[numberOfPlayers];
        SelectorsActivation();
        players = new Player[numberOfPlayers];
        canSelect = new bool[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
    }

    private void Update()
    {
        MoveControllerAxis();
        PressControllerButton();
        StartMatch();
    }

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
        selectedGrannies[id] = grannies[indexes[id]];
    }

    public void MoveControllerAxis()
    {
        //string tag = button.tag;
        //int id = int.Parse(button.transform.parent.tag);
        for (int id = 0; id < numberOfPlayers; id++)
        {
            if (canSelect[id])
            {
                if (players[id].GetAxis("Move horizontal") < -0.2f || players[id].GetAxis("Move horizontal") > 0.2f)
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
                    selectedGrannies[id] = grannies[indexes[id]];
                }
                canSelect[id] = false;
                //Debug.Log(indexes[id]);
                centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
                playersLabels[id].text = granniesNames[indexes[id]];
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
        for (int id = 0; id < numberOfPlayers; id++)
        {
            if (players[id].GetButtonDown("Cross"))
            {
                readyButtons[id].SetState(UIButtonColor.State.Pressed, true);
                for (int i = 0; i < readyButtons[id].onClick.Count; i++)
                {
                    readyButtons[id].onClick[i].Execute();
                }
                readyButtons[id].SetState(UIButtonColor.State.Normal, false);
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

    public void ClickReady(GameObject button)
    {
        int id = int.Parse(button.transform.parent.tag);
        if (!readys[id])
        {
            //Debug.Log(Time.maximumDeltaTime);
            //Debug.Log(Time.deltaTime);
            float timeToWait = 0;
            float rgb = frames[id].color.r + frames[id].color.g + frames[id].color.b;
            float confRGB = Configuration.instance.playersColors[id].r + Configuration.instance.playersColors[id].g + Configuration.instance.playersColors[id].b;
            while (rgb > confRGB + frameLerpFringe)
            {
                if (timeToWait >= Time.deltaTime)
                {
                    //Debug.Log("SSSSS");
                    rgb = frames[id].color.r + frames[id].color.g + frames[id].color.b;
                    frames[id].color = Color.Lerp(frames[id].color, Configuration.instance.playersColors[id], frameLerpSpeed);
                }
                else
                {
                    timeToWait += Time.deltaTime;
                }
            }
            frames[id].color = Configuration.instance.playersColors[id];
            readys[id] = true;
        }
        else
        {
            frames[id].color = Color.white;
            readys[id] = false;
        }
    }

    ienumerat

    void StartMatch()
    {
        foreach (Player player in players)
        {
            if (player.GetButtonDown("Start"))
            {
                startButton.SetState(UIButtonColor.State.Pressed, true);
                for (int i = 0; i < startButton.onClick.Count; i++)
                {
                    startButton.onClick[i].Execute();
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