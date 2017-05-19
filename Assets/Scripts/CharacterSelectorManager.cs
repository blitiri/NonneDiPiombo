using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelectorManager : MonoBehaviour
{
    /// <summary>
    /// the maximum number of possible players.
    /// </summary>
    public const int maxNumberOfPlayers = 4;
    /// <summary>
    /// The true number of players.
    /// </summary>
    [Range(2,4)]
    public int numberOfPlayers = 3;
    /// <summary>
    /// An instance of SceneController class.
    /// </summary>
    private SceneController mySceneContr;
    /// <summary>
    /// The indexes needed to switch between grannies. One for each player.
    /// </summary>
    public int[] indexes;

    public string nextSceneToLoadName = "LevelPostOffice";
    /// <summary>
    /// All the grannies.
    /// </summary>
    public string[] iconNames;
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
    /// An icon for each selector.
    /// </summary>
    public UIButton[] centrals;
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
        selectedGrannies = new GameObject[numberOfPlayers];
        SelectorsActivation();
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

    public void ClickArrow (GameObject button)
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
        centrals[id].normalSprite = iconNames[indexes[id]] + "PlayerIcon";
    }

    public void ClickCentral(GameObject button)
    {
        int id = int.Parse(button.transform.parent.tag);

        selectedGrannies[id] = grannies[indexes[id]];
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