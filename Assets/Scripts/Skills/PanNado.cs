using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PanNado : Abilities {


	public override void OnAbilityActivation ()
	{
        player.transform.FindChild("Pan").gameObject.SetActive(true);
	}
}
