using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public Transform transformPickUp;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(this.gameObject.tag=="BulletPickUp")
        {
            transform.RotateAround(transform.position, Vector3.up, 5.0f);
        }
       
	}

    //effetti dei PickUp
    public void OnTriggerEnter(Collider other)
    {
        //Player1
        if(other.gameObject.tag=="Player1" && this.gameObject.tag=="BulletPickUp")
        {
            Destroy(this.gameObject);
            GameManager.instance.player1Control.ammo += 10;
        }
        if (other.gameObject.tag == "Player1" && this.gameObject.tag == "Medikit")
        {
            Destroy(this.gameObject);
            GameManager.instance.player1Control.life += 20;
        }

        //Player2
        if (other.gameObject.tag == "Player2" && this.gameObject.tag == "BulletPickUp")
        {
            Destroy(this.gameObject);
            GameManager.instance.player2Control.ammo += 10;
        }
        if (other.gameObject.tag == "Player2" && this.gameObject.tag == "Medikit")
        {
            Destroy(this.gameObject);
            GameManager.instance.player2Control.life += 20;
        }
    }

}
