using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMine : MonoBehaviour 
{
	private TweenColor tweenColor;
	private SphereCollider collider;
	private MeshRenderer mesh;
	public GameObject explosionParticle;
	public float timerToExplode=5.0f;
	public float radiusOfCollider;


	void Awake () 
	{
		tweenColor = GetComponent<TweenColor> ();
		collider = GetComponent<SphereCollider> ();
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
		this.collider.radius = radiusOfCollider;
		mesh.enabled = false;
		explosionParticle.SetActive(true);
		Destroy (this.gameObject,2.0f);
	}
}