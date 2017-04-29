using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour 
{
	public int ammoMagazine;
	public float ratioOfFire;
	public float speedBullet;
	public GameObject standardBulletPrefab;
	public GameObject powerUpBulletPrefab;
	public float weaponStress;
	private string playerID;
	private Transform bulletSpawnPoint;
	private float timerToShoot;


	void Awake()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Executes a shoot attack against other connected players
	/// </summary>
	public void Shoot()
	{
		GameObject bullet;
		Rigidbody bulletRigidbody;

		if (timerToShoot < ratioOfFire)
		{
			timerToShoot += Time.deltaTime;
		}
		else if ((ammoMagazine > 0))
		{
			bullet = Instantiate(standardBulletPrefab) as GameObject;
			bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
			bullet.transform.position = bulletSpawnPoint.position;
			//bullet.tag = bulletTagPrefix + gameObject.tag;
			bulletRigidbody = bullet.GetComponent<Rigidbody>();
			bulletRigidbody.AddForce(bulletSpawnPoint.transform.up, ForceMode.Impulse);
		
			/*if(selectedWeapon.weapon!=WeaponsManagerEnum.Weapons.Revolver)
			{
				selectedWeapon.ammoMagazine--;
			}
			stress += weaponStressDamage;*/
			timerToShoot = 0.0f;
		}
	}

	/*/// <summary>
	/// Updates the UI with player's statistics.
	/// </summary>
	private void UpdateUI()
	{
		if (selectedWeapon.weapon==WeaponsManagerEnum.Weapons.Revolver)
		{
			//LevelUIManager.instance.ammoCounters [playerId].text = "--";
			LevelUIManager.instance.SetInfiniteAmmo(playerId);
		}
		else
		{
			LevelUIManager.instance.SetAmmo(selectedWeapon.ammoMagazine, playerId);
		}
		// Cast to float is required to avoid an integer division
		LevelUIManager.instance.SetLife(life / maxLifeValue, playerId);
		// Cast to float is required to avoid an integer division

		LevelUIManager.instance.SetStress(stress / maxStressValue, playerId);

	}*/
}