﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : MonoBehaviour {

	public Rigidbody[] rb;
	public GameObject bloodPrefab;
	public float bloodDuration;
    public AudioClip corpseSplat;
    private AudioSource source;
    public float volumeSound = 1.0f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
        
	// Use this for initialization
	void Start () {
		rb = GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rigid in rb) {
			//rigid.AddExplosionForce(Random.Range(500,1000), rigid.transform.position, 5);
			rigid.AddForce(rigid.transform.up * Random.Range(20,100), ForceMode.Impulse);
			GameObject bloodSplat = Instantiate (bloodPrefab, transform.position + new Vector3(0,0.1f,0), transform.rotation) as GameObject;
            source.PlayOneShot(corpseSplat, volumeSound);
			Destroy (bloodSplat, bloodDuration);
			Destroy (this.gameObject, 5);
		}
	}


}
