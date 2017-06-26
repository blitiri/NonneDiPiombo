using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour {

    public int bulletTaken;
    private int bulletCount = 0;
    public GameObject brokenVersion;
    private BoxCollider bCollider;
    private MeshRenderer meshrenderer;
    public float maxTimer = 1.0f;
    private float timer = 0.0f;
    private bool rebuild = false;
	
    void Awake()
    {
        Components();
    }
 
    void Update()
    {
        if(rebuild==true)
        {
            if (timer <= maxTimer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                bCollider.enabled = true;
                bCollider.isTrigger = false;
                meshrenderer.enabled = true;
                rebuild = false;
                bulletCount = 0;
                timer = 0.0f;
            }
        }
    }

	void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag.StartsWith("Bullet"))
        {
            bulletCount++;

            if (bulletCount >= bulletTaken)
            {
                GameObject brokenObj = Instantiate(brokenVersion, transform.position, transform.rotation) as GameObject;
                bCollider.enabled = false;
                bCollider.isTrigger = true;
                CameraShake.instance.shake = true;
                meshrenderer.enabled = false;
                rebuild = true;
            }
        }
       
    }

    void Components()
    {
        bCollider = GetComponent<BoxCollider>();
        meshrenderer = GetComponent<MeshRenderer>();
    }
}
