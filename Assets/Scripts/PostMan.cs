using UnityEngine;
using System.Collections;

public class PostMan : MonoBehaviour
{
	public NavMeshAgent postMan;
	public float postManSpeed;
	private int pathIndex = 1;
   

	// Use this for initialization
	void Awake ()
	{
		postMan = GetComponent<NavMeshAgent> ();
	}

	void Start ()
	{
        
	}
	
	// Update is called once per frame
	void Update ()
	{
		PostManMove ();
	}

	void PostManMove ()
	{
	}
}
