using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PanNado : Abilities {


	public override void OnAbilityActivation ()
	{

		for (int i = 0; i < players.Length; i++) 
		{
			if (players [i] != null) {
				players[i].transform.GetChild(1).gameObject.SetActive(true);
				players[i].GetComponent<PlayerControl>().isImmortal = true;
				players [i] = null;
			}
		}  
	}
}