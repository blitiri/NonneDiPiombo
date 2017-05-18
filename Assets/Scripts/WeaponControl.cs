﻿using System.Collections;
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
    public PlayerControl playerScript;
	private string playerID;
	private float timerToShoot;
    public float recoilForce;
    

	public bool isDefaultWeapon;

	void Start(){
		timerToShoot = ratioOfFire;
	}

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
			bulletRigidbody.velocity = bulletSpawnPoint.transform.up * speedBullet;
            playerScript.AddStress(weaponStress);
            playerScript.gameObject.transform.localPosition -= recoilForce * transform.right*Time.deltaTime;
            timerToShoot = 0.0f;
		}
	}
}