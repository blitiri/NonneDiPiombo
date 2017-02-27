using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;

    
	public GameObject player1;
	public GameObject player2;

	/*[HideInInspector]*/ public PlayerControl player1Control;
	/*[HideInInspector]*/ public PlayerControl player2Control;

    public Transform[] respawnPlayer;//array di transform per il respawn dei player


	void Awake()
    {
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
	void Update ()
    {
        RespawnPlayer();
    }



    public void RespawnPlayer()
    {
        if(player1Control.life==0)
        {
            player1.GetComponentInChildren<SkinnedMeshRenderer>(false);
            int random = Random.Range(1, 14);
            player1.transform.position=respawnPlayer[random].position;
            player1Control.life = 100;
            player1Control.underAttack = false;
            player1.GetComponentInChildren<SkinnedMeshRenderer>(true);

        }
        if (player2Control.life == 0)
        {
            player2.GetComponentInChildren<SkinnedMeshRenderer>(false);
            int random = Random.Range(1, 14);
            player2.transform.position = respawnPlayer[random].position;
            player2Control.life = 100;
            player2Control.underAttack = false;
            player2.GetComponentInChildren<SkinnedMeshRenderer>(true);
        }

    }

}
