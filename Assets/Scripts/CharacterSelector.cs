using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour, IComparer<int>
{

    public const int maxNumberOfPlayers = 4;
    public static int numberOfPlayers = 1;
    public static int readys = 0;
    /// <summary>
    /// Each controller needs it to switch between grannies.
    /// </summary>
    public int index = 0;
    /// <summary>
    /// Each controller needs it to know which controller it is.
    /// </summary>
    public int selectorID;
    private GameObject selectedGranny;
    private PlayerControl myPlayer;
    public static GameObject[] selectedGrannies;
    public CharacterSelector[] buttons;
    public static List<GameObject> characterSelectors = new List<GameObject>();
    public UISprite grannyIcon;
    public CharacterSelectorData cSDataInstance;
    public InputManager inputManager;

    void Awake()
    {
        characterSelectors.Add(gameObject);
        int.TryParse(gameObject.name, out selectorID);
        myPlayer = GameManager.instance.players[selectorID].GetComponent<PlayerControl>();
        while (characterSelectors.Count != 4)
        {
            return;
        }
        //Debug.Log(characterSelectors.Count);
        for (int i = 0; i < maxNumberOfPlayers; i++)
        {
            //Debug.Log(characterSelectors[i]);
            //characterSelectors.Sort(Compare());
            Debug.Log(characterSelectors[i].name);
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            //Debug.Log(characterSelectors[i]);
            characterSelectors[i].SetActive(true);
        }
        for (int i = numberOfPlayers; i < maxNumberOfPlayers; i++)
        {
            characterSelectors[i].SetActive(false);
        }
        grannyIcon.spriteName = cSDataInstance.iconsNames[index] + "PlayerIcon";
    }

    private void Update()
    {
        ManageController();
    }

    public int Compare(int x, int y)
    {
        if (x < y)
        {
            return -1;
        }
        else if (x > y)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void ArrowButton(GameObject button)
    {
        switch (button.tag)
        {
            case "Left":
                if (index <= 0)
                {
                    index = cSDataInstance.grannies.Length - 1;
                }
                else
                {
                    index--;
                }
                break;
            case "Right":
                if(index >= cSDataInstance.grannies.Length - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                break;
        }
        grannyIcon.spriteName = cSDataInstance.iconsNames[index] + "PlayerIcon";
    }

    public void CentralButton()
    {
        selectedGranny = cSDataInstance.grannies[index];
        selectedGrannies[0] = selectedGranny;
    }

    public void ManageController()
    {
        if(myPlayer.moveVector.x > 0.2f)
        {
            buttons[1].ArrowButton(buttons[1].gameObject);
        }
        else if (myPlayer.moveVector.x < -0.2f)
        {
            buttons[0].ArrowButton(buttons[0].gameObject);
        }
    }
}
