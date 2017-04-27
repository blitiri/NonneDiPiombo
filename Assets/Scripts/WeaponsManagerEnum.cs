using UnityEngine;
using System.Collections;

public class WeaponsManagerEnum : MonoBehaviour
{
    public enum Weapons
    {
        Revolver,
        Uzi
    }
    public Weapons weapon;
   

    
    public int ammoMagazine = 100; 
    public float ratioOfFire = 1.0f; 
    public int weaponDamage = 20; 
    public float delayDeathWeapon = 5.0f;
    
    void Awake()
    {
        if (this.gameObject.tag == "Revolver")
        {
            weapon = Weapons.Revolver;
        }
        else if (this.gameObject.tag == "Uzi")
        {
            weapon = Weapons.Uzi;
        }

        switch (weapon)
        {
            case Weapons.Revolver:
                ammoMagazine = 100;
                ratioOfFire = 1.0f;
                weaponDamage = 20;
                break;
            case Weapons.Uzi:
                ammoMagazine = 60;
                ratioOfFire = 0.3f;
                weaponDamage = 15;
                delayDeathWeapon = 3.0f;
                break;
        }
    }
    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
       if(ammoMagazine==0)
        {
            Destroy(this.gameObject, delayDeathWeapon);
        }
    }

}
