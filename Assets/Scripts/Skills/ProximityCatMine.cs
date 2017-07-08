using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ProximityCatMine : Abilities {

	public GameObject catPrefab;
    public int maxCatNumber = 3;


	public override void OnAbilityActivation ()
	{
        if (GameManager.instance.catCounter <= maxCatNumber-1)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    GameObject cat = Instantiate(catPrefab, players[i].transform.position, Quaternion.identity) as GameObject;
                    cat.tag = "Bullet" + players[i].tag;
                    players[i] = null;
                }
            }
        }
	}
}