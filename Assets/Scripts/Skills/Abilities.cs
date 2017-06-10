using UnityEngine;
using System.Collections;

public abstract class Abilities : ScriptableObject {

	protected GameObject[] players=new GameObject[Configuration.maxNumberOfPlayers];

	public abstract void OnAbilityActivation();
	public void SetPlayer(GameObject player,int playerIndex){
		this.players[playerIndex] = player;
	}
}