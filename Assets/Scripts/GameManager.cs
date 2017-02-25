using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;


	public GameObject player1;
	public GameObject player2;

	/*[HideInInspector]*/ public PlayerControl player1Control;
	/*[HideInInspector]*/ public PlayerControl player2Control;

    public Transform[] respawn;


	void Awake(){
		instance = this;

		player1Control = player1.GetComponent<PlayerControl>();
		player2Control = player2.GetComponent<PlayerControl>();
	}
	
	// Use this for initialization
	void Start () {
		player1Control.playerNumber = 1;
		player2Control.playerNumber = 2;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RespawnPlayer()
    {
        
    }

}
