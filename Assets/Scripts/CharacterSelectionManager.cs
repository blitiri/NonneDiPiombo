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
    /// Seconds before the match starts.
    /// </summary>
    private const float countdownSeconds = 3;
    /// <summary>
    /// The count of seconds before the match starts.
    /// </summary>
    [SerializeField] private float countdownCount;
    /// <summary>
    /// Name of the net scene.
    /// </summary>
    [SerializeField] private string nextSceneToLoadName = "LevelPostOffice";
    /// <summary>
    /// All the grannies names in the atlas.
    /// </summary>
    [SerializeField] private string[] iconAtlasNames;
    /// <summary>
    /// All the grannies names.
    /// </summary>
    [SerializeField] private string[] granniesNames;
    /// <summary>
    /// Selector's color when granny is clicked.
    /// </summary>
    private Color[] frameColors;
    /// <summary>
    /// Tweens of all selectors.
    /// </summary>
    [SerializeField] private TweenColor[] tweens;
    /// <summary>
    /// The countdown gameObject;
    /// </summary>
    [SerializeField] private UILabel countdown;
    /// <summary>
    /// All the grannies names.
    /// </summary>
    [SerializeField] private UILabel[] playerLabels;
    /// <summary>
    /// The player's number.
    /// </summary>
    [SerializeField] private UILabel[] playerNumbers;
    /// <summary>
    /// All the selectors.
    /// </summary>
    [SerializeField] private GameObject[] selectors;
    /// <summary>
    /// Grannies selected by each player.
    /// </summary>
    private GameObject[] selectedGrannies;
    /// <summary>
    /// All playable grannies.
    /// </summary>
    [SerializeField] private GameObject[] grannies;
    /// <summary>
    /// An icon for each selector.
    /// </summary>
    [SerializeField] private UIButton[] centrals;
    /// <summary>
    /// Indexes needed to choose your granny. One for each player.
    /// </summary>
    private int[] indexes;
    /// <summary>
    /// Players on the basis of who pressed Cross button first.
    /// </summary>
    private IList<Player> players;

    void Awake()
    {

        indexes = new int[Configuration.maxNumberOfPlayers];
        canSelect = new bool[Configuration.maxNumberOfPlayers];
        readys = new bool[Configuration.maxNumberOfPlayers];
        selectedGrannies = new GameObject[Configuration.maxNumberOfPlayers];
        players = new List<Player>();
    }

    void Start()
    {
        for(int id = 0; id < Configuration.instance.playersColors.Length; id++)
        {
            Color playerColor = Configuration.instance.playersColors[id];

            playerLabels[id].color = playerColor;
            playerNumbers[id].color = playerColor;

            float h;
            float s;
            float v;

            Color.RGBToHSV(playerColor, out h, out s, out v);
            frameColors[id] = Color.HSVToRGB(h, s / 3, v);
        }
    }

    void Update()
    {
        foreach (Player player in players)
        {

            int index = players.IndexOf(player);
            Debug.Log(index);
            MoveControllerAxis(player, index);
            PressCross(player, index);
            PressCircle(player, index);
        }
    }

    private void MoveControllerAxis(Player player, int index)
    {
        if (readys[index] && canSelect[index])
        {
            Debug.Log("SSSSS");
            if (player.GetAxis("Move horizontal") < -0.2f || player.GetAxis("Move horizontal") > 0.2f)
            {
                if (player.GetAxis("Move horizontal") < -0.2f)          //To decrease the index of the player which moves the axis.
                {
                    if (indexes[index] > 0)
                    {
                        indexes[index]--;
                    }
                    else
                    {
                        indexes[index] = iconAtlasNames.Length - 1;
                    }
                }
                else if (player.GetAxis("Move horizontal") > 0.2f)          //To increase the index of the player which moves the axis.
                {
                    if (indexes[index] < iconAtlasNames.Length - 1)
                    {
                        indexes[index]++;
                    }
                    else
                    {
                        indexes[index] = 0;
                    }
                }
                canSelect[index] = false;
                centrals[index].normalSprite = iconAtlasNames[indexes[index]] + "PlayerIcon";
                playerLabels[index].text = granniesNames[indexes[index]];
            }
        }
        else
        {
            if (player.GetAxis("Move horizontal") > -0.2f && player.GetAxis("Move horizontal") < 0.2f)
            {
                if (player.GetAxis("Move vertical") > -0.2f && player.GetAxis("Move vertical") < 0.2f)
                {
                    canSelect[index] = true;
                }
            }
        }
    }

    public void ClickArrow(GameObject button)
    {
        string tag = button.tag;
        int id = int.Parse(button.transform.parent.tag);
        if (!readys[id])
        {
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
    }

    private void PressCross(Player player, int index)
    {
        if (player.GetButtonDown("Cross"))
        {
            if (!players.Contains(player))
            {
                players.Add(player);
                selectors[index].SetActive(true);
            }
            else
            {
                if (!readys[index])
                {
                    selectedGrannies[index] = grannies[indexes[index]];
                    tweens[index].SetStartToCurrentValue();
                    tweens[index].to = frameColors[index];
                    tweens[index].PlayForward();
                    readys[index] = true;
                    readyCount++;
                }
            }
        }
    }

    private void PressCircle(Player player, int index)
    {
        if (player.GetButtonDown("Circle"))
        {
            if (players.Contains(player))
            {
                selectors[index].SetActive(false);
                players.Remove(player);
            }
            else
            {
                if (readys[index])
                {
                    tweens[index].SetEndToCurrentValue();
                    tweens[index].from = Color.white;
                    tweens[index].PlayReverse();
                    readys[index] = false;
                    readyCount--;
                }
            }
        }
    }

    private void CheckReady()
    {
        if (readyCount == Configuration.instance.GetNumberOfPlayers())
        {
            StartCountdown();
        }
        else
        {
            StopCountdown();
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
        Debug.Log(countdownCount);
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
        LoadLevelScene(nextSceneToLoadName);
    }

    public void StopCountdown()
    {
        if (RunCountdownIsRunning)
        {
            StopCoroutine("RunCountdown");
            RunCountdownIsRunning = false;
        }
        if (countdown.gameObject.activeInHierarchy)
        {
            countdown.gameObject.SetActive(false);
        }
        if (countdownCount != countdownSeconds)
        {
            countdownCount = countdownSeconds;
        }
    }

    public void LoadLevelScene(string scene)
    {
        if (Configuration.instance)
        {
            Configuration.instance.selectedGrannies = selectedGrannies;
        }
        SceneController.instance.LoadSceneByName(scene);
    }
}