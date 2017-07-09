using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterSelectionManager : MonoBehaviour
{
    /// <summary>
    /// It is needed to know whether countdown is running or not.
    /// </summary>
    private bool RunCountdownIsRunning;
    private bool CountdownIsStarted;
    /// <summary>
    /// They are needed to slowly switch between button.
    /// </summary>
    public bool[] canSelect;
    /// <summary>
    /// Defines if a player is redy or not.
    /// </summary>
    public bool[] readys;
    /// <summary>
    /// Number of ready players.
    /// </summary>
    private int readyCount;
    /// <summary>
    /// Total number of players.
    /// </summary>
    public int totalPlayers;
    /// <summary>
    /// Seconds before the match starts.
    /// </summary>
    private const float countdownSeconds = 3;
    /// <summary>
    /// The count of seconds before the match starts.
    /// </summary>
    [SerializeField]
    private float countdownCount;
    /// <summary>
    /// Name of the net scene.
    /// </summary>
    [SerializeField]
    private string nextSceneToLoadName = "LevelPostOffice";
    /// <summary>
    /// All the grannies names in the atlas.
    /// </summary>
    [SerializeField]
    private string[] iconAtlasNames;
    /// <summary>
    /// All the grannies names.
    /// </summary>
    [SerializeField]
    private string[] granniesNames;
    /// <summary>
    /// Names of all weapons.
    /// </summary>
    [SerializeField]
    private string[] weaponsNames;
    /// <summary>
    /// Selector's color when granny is clicked.
    /// </summary>
    [SerializeField]
    private Color[] frameColors;
    /// <summary>
    /// Tweens of all selectors.
    /// </summary>
    [SerializeField]
    private TweenColor[] tweens;
    /// <summary>
    /// All "join" sprites.
    /// </summary>
    [SerializeField]
    private UILabel[] joinLabels;
    /// <summary>
    /// The countdown gameObject;
    /// </summary>
    [SerializeField]
    private UILabel countdown;
    /// <summary>
    /// All the grannies names.
    /// </summary>
    [SerializeField]
    private UILabel[] playerLabels;
    /// <summary>
    /// The player's number.
    /// </summary>
    [SerializeField]
    private UILabel[] playerNumbers;
    /// <summary>
    /// All the selectors.
    /// </summary>
    [SerializeField]
    private GameObject[] selectors;
    /// <summary>
    /// Grannies selected by each player.
    /// </summary>
    private GameObject[] selectedGrannies;
    /// <summary>
    /// All playable grannies.
    /// </summary>
    [SerializeField]
    private GameObject[] grannies;
    /// <summary>
    /// Weapons' icons.
    /// </summary>
    [SerializeField]
    private UISprite[] weapons;
    /// <summary>
    /// An icon for each selector.
    /// </summary>
    [SerializeField]
    private UIButton[] centrals;
    /// <summary>
    /// All arrows, rights first, the lefts.
    /// </summary>
    [SerializeField]
    private UIButton[] arrows;
    /// <summary>
    /// Indexes needed to choose your granny. One for each player.
    /// </summary>
    private int[] indexes;
    /// <summary>
    /// All the players.
    /// </summary>
    private List<Player> players;
    /// <summary>
    /// Players on the basis of who pressed Cross button first.
    /// </summary>
    private List<Player> orderedPlayers;

    void Awake()
    {
        indexes = new int[Configuration.maxNumberOfPlayers];
        canSelect = new bool[Configuration.maxNumberOfPlayers];
        readys = new bool[Configuration.maxNumberOfPlayers];
        selectedGrannies = new GameObject[Configuration.maxNumberOfPlayers];
        frameColors = new Color[Configuration.maxNumberOfPlayers];
        players = new List<Player>();
        orderedPlayers = new List<Player>();
        for (int i = 0; i < Configuration.maxNumberOfPlayers; i++)
        {
            players.Add(ReInput.players.GetPlayer(i));
        }
    }

    void Start()
    {
        for (int id = 0; id < Configuration.maxNumberOfPlayers; id++)
        {
            int leftIndex = id + arrows.Length / 2;

            Color color = Configuration.instance.playersColors[id];

            float h;
            float s;
            float v;

            playerLabels[id].color = color;
            playerNumbers[id].color = color;

            Color.RGBToHSV(color, out h, out s, out v);
            frameColors[id] = Color.HSVToRGB(h, s / 3, v);

            Color.RGBToHSV(arrows[id].defaultColor, out h, out s, out v);
            arrows[id].pressed = Color.HSVToRGB(h, s * 3, v);

            Color.RGBToHSV(arrows[leftIndex].defaultColor, out h, out s, out v);
            arrows[leftIndex].pressed = Color.HSVToRGB(h, s * 3, v);
        }
    }

    void Update()
    {
        //Debug.Log("capacity: " + orderedPlayers.Capacity);
        //Debug.Log("count: " + orderedPlayers.Count);
        for (int id = 0; id < orderedPlayers.Count; id++)
        {
            //Debug.Log(id + " id - name " + orderedPlayers[id].name);
            //Debug.Log("orderedPlayersindex: " + orderedPlayers.IndexOf(players[id]));
            //Debug.Log("playersid.id: " + players[id].id);
            if (orderedPlayers[id] != null)
            {
                MoveControllerAxis(orderedPlayers[id]);
                if (orderedPlayers[id].GetButtonDown("Cross"))
                {
                    PressCross(orderedPlayers[id]);
                }
                if (orderedPlayers[id].GetButtonDown("Circle"))
                {
                    //Debug.Log("SSSSSSSSS");
                    PressCircle(orderedPlayers[id]);
                }
            }
        }
        for (int id = 0; id < players.Count; id++)
        {
            if (players[id].GetButtonDown("Cross"))
            {
                PressCross(players[id]);
            }
        }
        CheckReady();
    }

    private void MoveControllerAxis(Player player)
    {
        int id = orderedPlayers.IndexOf(player);

        //Debug.Log("id: " + id);
        if (!readys[id] && canSelect[id])
        {
            if (player.GetAxis("Move horizontal") < -0.2f || player.GetAxis("Move horizontal") > 0.2f)
            {
                if (player.GetAxis("Move horizontal") < -0.2f)          //To decrease the index of the player which moves the axis.
                {
                    if (indexes[id] > 0)
                    {
                        indexes[id]--;
                    }
                    else
                    {
                        indexes[id] = iconAtlasNames.Length - 1;
                    }
                    arrows[id + arrows.Length / 2].SetState(UIButtonColor.State.Pressed, false);
                }
                else if (player.GetAxis("Move horizontal") > 0.2f)          //To increase the index of the player which moves the axis.
                {
                    if (indexes[id] < iconAtlasNames.Length - 1)
                    {
                        indexes[id]++;
                    }
                    else
                    {
                        indexes[id] = 0;
                    }
                    arrows[id].SetState(UIButtonColor.State.Pressed, false);
                }
                canSelect[id] = false;
                centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
                weapons[id].spriteName = weaponsNames[indexes[id]];
                playerLabels[id].text = granniesNames[indexes[id]];
            }
        }
        else
        {
            if (player.GetAxis("Move horizontal") > -0.2f && player.GetAxis("Move horizontal") < 0.2f)
            {
                if (arrows[id + arrows.Length / 2].state == UIButtonColor.State.Pressed)
                    arrows[id + arrows.Length / 2].SetState(UIButtonColor.State.Normal, false);
                else if (arrows[id].state == UIButtonColor.State.Pressed)
                    arrows[id].SetState(UIButtonColor.State.Normal, false);
                canSelect[id] = true;
            }
        }
    }

    //public void ClickArrow(GameObject button)
    //{
    //    string tag = button.tag;
    //    int id = int.Parse(button.transform.parent.tag);
    //    if (!readys[id])
    //    {
    //        //Debug.Log(id);
    //        switch (tag)
    //        {
    //            case "Left":
    //                if (indexes[id] > 0)
    //                {
    //                    indexes[id]--;
    //                }
    //                else
    //                {
    //                    indexes[id] = grannies.Length - 1;
    //                }
    //                break;
    //            case "Right":
    //                if (indexes[id] < grannies.Length - 1)
    //                {
    //                    indexes[id]++;
    //                }
    //                else
    //                {
    //                    indexes[id] = 0;
    //                }
    //                break;
    //        }
    //        //Debug.Log(indexes[id]);
    //        centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
    //    }
    //}

    //public void ClickCentral(GameObject button)
    //{
    //    int id = int.Parse(button.transform.parent.tag);
    //    //if (indexes[id] < grannies.Length - 1)
    //    //{
    //    //    indexes[id]++;
    //    //}
    //    //else
    //    //{
    //    //    indexes[id] = 0;
    //    //}
    //    //centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
    //    if (!readys[id])
    //    {
    //        readys[id] = true;
    //        selectedGrannies[id] = grannies[indexes[id]];
    //        tweens[id].SetStartToCurrentValue();
    //        tweens[id].to = frameColors[id];
    //        tweens[id].PlayForward();
    //        readyCount++;
    //        CheckReady();
    //    }
    //    else
    //    {
    //        tweens[id].SetEndToCurrentValue();
    //        tweens[id].from = Color.white;
    //        tweens[id].PlayReverse();
    //        readys[id] = false;
    //        readyCount--;
    //        CheckReady();
    //    }
    //}

    //private void PressEnter()
    //{

    //}

    private void PressCross(Player player)
    {
        bool toInsert = false;
        int slotToFillId = 0;

        for (int i = 0; i < orderedPlayers.Count; i++)
        {
            if (!orderedPlayers.Contains(player))
            {
                if (orderedPlayers[i] == null)
                {
                    //Debug.Log("capacityChecked");
                    toInsert = true;
                    slotToFillId = i;
                    break;
                }
            }
        }
        if (!orderedPlayers.Contains(player))
        {
            if (toInsert)
            {
                orderedPlayers[slotToFillId] = player;
            }
            else if (!toInsert)
            {
                orderedPlayers.Add(player);
            }
            int id = orderedPlayers.IndexOf(player);

            joinLabels[id].enabled = false;
            selectors[id].SetActive(true);

            totalPlayers++;
            Configuration.instance.SetNumberOfPlayers(totalPlayers);
        }
        else
        {
            int id = orderedPlayers.IndexOf(player);
            if (!readys[id])
            {
                readys[id] = true;
                selectedGrannies[id] = grannies[indexes[id]];
                tweens[id].SetStartToCurrentValue();
                tweens[id].to = frameColors[id];
                tweens[id].PlayForward();
                readyCount++;
                //CheckReady();
            }
        }
    }

    private void PressCircle(Player player)
    {
        int id = orderedPlayers.IndexOf(player);

        if (orderedPlayers.Contains(player) && readys[id])
        {
            tweens[id].SetEndToCurrentValue();
            tweens[id].from = Color.white;
            tweens[id].PlayReverse();
            readys[id] = false;
            readyCount--;
            //CheckReady();
        }
        else if (orderedPlayers.Contains(player) && !readys[id])
        {
            selectors[id].SetActive(false);
            joinLabels[id].enabled = true;
            orderedPlayers[id] = null;
            selectedGrannies[id] = null;
            indexes[id] = 0;
            centrals[id].normalSprite = iconAtlasNames[indexes[id]] + "PlayerIcon";
            weapons[id].spriteName = weaponsNames[indexes[id]];
            playerLabels[id].text = granniesNames[indexes[id]];
            totalPlayers--;
            //Debug.Log(indexes[id]);
            Configuration.instance.SetNumberOfPlayers(totalPlayers);
            //CheckReady();
        }
    }

    private void CheckReady()
    {
        //Debug.Log(Configuration.instance + " esiste");
        if (!CountdownIsStarted && readyCount == Configuration.instance.GetNumberOfPlayers())
        {
            StartCountdown();
            CountdownIsStarted = true;
        }
        else if (CountdownIsStarted && readyCount != Configuration.instance.GetNumberOfPlayers())
        {
            StopCountdown();
            CountdownIsStarted = false;
        }
    }

    public void StartCountdown()
    {
        if (countdownCount != countdownSeconds)
        {
            countdownCount = countdownSeconds;
        }
        if (!RunCountdownIsRunning)
        {
            StartCoroutine("RunCountdown");
            RunCountdownIsRunning = true;
        }
        if (!countdown.gameObject.activeInHierarchy)
        {
            countdown.gameObject.SetActive(true);
        }
    }

    public IEnumerator RunCountdown()
    {
        // +1 to leave last number a while to screen
        while (countdownCount + 1 > 0)
        {
            countdownCount -= Time.deltaTime;
            if (countdownCount > 0)
            {
                countdown.text = countdownCount.ToString("#");
            }
            yield return null;
        }
        Configuration.instance.players = orderedPlayers;
        LoadLevelScene(nextSceneToLoadName);
    }

    public void StopCountdown()
    {
        if (RunCountdownIsRunning)
        {
            StopCoroutine("RunCountdown");
            if (countdown.gameObject.activeInHierarchy)
            {
                countdown.gameObject.SetActive(false);
            }
            if (countdownCount != countdownSeconds)
            {
                countdownCount = countdownSeconds;
            }
            RunCountdownIsRunning = false;
        }
    }

    public void LoadLevelScene(string scene)
    {
        if (Configuration.instance)
        {
            Configuration.instance.selectedGrannies.Clear();
            for (int i = 0; i < selectedGrannies.Length; i++)
            {
                Configuration.instance.selectedGrannies.Add(selectedGrannies[i]);
            }
        }
        SceneController.instance.LoadSceneByName(scene);
    }
}