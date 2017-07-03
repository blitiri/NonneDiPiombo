using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour {

    public int bulletTaken;
	public int bulletCount = 0;
    public GameObject brokenVersion;
    private BoxCollider bCollider;
    private MeshRenderer meshrenderer;
    private bool brokenHead = false;
    public float timerHeadDestroyed = 3.0f;

	private GameObject brokenObj;

	Component[] meshRenderers;

    void Awake()
    {
        Components();
    }
 

	void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag.StartsWith("Bullet"))
        {
            bulletCount++;

            if (bulletCount == bulletTaken)
            {
                brokenObj = Instantiate(brokenVersion, transform.position, transform.rotation) as GameObject;
				meshRenderers = brokenObj.GetComponentsInChildren (typeof(MeshRenderer));
				if (meshRenderers != null) {
					foreach (MeshRenderer mesh in meshRenderers){
						if (mesh.gameObject.name == "Head" ) {
                            Destroy(mesh.gameObject, timerHeadDestroyed);
						} 
					}
				}


                bCollider.enabled = false;
                bCollider.isTrigger = true;
                CameraShake.instance.shake = true;
                meshrenderer.enabled = false;
            }
        }
       
    }

    void Components()
    {
        bCollider = GetComponent<BoxCollider>();
        meshrenderer = GetComponent<MeshRenderer>();
    }
}
