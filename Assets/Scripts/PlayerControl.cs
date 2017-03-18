using UnityEngine;
using System.Collections;
using Rewired;

/// <summary>
/// Player control.
/// </summary>
public class PlayerControl : MonoBehaviour
{
	private bool isDashing;
	public GameObject bulletPrefab;
	public GameObject levelWallsEmpty;
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
	[HideInInspector]
	public int playerId;
	public float stressDecreaseFactor;
	public float timerToRefillStress;
	public float weaponStressDamage;
	private float horizontalMovement;
	private float verticalMovement;
	private Rigidbody rb;
	private IList otherConnectedPlayers;
	private Animator ani;
	private float shot;
	private float melee;
	private float timerToShoot;
	[Range (0, 10)]
	public float dashTime;
	[Range (0, 10)]
	public float dashDistance;
    public float dash;
	private bool underAttack;
	private bool stopped;
	private int ammo;
	private int life;
	private float stress;
	public float stressIncrease = 10;
	private RaycastHit info;
	public LayerMask environment;
	public Transform dashTransform;
	public Transform dashTransform2;

	// The Rewired Player
	private Player player;
	// Vector indicating player movement direction
	private Vector3 moveVector;
	// Vector indicating player aim direction
	private Vector3 aimVector;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		// Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
		player = ReInput.players.GetPlayer (playerId);
		rb = GetComponent<Rigidbody> ();
		ani = GetComponent<Animator> ();
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		ResetStatus ();
		StartCoroutine (RefillStress ());
	}

	/// <summary>
	/// Updates the player instance.
	/// </summary>
	void Update ()
	{
		if (!underAttack) {
			if (!stopped) {
				moveVector.z = -player.GetAxis ("Move horizontal");
				moveVector.x = player.GetAxis ("Move vertical");
				aimVector.z = -player.GetAxis ("Aim horizontal");
				aimVector.x = player.GetAxis ("Aim vertical");
                dash = player.GetAxis("Dash");
				shot = player.GetAxis ("Shoot");
				melee = player.GetAxis ("Melee");
				CheckingEnvironment ();
				Move ();
				Aim ();
				Shoot ();
				Melee ();
				StartDashing ();
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
	private void Move ()
	{
		rb.MovePosition (rb.position + moveVector * speed * Time.deltaTime);
		ani.SetFloat ("Movement", horizontalMovement);
		ani.SetFloat ("Movement", verticalMovement);
	}

	private void Aim ()
	{
		if (aimVector != Vector3.zero) {
			transform.forward = Vector3.Normalize (aimVector);
		}
	}

	private float RotationAngle ()
	{
		Vector3 normal;
		float angle;

		if (aimVector != Vector3.zero) {
			angle = Vector3.Angle (aimVector, transform.forward);
			//Debug.Log ("Angle: " + angle);
			normal = Vector3.Cross (aimVector, transform.forward);
			angle = (normal.y > 0 ? angle : -angle);
		} else {
			angle = 0;
		}
		return angle;
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
				control.Attacked (20, gameObject.tag);
			}
			AddStress (stressIncrease);
			UpdateUI ();
		}
	}

	/// <summary>
	/// Notifies player is under attack by another.
	/// </summary>
	/// <param name="damage">Attack damage.</param>
	public void Attacked (int damage, string killerTag)
	{
		underAttack = true;
		StartCoroutine (AttackAnimation (damage, killerTag));
	}

	/// <summary>
	/// Coroutine to show attack animation.
	/// </summary>
	/// <returns>The animation.</returns>
	/// <param name="damage">Attack damage.</param>
	IEnumerator AttackAnimation (int damage, string killerTag)
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
		AddDamage (damage, killerTag);
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
			this.stress += weaponStressDamage;
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
			AddDamage (bulletDamage, other.gameObject.tag);
			Destroy (other.gameObject);
		}
	}

	/// <summary>
	/// Adds the damage.
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <param name="killerTag">Killer tag.</param>
	private void AddDamage (int damage, string killerTag)
	{
		life -= bulletDamage;
		if (life < 0) {
			life = 0;
			GameManager.instance.KillMe (gameObject.tag, killerTag);
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

	public void AddStress (float stressAdd)
	{
		if (stress < maxStressValue) {
			stress += stressAdd;
		} else if (stress >= maxStressValue) {
			stress = maxStressValue;
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
	/// Gets the player's stress.
	/// </summary>
	/// <returns>The stress.</returns>
	public float GetStress ()
	{
		return stress;
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
		UIManager.instance.SetLife ((float)life / maxLifeValue, playerId);
		// Cast to float is required to avoid an integer division
		UIManager.instance.SetStress (stress / maxStressValue, playerId);
		UIManager.instance.SetMaxAmmo (maxAmmoValue, playerId);
		UIManager.instance.SetAmmo (ammo, playerId);
	}

	IEnumerator RefillStress ()
	{
		while (stress < 100) {
			yield return new WaitForSeconds (timerToRefillStress);
			this.stress -= stressDecreaseFactor;
			UpdateUI ();
		}
	}

	private void StartDashing ()
	{
        if (dash > 0)
        {
            if (!isDashing && stress <= maxStressValue - stressIncrease)
            {
                StartCoroutine(Dashing());
                isDashing = true;
            }
		}
	}

	private bool CheckingEnvironment ()
	{
		Vector3 ray = transform.position;


		Debug.DrawRay (ray, transform.forward);

		if (Physics.Raycast (ray, transform.forward, out info, dashDistance, environment))
			return true;

		return false;
	}

	private IEnumerator Dashing ()
	{
		Vector3 newPosition = Vector3.zero;

		if (!CheckingEnvironment ()) {
			dashTransform.localPosition = new Vector3 (dashTransform.localPosition.x, dashTransform.localPosition.y, dashTransform.localPosition.z + dashDistance);
			newPosition = new Vector3 (dashTransform.position.x, dashTransform.position.y, dashTransform.position.z);
		} else if (CheckingEnvironment ()) {
			dashTransform.position = new Vector3 (info.point.x, dashTransform.position.y, info.point.z);
			dashTransform.localPosition = new Vector3 (dashTransform.localPosition.x, dashTransform.localPosition.y, dashTransform.localPosition.z - 0.5f);
			newPosition = new Vector3 (dashTransform.position.x, dashTransform.position.y, dashTransform.position.z);
		}
		while (Vector3.Distance (transform.position, newPosition) > 1) {
			transform.position = Vector3.Lerp (transform.position, newPosition, 0.1f);
			yield return null;
		}
		dashTransform.localPosition = dashTransform2.localPosition;
		AddStress (stressIncrease);
		yield return new WaitForSeconds (dashTime);
		isDashing = false;
	}
}
