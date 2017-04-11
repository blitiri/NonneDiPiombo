using UnityEngine;
using System.Collections;

public class CharacterSelectorManager : MonoBehaviour {

    private const int maxNumberOfPlayers = 4;
    [Range(2, maxNumberOfPlayers)]
    public int numberOfPlayers = 2;

    public GameObject[] playerSelectors = new GameObject[maxNumberOfPlayers];


    //void Awake()
    //{

    //}

    void Start ()
    {
        SelectorsActivation();
	}
	
	void Update ()
    {
	
	}

    void SelectorsActivation()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerSelectors[i].SetActive(true);
        }
        for (int i = numberOfPlayers; i < maxNumberOfPlayers; i++)
        {
            playerSelectors[i].SetActive(false);
        }
    }

}
