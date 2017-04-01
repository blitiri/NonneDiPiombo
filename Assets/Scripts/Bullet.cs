using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	void OnTriggerEnter(){
		Destroy (this.gameObject);
	}
}
