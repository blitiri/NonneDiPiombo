using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour {

    public float lifeTime = 2.0f;
    public float force = 50.0f;
    public Rigidbody[] rb;
    private AudioSource source;
    public AudioClip rockFalling;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigid in rb)
        {
            rigid.AddForce(rigid.transform.forward *force*Time.deltaTime /* ForceMode.Force*/);
            source.PlayOneShot(rockFalling);
            Destroy(this.gameObject, lifeTime);
        }
    }
}
