using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;

    
	public GameObject player1;
	public GameObject player2;
    public float lifePlayer1 = 100;
    public float lifePlayer2 = 100;

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
        if(lifePlayer1==0)
        {
            player1.GetComponentInChildren<SkinnedMeshRenderer>(false);
            int random = Random.Range(1, 14);
            player1.transform.position=respawnPlayer[random].position;
            lifePlayer1 = 100;
            player1.GetComponentInChildren<SkinnedMeshRenderer>(true);

        }
        if (lifePlayer2 == 0)
        {
            player2.GetComponentInChildren<SkinnedMeshRenderer>(false);
            int random = Random.Range(1, 14);
            player2.transform.position = respawnPlayer[random].position;
            lifePlayer2 = 100;
            player2.GetComponentInChildren<SkinnedMeshRenderer>(true);
        }

    }

}
