using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMine : MonoBehaviour 
{
	private TweenColor tweenColor;
	private SphereCollider colliderCat;
	private MeshRenderer mesh;
	public GameObject explosionParticle;
	public float timerToExplode=5.0f;
	public float radiusOfCollider;


	void Awake () 
	{
		tweenColor = GetComponent<TweenColor> ();
		colliderCat = GetComponent<SphereCollider> ();
		mesh = GetComponent<MeshRenderer> ();
	}

	void Start()
	{
		StartCoroutine ("CatExplosion");
	}

	IEnumerator CatExplosion()
	{
		tweenColor.enabled=true;
		yield return new WaitForSeconds (timerToExplode);
		colliderCat.radius = radiusOfCollider;
		mesh.enabled = false;
		explosionParticle.SetActive(true);
        colliderCat.enabled = true;
        Destroy (this.gameObject,2.0f);
	}

}