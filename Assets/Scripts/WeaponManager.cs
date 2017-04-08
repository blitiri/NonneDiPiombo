using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour 
{
	public int ammoMagazine=100;
	public float ratioOfFire=1.0f;
	public float weaponDamage=5.0f;



	// Use this for initialization
	void Start () 
	{
	    
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (ammoMagazine <= 0)
		{
			Destroy (this.gameObject);
		}
	}
}
