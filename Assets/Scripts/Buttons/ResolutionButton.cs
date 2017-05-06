using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionButton : MonoBehaviour
{

    int resolutionNumber;
    int x;
    int y;

    string selectedResolution;

    UILabel resLabel;

    //int[,] resolutions = new int { 1920, 1080; 1080, 720;  }

    void Start()
    {
        resLabel = GetComponentInChildren<UILabel>();
    }

    public void ChangingResolution()
    {
        Debug.Log(Screen.currentResolution);

        switch (resolutionNumber)
        {
            //case 0:
            //    selectedResolution = Screen.currentResolution.ToString() + "(Current)";
            //    break;
            case 0:
                x = 1920;
                y = 1080;
                resolutionNumber++;
                break;
            case 1:
                x = 1600;
                y = 900;
                resolutionNumber++;
                break;
            case 2:
                x = 1280;
                y = 720;
                resolutionNumber = 0;
                break;
        }
        selectedResolution = (x + "x" + y).ToString();
        resLabel.text = selectedResolution;
    }

    public void ApplyingResolution()
    {
        Screen.SetResolution(x, y, true, 0);
    }
}
