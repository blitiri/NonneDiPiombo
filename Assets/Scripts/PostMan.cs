using UnityEngine;
using System.Collections;

public class PostMan : MonoBehaviour
{
    public NavMeshAgent postMan;
    public Transform[] path;
    public float postManSpeed;
    private int pathIndex=0;
   

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

            postMan.destination = path[pathIndex].transform.position;

            if (Vector3.Distance(postMan.transform.position, path[pathIndex].transform.position) <= 0.5)
            {

                pathIndex++;

                Debug.Log(pathIndex);

                if (pathIndex == path.Length)
                {
                    GameManager.instance.postManIsAlive = false;
                    Destroy(this.gameObject);
                }
            }
    }
}
