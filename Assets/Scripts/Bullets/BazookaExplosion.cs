using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaExplosion : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.StartsWith("Player")){
			Destroy (other.gameObject);
		}
	}
}
