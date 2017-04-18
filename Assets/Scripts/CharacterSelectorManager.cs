using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelectorManager : MonoBehaviour {

    private const int maxNumberOfPlayers = 4;
    [Range(2, maxNumberOfPlayers)]
    public int numberOfPlayers = 2;
    public int numberOfDifferentGrannies = 2;

    public static CharacterSelectorManager instance;

    public Sprite[] grannieIcons;

    public GameObject[] playerSelectors;
    public GameObject[] grannies;
    public GameObject[] playerSelectedGrannies;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        playerSelectedGrannies = new GameObject[numberOfPlayers];
        SelectorsActivation();
        //    grannyIcons = new Sprite[numberOfDifferentGrannys];
        //    playerSelectors = new GameObject[maxNumberOfPlayers];
        //    grannys = new GameObject[numberOfDifferentGrannys];
    }

    void Start ()
    {
        for (int i = 0; i < playerSelectedGrannies.Length; i++)
        {
            playerSelectedGrannies[i] = grannies[0];
        }
    }
	
	void Update ()
    {

    }

    void SelectorsActivation()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerSelectors[i].SetActive(true);
            CharacterSelectorController thisPlayerSelectorController = playerSelectors[i].GetComponent<CharacterSelectorController>();
            if (thisPlayerSelectorController.myPlayer == null)
            {
                thisPlayerSelectorController.myPlayer = i;
            }
        }
        for (int i = numberOfPlayers; i < maxNumberOfPlayers; i++)
        {
            playerSelectors[i].SetActive(false);
            CharacterSelectorController thisPlayerSelectorController = playerSelectors[i].GetComponent<CharacterSelectorController>();
            if (thisPlayerSelectorController.myPlayer == null)
            {
                thisPlayerSelectorController.myPlayer = i;
            }
        }
    }
}
