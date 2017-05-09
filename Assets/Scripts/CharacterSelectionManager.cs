using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    /// <summary>
    /// If the player is ready or not.
    /// </summary>
    public bool playerSelected;
    /// <summary>
    /// The number of playable players.
    /// </summary>
    [Range(2, 4)]
    public int numberOfPlayers;
    /// <summary>
    /// The index for the button.
    /// </summary>
    public int index;
    /// <summary>
    /// The selectors, one for each player.
    /// </summary>
    public GameObject[] selectors;
    /// <summary>
    /// Alle the grannies.
    /// </summary>
    public GameObject[] grannies;
    /// <summary>
    /// The names of icons in the atlas.
    /// </summary>
    public string[] iconNames;
    /// <summary>
    /// The granny selected by the player at the moment.
    /// </summary>
    public GameObject selectedGranny;

    void Start()
    {
        InstantiateSelectors();
    }

    private void Update()
    {
        Debug.Log(index);
    }

    public void InstantiateSelectors()
    {
        for (int i = numberOfPlayers; i > 0; i--)
        {
            selectors[i].SetActive(true);
        }
    }

    public void ClickArrowButton(GameObject button, UISprite icon)
    {
        switch (button.tag)
        {
            case "Left":
                if (index < 0)
                {
                    index = grannies.Length - 1;
                }
                else
                {
                    index--;
                }
                break;
            case "Right":
                if (index > grannies.Length - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                break;
        }
        icon.spriteName = iconNames[index] + "PlayerIcon";
    }

    public void ClickReadyButton()
    {
        selectedGranny = grannies[index];
        playerSelected = !playerSelected ? true : false;
    }
}
