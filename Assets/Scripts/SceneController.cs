using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{


//	private const string levelPrefix = "Level";

    public GameManager myGameManager;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        LoadingEndMenu();
	}
		
	public void LoadingScene(GameObject button)
    {
//		if (!SceneManager.GetActiveScene().name.StartsWith(levelPrefix))
//        {
	    SceneManager.LoadScene(button.transform.tag, LoadSceneMode.Single);
//        }
    }

    public void LoadingEndMenu()
    {
        if (myGameManager)
        {
			if (GameManager.instance.IsRoundEnded()/* && SceneManager.GetActiveScene().name.StartsWith(levelPrefix)*/)
            {
                   SceneManager.LoadScene("Ending", LoadSceneMode.Single);
            }
        }
    }

    /// <summary>
    /// Clicking on Quit button
    /// </summary>
    public void OnClickQuitButton()
    {
        Application.Quit();
    }

}
