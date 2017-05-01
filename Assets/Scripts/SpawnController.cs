using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform[] spawns;

    public int itemCount;
	public int pickUp;
    //public int capsuleCount;

    //public GameObject revolverPrefab;
    //public GameObject smgPrefab;
    //public GameObject shotgunPrefab;
    //public GameObject rocketLauncher;
    public GameObject[] items;
    public GameObject[] pickUps;

    //public GameObject lifeCapsule;
    //public GameObject stressCapsule;

    void Start()
    {
        ItemSpawning();
    }

    void ItemSpawning()
    {
        int randomSpawnIndex;
        int randomItemIndex;
        int randomRotationY;

        GameObject item = null;

        for (int i = 0; i < itemCount; i++)
        {
            randomSpawnIndex = Random.Range(0, spawns.Length - 1);
            randomItemIndex = Random.Range(0, items.Length - 1);
            randomRotationY = Random.Range(0, 360);

            if (item)
            {
                if (Vector3.Distance(spawns[randomSpawnIndex].position, item.transform.position) <= 5 && items[randomItemIndex] != item)
                {
                    item = Instantiate(items[randomItemIndex], spawns[randomSpawnIndex].position, Quaternion.Euler(items[randomItemIndex].transform.rotation.x, randomRotationY, items[randomItemIndex].transform.rotation.z)) as GameObject;
                }
                else
                {
                    randomSpawnIndex = Random.Range(0, spawns.Length - 1);
                }
            }
            else
            {
                item = Instantiate(items[randomItemIndex], spawns[randomSpawnIndex].position, Quaternion.Euler(items[randomItemIndex].transform.rotation.x, randomRotationY, items[randomItemIndex].transform.rotation.z)) as GameObject;
            }
        }
    }

    void PickUpSpawning()
    {
        int randomSpawnIndex = Random.Range(0, spawns.Length - 1);

		Transform lastDropPoint; 

        for (int i = 0; i < pickUps.Length; i++)
        {
            GameObject pickUp = Instantiate(pickUps[i], spawns[randomSpawnIndex].position, pickUps[i].transform.rotation) as GameObject;
        }
    }
}
