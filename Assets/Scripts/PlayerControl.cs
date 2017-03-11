using UnityEngine;
using System.Collections;

/// <summary>
/// Player control.
/// </summary>
public class PlayerControl : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform gunSpawnpoint;
	public int startingAmmo = 20;
	public int startingLife = 100;
	public int startingStress = 0;
	public int maxAmmoValue = 20;
	public int maxLifeValue = 100;
	public int maxStressValue = 100;
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

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
		ani = GetComponent<Animator> ();
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		ResetStatus ();
	}

	/// <summary>
	/// Updates the player instance.
	/// </summary>
	void Update ()
	{
		if (!underAttack) {
			if (!stopped) {
				horizontalMovement = Input.GetAxis ("Horizontal" + playerNumber);
				verticalMovement = Input.GetAxis ("Vertical" + playerNumber);
				shot = Input.GetAxis ("Shot" + playerNumber);
				melee = Input.GetAxis ("Melee" + playerNumber);
				Move ();
				Shoot ();
				Melee ();
			}
		}
	}

	/// <summary>
	/// Resets the player status.
	/// </summary>
	public void ResetStatus ()
	{
		if (otherConnectedPlayers == null) {
			otherConnectedPlayers = new ArrayList ();
		} else {
			otherConnectedPlayers.Clear ();
		}
		ammo = startingAmmo;
		life = startingLife;
		stress = startingStress;
		stopped = false;
		underAttack = false;
		timerToShoot = maxTimeToShoot;
		UpdateUI ();
	}

	/// <summary>
	/// Moves the player.
	/// </summary>
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

	/// <summary>
	/// Executes a melee attack against other connected players
	/// </summary>
	private void Melee ()
	{
		PlayerControl control;

		if ((otherConnectedPlayers.Count > 0) && (melee > 0)) {
			//Debug.Log ("Contacted players: " + otherConnectedPlayers.Count + " - Melee: " + melee);
			foreach (GameObject otherPlayer in otherConnectedPlayers) {  
				control = otherPlayer.GetComponent<PlayerControl> ();
				control.Attacked (20);
			}
		}
	}

	/// <summary>
	/// Notifies player is under attack by another.
	/// </summary>
	/// <param name="damage">Attack damage.</param>
	public void Attacked (int damage)
	{
		underAttack = true;
		StartCoroutine (AttackAnimation (damage));
	}

	/// <summary>
	/// Coroutine to show attack animation.
	/// </summary>
	/// <returns>The animation.</returns>
	/// <param name="damage">Attack damage.</param>
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

	/// <summary>
	/// Executes a shoot attack against other connected players
	/// </summary>
	private void Shoot ()
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
			UpdateUI ();
		}
	}

	/// <summary>
	/// Detects a collision enter with another player
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter (Collision collision)
	{
		//Debug.Log ("Collision enter detected: " + collision.gameObject.tag);
		if (collision.gameObject.tag.StartsWith ("Player")) {
			otherConnectedPlayers.Add (collision.gameObject);
		}
	}

	/// <summary>
	/// Detects a collision exit with another player
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionExit (Collision collision)
	{
		//Debug.Log ("Collision exit detected: " + collision.gameObject.tag);
		if (collision.gameObject.tag.StartsWith ("Player")) {
			otherConnectedPlayers.Remove (collision.gameObject);
		}
	}

	/// <summary>
	/// Detects a trigger enter with a bullet
	/// </summary>
	/// <param name="other">Collider.</param>
	void OnTriggerEnter (Collider other)
	{
		//Debug.Log ("Trigger detected: " + other.gameObject.tag);
		if (other.gameObject.tag.Equals ("Bullet")) {
			Destroy (other.gameObject);
			AddDamage (bulletDamage);
		}
	}

	/// <summary>
	/// Adds the damage to the player.
	/// </summary>
	/// <param name="damage">Damage.</param>
	private void AddDamage (int damage)
	{
		life -= bulletDamage;
		if (life < 0) {
			life = 0;
		}
		UpdateUI ();
	}

	/// <summary>
	/// Adds the ammo to the player.
	/// </summary>
	/// <param name="ammo">Ammo.</param>
	public void AddAmmo (int ammo)
	{
		this.ammo += ammo;
		if (this.ammo > maxAmmoValue) {
			this.ammo = maxAmmoValue;
		}
		UpdateUI ();
	}

	/// <summary>
	/// Adds the life to the player.
	/// </summary>
	/// <param name="life">Life.</param>
	public void AddLife (int life)
	{
		this.life += life;
		if (life > maxLifeValue) {
			life = maxLifeValue;
		}
		UpdateUI ();
	}

	/// <summary>
	/// Gets the player's life.
	/// </summary>
	/// <returns>The life.</returns>
	public int GetLife ()
	{
		return life;
	}

	/// <summary>
	/// Determines whether the player is under attack.
	/// </summary>
	/// <returns><c>true</c> if this instance is under attack; otherwise, <c>false</c>.</returns>
	public bool IsUnderAttack ()
	{
		return underAttack;
	}

	/// <summary>
	/// Updates the UI with player's statistics.
	/// </summary>
	private void UpdateUI ()
	{
		// Cast to float is required to avoid an integer division
		UIManager.instance.SetLife ((float)life / maxLifeValue, playerNumber);
		// Cast to float is required to avoid an integer division
		UIManager.instance.SetStress ((float)stress / maxStressValue, playerNumber);
		UIManager.instance.SetMaxAmmo (maxAmmoValue, playerNumber);
		UIManager.instance.SetAmmo (ammo, playerNumber);
	}
}
