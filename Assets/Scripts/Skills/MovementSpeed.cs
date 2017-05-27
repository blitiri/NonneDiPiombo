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
		player.GetComponent<PlayerControl> ().speedMod = speedModify;
	}
}
