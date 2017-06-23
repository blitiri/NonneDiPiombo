using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour {

    public float lifeTime = 2.0f;
    public Rigidbody[] rb;


    void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigid in rb)
        {
            rigid.AddForce(rigid.transform.forward * Random.Range(20,40), ForceMode.Impulse);
            Destroy(this.gameObject, lifeTime);
        }
    }
}
