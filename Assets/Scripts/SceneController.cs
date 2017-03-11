using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public GameManager myGameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        LoadingEndMenu();
	}

    public void LoadingScene()
    {
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Ending1" || SceneManager.GetActiveScene().name == "Ending2")
        {
            SceneManager.LoadScene("NonneDiPiombo", LoadSceneMode.Single);
        }
    }

    public void LoadingEndMenu()
    {
        if (myGameManager)
        {
            if (SceneManager.GetActiveScene().name == "NonneDiPiombo")
            {
                if (myGameManager.player1Kills > 2)
                {
                    SceneManager.LoadScene("Ending1", LoadSceneMode.Single);
                }
                else if (myGameManager.player2Kills > 2)
                {
                    SceneManager.LoadScene("Ending2", LoadSceneMode.Single);
                }
            }
        }
    }


}
