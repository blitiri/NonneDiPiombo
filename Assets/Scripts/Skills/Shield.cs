using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public GameObject player;
    public float lifeTime = 1.0f;
    public float speedRotation = 20.0f;
    private float timer = 0.0f;

    void Update()
    {
        if (timer < lifeTime)
        {
            transform.RotateAround(player.transform.position, player.transform.up, speedRotation);
            timer += Time.deltaTime;
        }
        else
        {
            this.gameObject.SetActive(false);
            timer = 0.0f;
        }

    }
}
