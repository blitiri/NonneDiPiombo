using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private Transform[] targetArr = new Transform[4];

	void Awake(){
		targetArr = GameObject.Find ("CameraRig").GetComponent<CameraControl> ().m_Targets;
	}
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i <= targetArr.Length; i++) {
			targetArr [i] = GameObject.FindGameObjectWithTag ("Player" + (i+1)).transform; 
		}

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
