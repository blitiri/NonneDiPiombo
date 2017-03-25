using UnityEngine;
using System.Collections;

public class PostMan : MonoBehaviour
{
    public NavMeshAgent postMan;
    public float postManSpeed;
    private int pathIndex=1;
   

	// Use this for initialization
	void Awake ()
    {
	  postMan = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        PostManMove();
    }

    void PostManMove()
    {
            postMan.speed = postManSpeed;

		    postMan.destination = GameManager.instance.postManTablePath.paths[GameManager.instance.postManRandomSpawnIndex].pathPoint[pathIndex].position;

		    if (Vector3.Distance(postMan.transform.position, GameManager.instance.postManTablePath.paths[GameManager.instance.postManRandomSpawnIndex].pathPoint[pathIndex].transform.position) <= 0.5)
            {

                pathIndex++;

                Debug.Log(pathIndex);

			    if (pathIndex == GameManager.instance.postManTablePath.paths[GameManager.instance.postManRandomSpawnIndex].pathPoint.Length)
                {
                    GameManager.instance.postManIsAlive = false;
                    Destroy(this.gameObject);
                }
            }
    }
}
