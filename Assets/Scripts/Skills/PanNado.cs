using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PanNado : Abilities {


	public override void OnAbilityActivation ()
	{
        player.transform.GetChild(1).gameObject.SetActive(true);
	}
}
