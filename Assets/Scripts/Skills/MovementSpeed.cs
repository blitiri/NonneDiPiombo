using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class MovementSpeed : Abilities 
{
	public float speedModify;
	public PlayerControl playerControl;

	public override void OnAbilityActivation ()
	{
		for (int i = 0; i < players.Length; i++) 
		{
			if (players [i] != null) {
				players[i].GetComponent<PlayerControl> ().speed += speedModify;
				players [i] = null;
			}
		}
	}
}
