using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScrollManager : MonoBehaviour
{
	public static CreditsScrollManager instance;
	public Transform gameDesigners;
	public Transform conceptArtists;
	public Transform threeDArtists;
	public Transform programmersArtists;
	public float autoScrollSpeed = .001f;
	public float bottomReposition = -660f;
	public float topLimitForReposition = .3f;

	void Awake ()
	{
		instance = this;
	}

	public void StartScroll ()
	{
		StartCoroutine (AutoScroll ());
	}

	public void StopScroll ()
	{
		Debug.Log ("Stop auto scroll");
		StopCoroutine (AutoScroll ());
	}

	private IEnumerator AutoScroll ()
	{
		while (true) {
			AutoScrollElement (gameDesigners);
			AutoScrollElement (conceptArtists);
			AutoScrollElement (threeDArtists);
			AutoScrollElement (programmersArtists);
			yield return null;
		}
	}

	private void AutoScrollElement (Transform element)
	{
		Vector3 newPosition;
		float y;

		y = element.position.y + autoScrollSpeed;
		//Debug.Log (y + " >= " + topLimitForReposition + ": " + (y >= topLimitForReposition));
		if (y >= topLimitForReposition) {
			Debug.Log ("Repositioned");
			y = bottomReposition;
		}
		newPosition = new Vector3 (element.position.x, y, element.position.z);
		//Debug.Log ("newPosition: " + newPosition);
		element.position = newPosition;
	}
}
