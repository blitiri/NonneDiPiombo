using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PanNado : Abilities {

	public GameObject panPrefab;
	public Animator panAnimation;

	public override void OnAbilityActivation ()
	{
		GameObject pan = Instantiate (panPrefab,player.transform.position,Quaternion.identity)as GameObject;
		/*foreach (Transform child in player.transform) 
		{
			if (child.tag == "ShotGun")
			{
				child.gameObject.SetActive = false;
			}
			if (child.gameObject.tag == "Pan")
			{
				child.gameObject.SetActive = true;
			}
		}*/
	}
}
