using UnityEngine;
using System.Collections;

public class PostMan : MonoBehaviour
{
    public NavMeshAgent postMan;
    public float postManSpeed;
    private int pathIndex;
	public PathTable postManTablePath;
	private int postManRandomSpawnIndex=0;
	private int postManInversePosition;
	public bool postManIsAlive;
	public bool postManStartPositionIsInvert;
	public bool Isarrived;
   

	// Use this for initialization
	void Awake ()
    {
	  postMan = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
		postManRandomSpawnIndex = 0;
		//postManRandomSpawnIndex = Random.Range (0, 6);

		postManInversePosition=Random.Range(0,1);

		if (postManInversePosition == 0)
		{
			Debug.Log ("ok");
			postManStartPositionIsInvert = true;
			pathIndex = postManTablePath.paths [postManRandomSpawnIndex].pathPoint.Length-1;
			Debug.Log (pathIndex);
		} 
		/*else if(postManInversePosition==1)
		{
			postManStartPositionIsInvert = false;
			pathIndex = 1;
		}*/

		StartCoroutine ("PostManMove");
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

	IEnumerator PostManMove()
    {
		
        postMan.speed = postManSpeed;
		while (pathIndex != 0)
		{
			postMan.destination = postManTablePath.paths[postManRandomSpawnIndex].pathPoint[pathIndex].position;

			if (Vector3.Distance (postMan.transform.position, postManTablePath.paths [postManRandomSpawnIndex].pathPoint [pathIndex].transform.position) <= 0.5) 
			{
				pathIndex--;

				Debug.Log (pathIndex);

				if (pathIndex == 0) 
				{
					postManIsAlive = false;
					Destroy (this.gameObject);
				}
			}
			yield return null;
		}

	}    
}
