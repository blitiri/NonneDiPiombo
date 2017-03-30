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
	
   

	// Use this for initialization
	void Awake ()
    {
	  postMan = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
		postManRandomSpawnIndex = 0;
        //postManRandomSpawnIndex = Random.Range (0, 6);

       /* postManInversePosition=Random.Range(0,1);
        
        Debug.Log(postManStartPositionIsInvert);

		if (postManInversePosition == 0)
		{
			postManStartPositionIsInvert = true;
			pathIndex = postManTablePath.paths [postManRandomSpawnIndex].pathPoint.Length-1;
		} 
		else if(postManInversePosition == 2)
		{
			postManStartPositionIsInvert = false;
			pathIndex = 1;
		}*/

        postManIsAlive = true;


        StartCoroutine ("PostManMove");
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

	IEnumerator PostManMove()
    {
		
        postMan.speed = postManSpeed;

        postManInversePosition = Random.Range(0,2);

//        Debug.Log(postManInversePosition);

        if (postManInversePosition == 0)
        {
            postManStartPositionIsInvert = true;
            pathIndex = postManTablePath.paths[postManRandomSpawnIndex].pathPoint.Length - 1;
        }
        else if (postManInversePosition == 1)
        {
            postManStartPositionIsInvert = false;
            pathIndex = 1;
        }

        while (postManIsAlive==true)
		{
			postMan.destination = postManTablePath.paths[postManRandomSpawnIndex].pathPoint[pathIndex].position;

			if (Vector3.Distance (postMan.transform.position, postManTablePath.paths [postManRandomSpawnIndex].pathPoint [pathIndex].transform.position) <= 0.5) 
			{
                if(postManStartPositionIsInvert == true)
                {
                    pathIndex--;
                }
				else if(postManStartPositionIsInvert == false)
                {
                    pathIndex++;
                }

				Debug.Log (pathIndex);

				if (postManStartPositionIsInvert == true && pathIndex == 1 ) 
				{
					postManIsAlive = false;
					Destroy (this.gameObject);
				}
                else if(postManStartPositionIsInvert == false && pathIndex == postManTablePath.paths[postManRandomSpawnIndex].pathPoint.Length)
                {
                    postManIsAlive = false;
                    Destroy(this.gameObject);
                }
			}
			yield return null;
		}

	}    
}
