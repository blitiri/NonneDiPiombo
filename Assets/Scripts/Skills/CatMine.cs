using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMine : MonoBehaviour 
{
	private TweenColor tweenColor;
	private SphereCollider collider;
	public float timerToExplode;
	public float radiusOfCollider;

	void Awake () 
	{
		tweenColor = GetComponent<TweenColor> ();
		collider = GetComponent<SphereCollider> ();
	}

	void Start()
	{
		StartCoroutine ("CatExplosion");
	}

	IEnumerator CatExplosion()
	{
		tweenColor.enabled=false;
		yield return new WaitForSeconds (timerToExplode);
		this.collider.radius = radiusOfCollider;
	}
}
