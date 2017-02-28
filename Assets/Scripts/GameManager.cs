using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;
	//array di transform per il respawn dei player
	public Transform[] respawnPlayer;
	public GameObject[] players;
	private PlayerControl[] playersControls;

	void Awake ()
	{
		int playerIndex;

		instance = this;
		playersControls = new PlayerControl[players.Length];
		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			playersControls [playerIndex] = players [playerIndex].GetComponent<PlayerControl> ();
			playersControls [playerIndex].playerNumber = playerIndex + 1;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		RespawnPlayer ();
	}

	public void RespawnPlayer ()
	{
		GameObject player;
		int playerIndex;
		int spawnpointIndex;

		for (playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			player = players [playerIndex];
			if (playersControls [playerIndex].getLife () <= 0) {
				player.GetComponentInChildren<SkinnedMeshRenderer> (false);
				spawnpointIndex = Random.Range (0, respawnPlayer.Length);
				player.transform.position = respawnPlayer [spawnpointIndex].position;
				playersControls [playerIndex].Reset ();
				player.GetComponentInChildren<SkinnedMeshRenderer> (true);
			}
		}
	}
}
