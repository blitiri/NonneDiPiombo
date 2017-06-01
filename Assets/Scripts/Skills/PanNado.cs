using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PanNado : Abilities {

	public GameObject panPrefab;
    public Transform PanPosition;

	public override void OnAbilityActivation ()
	{
        /*GameObject pan = Instantiate (panPrefab)as GameObject;
        pan.transform.SetParent(player.transform);
        pan.transform.position = PanPosition.position;*/

        player.transform.FindChild("Pan").gameObject.SetActive(true);

	}
}
