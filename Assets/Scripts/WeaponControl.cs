using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class WeaponControl : MonoBehaviour 
{
	public float ratioOfFire;

	public Transform bulletSpawnPoint;
	public GameObject standardBulletPrefab;
	public GameObject powerUpBulletPrefab;

	public float weaponStress;
    public PlayerControl playerScript;
	private Rigidbody playerRb;
	private string playerID;

    public float recoilForce;

    public GameObject onShootVFX;

	void Start(){
		playerRb = playerScript.gameObject.GetComponent<Rigidbody> ();
	}

	/// <summary>
	/// Executes a shoot attack against other connected players
	/// </summary>
	public void Shoot(string playerId)
	{
		GameObject bullet;

        GameObject shootVFX = Instantiate (onShootVFX, bulletSpawnPoint.position, Quaternion.identity) as GameObject;

		Destroy (shootVFX, 0.5f);

		bullet = Instantiate(standardBulletPrefab) as GameObject;
		bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
		bullet.transform.position = bulletSpawnPoint.position;
		bullet.tag = "Bullet" + playerId;
        playerScript.AddStress(weaponStress);
		playerRb.AddForce (playerRb.transform.forward * -recoilForce * Time.deltaTime, ForceMode.Impulse);
	}

	public void ShootSMG(string playerId, float spreadValue){
		GameObject bullet;

		Quaternion yValue = Quaternion.Euler (0, 0, Random.Range (-spreadValue, spreadValue));

        bullet = Instantiate(standardBulletPrefab) as GameObject;
		bullet.transform.rotation = bulletSpawnPoint.rotation ;
		bullet.transform.position = bulletSpawnPoint.position;
		bullet.tag = "Bullet" + playerId;

		Rigidbody projRb = bullet.GetComponent<Rigidbody> ();
		CapsuleCollider projCollider = bullet.GetComponent<CapsuleCollider> ();
		MeshRenderer projRenderer = bullet.GetComponent<MeshRenderer> ();
		SMGBulletManager projBulletManager = bullet.GetComponent<SMGBulletManager> ();
		projRenderer.enabled = true;
		projCollider.enabled = true;
		projBulletManager.canSpread = false;
	}
}