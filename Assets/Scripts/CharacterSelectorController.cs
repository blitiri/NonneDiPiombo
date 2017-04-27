using UnityEngine;
using System.Collections;

public class CharacterSelectorController : MonoBehaviour {


    int index;
    public int? myPlayer;

    public UI2DSprite icon;

    public UIButton leftButton;
    public UIButton rightButton;

    public GameObject selectedGranny;

    //private void Awake()
    //{

    //}

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(index);
	}

    public void Click(string buttonTag)
    {
//        string buttonTagBeginning = buttonName.Contains("Left") ? "Left" : !buttonName.Contains("Right") ? "Right" : "Null";

        switch (buttonTag)
        {
            case "LeftButton":
                if (index == 0)
                {
                    index = CharacterSelectorManager.instance.numberOfDifferentGrannies - 1;
                }
                else
                {
                    index--;
                }
                break;
            case "RightButton":
                if (index == CharacterSelectorManager.instance.numberOfDifferentGrannies - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                break;
        }
        icon.sprite2D = CharacterSelectorManager.instance.grannieIcons[index];
        selectedGranny = CharacterSelectorManager.instance.grannies[index];
        CharacterSelectorManager.instance.playerSelectedGrannies[(int)myPlayer] = selectedGranny;
    }
}
