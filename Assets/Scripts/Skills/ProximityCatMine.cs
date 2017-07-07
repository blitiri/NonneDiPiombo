using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ProximityCatMine : Abilities {

	public GameObject catPrefab;
    public int maxCatNumber = 3;


	public override void OnAbilityActivation ()
	{
		for (int i = 0; i < players.Length; i++) 
		{
			if (players [i] != null) {
				if(GameManager.instance.catCounter<=maxCatNumber)
				{
					GameObject cat = Instantiate(catPrefab, players[i].transform.position, Quaternion.identity) as GameObject;
					cat.tag = "Bullet" + players[i].tag;
				}
				players [i] = null;
			}
		}
        
	}
}