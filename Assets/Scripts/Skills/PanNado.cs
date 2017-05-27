using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PanNado : Abilities {

	public GameObject panPrefab;


	public override void OnAbilityActivation ()
	{
		GameObject pan = Instantiate (panPrefab,player.transform.position,Quaternion.identity)as GameObject;
	}
}
