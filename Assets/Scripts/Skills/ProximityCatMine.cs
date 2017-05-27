using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ProximityCatMine : Abilities {

	public GameObject catPrefab;


	public override void OnAbilityActivation ()
	{
		GameObject cat = Instantiate (catPrefab,player.transform.position,Quaternion.identity)as GameObject;

	}
}
