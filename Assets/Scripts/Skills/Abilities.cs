using UnityEngine;
using System.Collections;

public abstract class Abilities : ScriptableObject {

	protected GameObject player;

	public abstract void OnAbilityActivation();
	public void SetPlayer(GameObject player){
		this.player = player;
	}


}
