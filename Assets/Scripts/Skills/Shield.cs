using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public GameObject player;
    public float lifeTime = 1.0f;
    public float speedRotation = 20.0f;
    private float timer = 0.0f;
    private AudioSource source;
    public AudioClip holyShieldSound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        source.PlayOneShot(holyShieldSound);
        if (timer < lifeTime)
        {
            transform.RotateAround(player.transform.position, player.transform.up, speedRotation);
            timer += Time.deltaTime;
        }
        else
        {
            this.gameObject.SetActive(false);
            player.GetComponent<PlayerControl>().isImmortal = false;
            timer = 0.0f;
        }

    }
}
