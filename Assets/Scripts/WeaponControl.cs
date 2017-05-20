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
	private Rigidbody playerRb;
	private string playerID;
	//private float timerToShoot;


    public float recoilForce;

	void Start(){
		playerRb = playerScript.gameObject.GetComponent<Rigidbody> ();
	}

	/// <summary>
	/// Executes a shoot attack against other connected players
	/// </summary>
	public void Shoot(string playerId)
	{
		GameObject bullet;
		Rigidbody bulletRigidbody;

	
		bullet = Instantiate(standardBulletPrefab) as GameObject;
		bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
		bullet.transform.position = bulletSpawnPoint.position;
		bullet.tag = playerId;
        playerScript.AddStress(weaponStress);
		playerRb.AddForce (playerRb.transform.forward * -recoilForce * Time.deltaTime, ForceMode.Impulse);
	}
}