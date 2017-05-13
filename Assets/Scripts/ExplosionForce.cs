using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : MonoBehaviour {

	public Rigidbody[] rb;

	// Use this for initialization
	void Start () {
		rb = GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rigid in rb) {
			//rigid.AddExplosionForce(Random.Range(500,1000), rigid.transform.position, 5);
			rigid.AddForce(rigid.transform.up * Random.Range(20,100), ForceMode.Impulse);
			Destroy (this.gameObject, 5);
		}
	}
}
