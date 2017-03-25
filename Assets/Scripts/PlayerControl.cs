using UnityEngine;
using System.Collections;

/// <summary>
/// Player control.
/// </summary>
public class PlayerControl : MonoBehaviour
{
	private const string bulletTagPrefix = "Bullet";
	private const string playerTagPrefix = "Player";
	private bool isDashing;
	public Texture2D crosshairCursor;
	public Vector2 cursorHotSpot = new Vector2 (16, 16);
	public GameObject bulletPrefab;
	public Transform weaponSpawnpoint;
	public GameObject weapon;
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
	//[HideInInspector]
	public int playerId;
	public float stressDecreaseFactor;
	public float timerToRefillStress;
	public float weaponStressDamage;
	private float horizontalMovement;
	private float verticalMovement;
	private Rigidbody rb;
	private IList otherConnectedPlayers;
	private Animator ani;
	private float timerToShoot;
	[Range (0, 10)]
	public float dashTime;
	[Range (0, 10)]
	public float dashDistance;
	private bool underAttack;
	private bool stopped;
	private int ammo;
	private int life;
	private float stress;
	private InputManager inputManager;
	private float angleCorrection;
	public float stressIncrease = 10;
	private RaycastHit info;
	public LayerMask environment;
	public Transform dashTransform;
	public Transform dashTransform2;
	public float fixedAimAngleCorrection = 90;

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
		inputManager = new InputManager (playerId, transform, angleCorrection);
		if (inputManager.HasMouse ()) {
			Cursor.visible = false;
			Cursor.SetCursor (crosshairCursor, cursorHotSpot, CursorMode.Auto);
		}
		ResetStatus ();
		StartCoroutine (RefillStress ());
	}

	/// <summary>
	/// Updates the player instance.
	/// </summary>
	void Update ()
	{
		Debug.Log ("underAttack: " + underAttack + " - stopped: " + stopped);
		if (!underAttack) {
			if (!stopped) {
				CheckingEnvironment ();
				Move ();
				Aim ();
				Shoot ();
				Melee ();
				StartDashing ();
				DropWeapon ();
				if (playerId == 1) {
					//Debug.Log ("Weapon position: " + weapon.transform.position);
				}
			}
		}
	}

	private void DropWeapon ()
	{
		if (inputManager.Drop ()) {
			Debug.Log (playerId + " drop weapon");
			//Destroy (weapon);
			weapon.transform.SetParent (null);
			weapon.transform.position = transform.position;
			//Debug.Log ("Assigned weapon position: " + weapon.transform.position);
		}
	}

	private void PickWeapon ()
	{
		if (inputManager.Pick ()) {
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
		rb.MovePosition (rb.position + inputManager.GetMoveVector () * speed * Time.deltaTime);
		//ani.SetFloat ("Movement", horizontalMovement);
		//ani.SetFloat ("Movement", verticalMovement);
	}

	/// <summary>
	/// Aim player.
	/// </summary>
	private void Aim ()
	{
		Vector3 aimVector;
		float aimAngle;

		aimVector = inputManager.GetAimVector ();
		aimAngle = inputManager.GetAimAngle ();
		if (aimVector != Vector3.zero) {
			transform.forward = Vector3.Normalize (aimVector);
		} else if (aimAngle != 0) {
			transform.rotation = Quaternion.AngleAxis (aimAngle, Vector3.up); 
		}
	}

	/// <summary>
	/// Executes a melee attack against other connected players
	/// </summary>
	private void Melee ()
	{
		PlayerControl control;

		if ((otherConnectedPlayers.Count > 0) && inputManager.Melee ()) {
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
		} else if ((ammo > 0) && inputManager.Shoot ()) {
			bullet = Instantiate (bulletPrefab) as GameObject;
			bullet.transform.rotation = weaponSpawnpoint.rotation;
			bullet.transform.position = weaponSpawnpoint.position;
			bullet.tag = bulletTagPrefix + gameObject.tag;
			bulletRigidbody = bullet.GetComponent<Rigidbody> ();
			bulletRigidbody.AddForce (bullet.transform.forward * bulletInitialForce, ForceMode.Impulse);
			Destroy (bullet, bulletLifeTime);
			ammo--;
			stress += weaponStressDamage;
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
		if (collision.gameObject.tag.StartsWith (playerTagPrefix)) {
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
		if (collision.gameObject.tag.StartsWith (playerTagPrefix)) {
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
		if (other.gameObject.name.StartsWith (bulletTagPrefix)) {
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
		if (life <= 0) {
			life = 0;
			GameManager.instance.PlayerKilled (GetPlayerId (gameObject.tag), GetPlayerId (killerTag));
		}
		UpdateUI ();
	}

	/// <summary>
	/// Gets the player identifier from player tag.
	/// </summary>
	/// <returns>The player identifier.</returns>
	/// <param name="playerTag">Player tag. Manage both player tag and bullet player tag</param>
	private int GetPlayerId (string playerTag)
	{
		if (playerTag.StartsWith (bulletTagPrefix)) {
			playerTag = playerTag.Substring (bulletTagPrefix.Length);
		}
		return int.Parse (playerTag.Substring (playerTagPrefix.Length));
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
	/// Adds the stress.
	/// </summary>
	/// <param name="stressAdd">Stress to add.</param>
	public void AddStress (float stressToAdd)
	{
		stress = Mathf.Clamp (stress + stressToAdd, 0, maxStressValue);
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

	/// <summary>
	/// Refills the stress.
	/// </summary>
	/// <returns>The stress.</returns>
	IEnumerator RefillStress ()
	{
		while (stress < 100) {
			yield return new WaitForSeconds (timerToRefillStress);
			stress = Mathf.Clamp (stress - stressDecreaseFactor, 0, maxStressValue);
			UpdateUI ();
		}
	}

	/// <summary>
	/// Starts dashing.
	/// </summary>
	private void StartDashing ()
	{
		
		if (inputManager.Dash ()) {
			if (!isDashing && (stress <= maxStressValue - stressIncrease)) {
				isDashing = true;
				StartCoroutine (Dashing ());
			}
		}
	}

	/// <summary>
	/// Checkings the environment.
	/// </summary>
	/// <returns><c>true</c>, if environment was checkinged, <c>false</c> otherwise.</returns>
	private bool CheckingEnvironment ()
	{
		Vector3 ray = transform.position;

		Debug.DrawRay (ray, new Vector3 (Input.GetAxis ("Horizontal1"), 0, Input.GetAxis ("Vertical1")));

		if (Physics.Raycast (ray, new Vector3 (Input.GetAxis ("Horizontal1"), 0, Input.GetAxis ("Vertical1")), out info, dashDistance, environment))
			return true;

		return false;
	}

	private IEnumerator Dashing ()
	{
		Vector3 newPosition = Vector3.zero;

		if (!CheckingEnvironment ()) {
			dashTransform.localPosition = new Vector3 (dashTransform.localPosition.x + (dashDistance * Input.GetAxis ("Horizontal1")), dashTransform.localPosition.y, dashTransform.localPosition.z + (dashDistance * Input.GetAxis ("Vertical1")));
			newPosition = new Vector3 (dashTransform.position.x, dashTransform.position.y, dashTransform.position.z);
		} else if (CheckingEnvironment ()) {
			dashTransform.position = new Vector3 (info.point.x, dashTransform.position.y, info.point.z);
			dashTransform.localPosition = new Vector3 (dashTransform.localPosition.x, dashTransform.localPosition.y, dashTransform.localPosition.z);
			newPosition = new Vector3 (dashTransform.position.x, dashTransform.position.y, dashTransform.position.z);
		}
		while (Vector3.Distance (transform.position, newPosition) > 1) {
			transform.position = Vector3.Lerp (transform.position, newPosition, 0.2f);
			yield return null;
		}
		dashTransform.localPosition = dashTransform2.localPosition;
		AddStress (stressIncrease);
		yield return new WaitForSeconds (dashTime);
		isDashing = false;
	}

	/// <summary>
	/// Sets the player identifier.
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void SetPlayerId (int playerId)
	{
		this.playerId = playerId;
		gameObject.tag = playerTagPrefix + playerId;
	}

	public void SetAngleCorrection (float angleCorrection)
	{
		this.angleCorrection = angleCorrection;
	}
}
