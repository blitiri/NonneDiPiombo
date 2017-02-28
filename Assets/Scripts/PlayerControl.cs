using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform gunSpawnpoint;
	public int startingAmmo = 20;
	public int maxLifeValue = 100;
	public float speed = 2;
	public float rotSpeed = 2;
	public float bulletLifeTime = 2;
	public float bulletInitialForce = 2;
	public float underAttackInactivityTime = 2;
	public float maxTimeToShoot = 1.0f;
	public int bulletDamage = 5;
	public int playerNumber;
	private float horizontalMovement;
	private float verticalMovement;
	private Rigidbody rb;
	private IList otherConnectedPlayers;
	private Animator ani;
	private float shot;
	private float melee;
	private float timerToShoot;
	private bool underAttack;
	private bool stopped;
	private int ammo;
	private int life;
	private int stress;

	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
		ani = GetComponent<Animator> ();
		Reset ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!underAttack) {
			if (!stopped) {
				horizontalMovement = Input.GetAxis ("Horizontal" + playerNumber);
				verticalMovement = Input.GetAxis ("Vertical" + playerNumber);
				shot = Input.GetAxis ("Shot" + playerNumber);
				melee = Input.GetAxis ("Melee" + playerNumber);
				Move ();
				Shot ();
				Melee ();
			}
		}
	}

	public void Reset ()
	{
		if (otherConnectedPlayers == null) {
			otherConnectedPlayers = new ArrayList ();
		} else {
			otherConnectedPlayers.Clear ();
		}
		ammo = startingAmmo;
		life = maxLifeValue;
		stress = 0;
		stopped = false;
		underAttack = false;
		timerToShoot = maxTimeToShoot;
	}

	public void Move ()
	{
		//DA APPROFONDIRE MEGLIO

		//movement
		//rb.AddForce (transform.right * horizontalMovement * speed * Time.deltaTime,ForceMode.Impulse);
		//rb.AddForce (transform.forward * verticalMovement * speed * Time.deltaTime, ForceMode.Impulse);

		Vector3 movePositionForward = transform.forward * verticalMovement * speed * Time.deltaTime;
		rb.MovePosition (rb.position + movePositionForward);

		//turning
		//da vedere Quaternion.LookRotation
		Quaternion turnRotationHorizontal = Quaternion.Euler (0f, horizontalMovement * rotSpeed * Time.deltaTime, 0f);
		rb.MoveRotation (rb.rotation * turnRotationHorizontal);
		ani.SetFloat ("Movement", horizontalMovement);
		ani.SetFloat ("Movement", verticalMovement);
	}

	private void Melee ()
	{
		PlayerControl control;

		if ((otherConnectedPlayers.Count > 0) && (melee > 0)) {
			Debug.Log ("Contacted players: " + otherConnectedPlayers.Count + " - Melee: " + melee);
			foreach (GameObject otherPlayer in otherConnectedPlayers) {  
				control = otherPlayer.GetComponent<PlayerControl> ();
				control.Attacked (20);
			}
		}
	}

	public void Attacked (int damage)
	{
		underAttack = true;
		StartCoroutine (AttackAnimation (damage));
	}

	IEnumerator AttackAnimation (int damage)
	{
		Vector3 animation;
		float startTime;

		animation = new Vector3 (0.2f, 0, 0);
		startTime = Time.time;
		while (Time.time - startTime < underAttackInactivityTime) {
			transform.Translate (animation);
			yield return null;
			animation = -animation;
		}
		AddDamage (damage);
		underAttack = false;
	}

	private void Shot ()
	{
		GameObject bullet;
		Rigidbody bulletRigidbody;

		if (timerToShoot < maxTimeToShoot) {
			timerToShoot += Time.deltaTime;
		} else if ((shot > 0) && (ammo > 0)) {
			bullet = Instantiate (bulletPrefab) as GameObject;
			bullet.transform.rotation = gunSpawnpoint.rotation;
			bullet.transform.position = gunSpawnpoint.position;
			bulletRigidbody = bullet.GetComponent<Rigidbody> ();
			bulletRigidbody.AddForce (bullet.transform.forward * bulletInitialForce, ForceMode.Impulse);
			Destroy (bullet, bulletLifeTime);
			ammo--;
			timerToShoot = 0.0f;
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		//Debug.Log ("Collision enter detected: " + collision.gameObject.tag);
		if (collision.gameObject.tag.StartsWith ("Player")) {
			otherConnectedPlayers.Add (collision.gameObject);
		}
	}

	void OnCollisionExit (Collision collision)
	{
		Debug.Log ("Collision exit detected: " + collision.gameObject.tag);
		if (collision.gameObject.tag.StartsWith ("Player")) {
			otherConnectedPlayers.Remove (collision.gameObject);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		//Debug.Log ("Trigger detected: " + other.gameObject.tag);
		if (other.gameObject.tag.Equals ("Bullet")) {
			Destroy (other.gameObject);
			AddDamage (bulletDamage);
		}
	}

	private void AddDamage (int damage)
	{
		life -= bulletDamage;
		if (life < 0) {
			life = 0;
		}
		UIManager.instance.setLife (life / maxLifeValue, playerNumber);
	}

	public void addAmmo (int ammo)
	{
		this.ammo += ammo;
	}

	public void addLife (int life)
	{
		this.life += life;
		if (life > maxLifeValue) {
			life = maxLifeValue;
		}
	}

	public int getLife ()
	{
		return life;
	}
}
