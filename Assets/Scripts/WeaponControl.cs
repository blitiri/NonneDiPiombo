using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour 
{
	public int ammoMagazine;
	public float ratioOfFire;
	public float speedBullet;
	public Transform bulletSpawnPoint;
	public GameObject standardBulletPrefab;
	public GameObject powerUpBulletPrefab;
	public float weaponStress;
	private string playerID;
	private float timerToShoot;

	public bool isDefaultWeapon;

	/// <summary>
	/// Executes a shoot attack against other connected players
	/// </summary>
	public void Shoot(string playerId)
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
			bullet.tag = playerId;
			bulletRigidbody = bullet.GetComponent<Rigidbody>();
			bulletRigidbody.AddForce(bulletSpawnPoint.transform.up*speedBullet, ForceMode.Impulse);
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