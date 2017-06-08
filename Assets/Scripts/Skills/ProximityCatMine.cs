using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ProximityCatMine : Abilities {

	public GameObject catPrefab;
    public int maxCatNumber = 3;


	public override void OnAbilityActivation ()
	{
        if(GameManager.instance.catCounter!= maxCatNumber)
        {
            GameObject cat = Instantiate(catPrefab, player.transform.position, Quaternion.identity) as GameObject;
            cat.tag = "Bullet" + player.tag;
            GameManager.instance.catCounter++;
        }
	}
}
