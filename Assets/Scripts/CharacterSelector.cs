using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour, IComparer<int>
{

    public const int maxNumberOfPlayers = 4;
    public static int numberOfPlayers = 2;
    public static int readys = 0;
    public int index = 0;
    private GameObject selectedGranny;
    public static GameObject[] selectedGrannies;
    public static List<GameObject> characterSelectors = new List<GameObject>();
    public UISprite grannyIcon;
    public CharacterSelectorData cSDataInstance;

    void Awake()
    {
        characterSelectors.Add(gameObject);
        while(characterSelectors.Count != 4)
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
}
