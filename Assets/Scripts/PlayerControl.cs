using UnityEngine;
using System.Collections;

/// <summary>
/// Player control.
/// </summary>
public class PlayerControl : MonoBehaviour
{

	public bool stopInputPlayer = false;
	private bool underAttack;
	private bool stopped;
	private bool isDashing;
	public bool isObstacle = false;
	public bool pickedAnotherWeapon = false;
    public bool dead = false;
	public bool isShooting;

	public int startingAmmo = 20;
	public int startingLife = 100;
	public int startingStress = 0;
	public int maxLifeValue = 100;
	public int maxStressValue = 100;
	public int bulletDamage = 5;
	public int playerId;
	private int ammo;
	private float life;

	public float speed = 2;
	public float rotSpeed = 2;
	public float bulletLifeTime = 2;
	public float bulletInitialForce = 2;
	public float underAttackInactivityTime = 2;
	public float maxTimeToShoot = 0.5f;
	public float dashWallDistance = 1.5f;
	public float stressDecreaseFactor;
	public float timeToDiminishStress;
	public float weaponStressDamage;
	private float horizontalMovement;
	private float verticalMovement;
	private float timerToShoot;
	public float defaultRatio = 0.8f;
	public int defaultDamage = 5;
	[Range (0, 10)]
	public float dashTime;
	float dashRecordedTime = 0;
	[Range (0, 10)]
	public float dashDistance;
	private float stress;
	private float angleCorrection;
	public float stressIncrease = 10;
	public float fixedAimAngleCorrection = 90;
	public float dashSpeed = 10;
	public float dashLength = 5;
	[Range (0, 100)]
	public float playerObstacleDistanceLimit;

	private const string bulletTagPrefix = "Bullet";
	private const string playerTagPrefix = "Player";
	private string selectedWeapon;
	private string defaultweapon = "Revolver";

	public LayerMask environment;
	public Texture2D crosshairCursor;
	public Vector2 cursorHotSpot = new Vector2 (16, 16);

    //public Color fromEmissionColor;
    public Color toEmissionColor;

    private Material playerMat;

    public Transform bulletSpawnPoint;

    private MeshRenderer meshPlayer;
	private MeshRenderer revolverMeshRenderer;

    private Rigidbody rb;

	private Animator ani;

	public GameObject bulletPrefab;
	public GameObject revolver;
	public GameObject uzi;
	public GameObject aimTargetPrefab;
	private GameObject aimTarget;

	private InputManager inputManager;

	private IList otherConnectedPlayers;

	private Hashtable weapons;

	public float blinkColorTime = 2;

	private OutlineShaderApply shaderApply;

	public Shader outlineShader;
	public Shader standardShader;


	//   [Range(0,1)]
	//   public float dashSpeed = 0.1f;


	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		 
		shaderApply = new OutlineShaderApply ();
		revolverMeshRenderer = revolver.GetComponent<MeshRenderer> ();
		weapons = new Hashtable ();
		foreach (Transform child in transform) {
			weapons.Add (child.gameObject.tag, child.gameObject);
		}
		SetActiveWeapons (defaultweapon);

		if (aimTargetPrefab != null) {
			aimTarget = Instantiate (aimTargetPrefab) as GameObject;
		}
		rb = GetComponent<Rigidbody> ();
		ani = GetComponent<Animator> ();
        meshPlayer = GetComponent<MeshRenderer>();
        playerMat = meshPlayer.material;
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		inputManager = new InputManager (playerId, transform, angleCorrection);
		if (inputManager.HasMouse ()) {
			Cursor.SetCursor (crosshairCursor, cursorHotSpot, CursorMode.Auto);

		}
		ResetStatus ();
		StartCoroutine (DiminishStress ());
	}

	/// <summary>
	/// Updates the player instance.
	/// </summary>
	void Update ()
	{

		transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
		//Debug.Log ("underAttack: " + underAttack + " - stopped: " + stopped);
		if (!underAttack) {
			if (!stopped && stopInputPlayer == false) {
				Move ();
				DashManaging ();
				//Debug.Log(inputManager.GetMoveVector());
				Aim ();
				Shoot ();
				Melee ();
				DropWeapon ();
				//Debug.Log("isDashing: " + isDashing);

				//Assegna Shader Outline su arma attiva
				shaderApply.ShaderApply (revolverMeshRenderer, revolver.transform.position, outlineShader, standardShader);

				//Munizioni Infite Se revolver
				if (selectedWeapon == "Revolver") {
					UIManager.instance.ammoCounters [playerId].text = "--";
				} else {
					UpdateUI ();
				}
			}
		}
	}

	

	/// <summary>
	/// Determines whether player is fit as a fiddle.
	/// </summary>
	/// <returns><c>true</c> if player is fit as a fiddle; otherwise, <c>false</c>.</returns>
	public bool IsFitAsAFiddle ()
	{
		return life >= maxLifeValue;
	}

	private void DropWeapon ()
	{
		if (inputManager.Drop () || pickedAnotherWeapon || IsDead () || IsCollapsed () || ammo <= 0) {
            if (selectedWeapon == "Uzi")
            {
                GameObject droppedUzi = Instantiate(uzi, transform.position, Quaternion.identity) as GameObject;
                WeaponManager droppedUziMan = droppedUzi.GetComponent<WeaponManager>();
                droppedUziMan.ammoMagazine = ammo;
                droppedUziMan.ratioOfFire = maxTimeToShoot;
                ammo = 100;
            }

             SetActiveWeapons(defaultweapon);
             maxTimeToShoot = defaultRatio;
             //bulletDamage = defaultDamage;
            
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

		if (Time.timeScale > 0) {
			aimVector = inputManager.GetAimVector ();

			if (aimVector != Vector3.zero) {

				if (inputManager.HasMouse ()) {
					//Correzione aimvector per modello girato.
					aimVector -= transform.position;
					//aimVector = Quaternion.Euler (new Vector3 (0, 87, 0)) * aimVector;
					aimVector += transform.position;
					transform.LookAt (aimVector);

					if (aimTarget != null) {
						aimTarget.transform.position = new Vector3 (aimVector.x, transform.position.y, aimVector.z);// + transform.position;
						//Debug.Log ("aimVector=" + aimVector);
					}
				} else {
					transform.forward = Vector3.Normalize (aimVector);
				}
			}

			/*aimAngle = inputManager.GetAimAngle ();
		if (aimVector != Vector3.zero) {
			transform.forward = Vector3.Normalize (aimVector);
		} else if (aimAngle != 0) {
			transform.rotation = Quaternion.AngleAxis (aimAngle, Vector3.up); 
		}*/
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
	public void Shoot ()
	{
		GameObject bullet;
		Rigidbody bulletRigidbody;

		if (timerToShoot < maxTimeToShoot) {
			timerToShoot += Time.deltaTime;
		} else if ((ammo > 0) && inputManager.Shoot ()) {
			isShooting = true;
			bullet = Instantiate (bulletPrefab) as GameObject;
			bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
			bullet.transform.position = bulletSpawnPoint.position;
			bullet.tag = bulletTagPrefix + gameObject.tag;
			bulletRigidbody = bullet.GetComponent<Rigidbody> ();
			bulletRigidbody.AddForce (bulletSpawnPoint.transform.up * bulletInitialForce, ForceMode.Impulse);
			Destroy (bullet, bulletLifeTime);
			if (selectedWeapon != "Revolver") {
				ammo--;
			}
			stress += weaponStressDamage;
			timerToShoot = 0.0f;
			UpdateUI ();
		} else {
			isShooting = false;
		}
	}

	/// <summary>
	/// Detects a collision enter with another player
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter (Collision collision)
	{
		//        Debug.Log ("Collision enter detected: " + collision.gameObject.tag);
		if (collision.gameObject.tag.StartsWith (playerTagPrefix)) {
			otherConnectedPlayers.Add (collision.gameObject);
		}
		if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9 || collision.gameObject.layer == 10) {
			isDashing = false;
			//            Debug.Log("SSSS");
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
		//Debug.Log ("Tag:" + other.gameObject.tag + " Prefix:" + bulletTagPrefix + " Result:" +  other.gameObject.tag.StartsWith (bulletTagPrefix));

		if (other.gameObject.tag.StartsWith (bulletTagPrefix)) {
			if (!other.gameObject.tag.EndsWith (playerId.ToString ())) {
				//Debug.Log ("Trigger detected: " + other.gameObject.tag);
				AddDamage (bulletDamage, other.gameObject.tag);
	
			}
		}
	}

    /// <summary>
	/// Determines whether player is dead.
	/// </summary>
	/// <returns><c>true</c> if player is dead; otherwise, <c>false</c>.</returns>
	public bool IsDead()
    {
        return life <= 0;
    }

    /// <summary>
    /// Detects a trigger enter with a weapon
    /// </summary>
    /// <param name="other">Collider.</param>
    void OnTriggerStay (Collider other)
	{
		if (inputManager.Pick ()) {

			if (selectedWeapon != "Revolver") {
				pickedAnotherWeapon = true;
				DropWeapon ();
                pickedAnotherWeapon = false;
            }
            WeaponManager pickedWeaponMan = other.gameObject.GetComponent<WeaponManager> ();
			ammo = pickedWeaponMan.ammoMagazine;
			maxTimeToShoot = pickedWeaponMan.ratioOfFire;
			foreach (Transform child in transform) {
				if (other.tag == child.tag) {
					SetActiveWeapons (other.tag);
					Destroy (other.gameObject);
				}
			}
		}

	}

	/// <summary>
	/// Adds the damage.
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <param name="killerTag">Killer tag.</param>
	private void AddDamage (int damage, string killerTag)
	{
        if(dead==false)
        {
            life -= bulletDamage;
            if (IsDead())
            {
                life = 0;
                GameManager.instance.PlayerKilled(GetPlayerId(gameObject.tag), GetPlayerId(killerTag));
            }
            UpdateUI();
            StopCoroutine(BounceColor(toEmissionColor));
            StartCoroutine(BounceColor(toEmissionColor));
        }
	}

	/// <summary>
	/// Gets the player identifier from player tag.
	/// </summary>
	/// <returns>The player identifier.</returns>
	/// <param name="tag">Tag whence retrieve player identifier. Manage both player tag and bullet player tag</param>
	private int GetPlayerId (string tag)
	{
		// Retrieve player ID from bullet tag
		if (tag.StartsWith (bulletTagPrefix)) {
			tag = tag.Substring (bulletTagPrefix.Length);
		}
		return int.Parse (tag.Substring (playerTagPrefix.Length));
	}

	/// <summary>
	/// Adds the life to the player.
	/// </summary>
	/// <param name="life">Life.</param>
	public void AddLife (int life)
	{
		this.life = Mathf.Clamp (this.life + life, 0, maxLifeValue);
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
	/// Determines whether player is stressed.
	/// </summary>
	/// <returns><c>true</c> if player is stressed; otherwise, <c>false</c>.</returns>
	public bool IsStressed ()
	{
		return stress > 0;
	}

	/// <summary>
	/// Determines whether player is collapsed.
	/// </summary>
	/// <returns><c>true</c> if player is collapsed; otherwise, <c>false</c>.</returns>
	public bool IsCollapsed ()
	{
		return stress >= maxStressValue;
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
		if (selectedWeapon == "Revolver") {
			UIManager.instance.ammoCounters [playerId].text = ("--");
		} else {
			UIManager.instance.SetAmmo (ammo, playerId);
		}
		// Cast to float is required to avoid an integer division
		UIManager.instance.SetLife (life / maxLifeValue, playerId);
		// Cast to float is required to avoid an integer division

		UIManager.instance.SetStress (stress / maxStressValue, playerId);

	}

	/// <summary>
	/// Refills the stress.
	/// </summary>
	/// <returns>The stress.</returns>
	IEnumerator DiminishStress ()
	{
		while (stress < 100) {
			yield return new WaitForSeconds (timeToDiminishStress);
				if (!isShooting) {
					stress = Mathf.Clamp (stress - stressDecreaseFactor, 0, maxStressValue);
					UpdateUI ();
				}
		}
	}

	/// <summary>
	/// Starts dashing.
	/// </summary>
	private void DashManaging ()
	{
		Vector3 moveVector = inputManager.GetMoveVector ();
		//Debug.Log (moveVector.magnitude);
		ObstacleChecking (moveVector);
		isObstacle = ObstacleChecking (moveVector);
		//        Debug.Log(obstacleInfo.transform);
		//        Debug.Log(ObstacleChecking(moveVector));
		//CorrectingDashDestinationRotation(dashTransform);
		//CorrectingDashDestinationRotation(dashTransform2);
		if (inputManager.Dash ()) {
			if (!isObstacle && !isDashing && dashTime <= Time.time - dashRecordedTime) {
				StartCoroutine (Dashing (moveVector));
			}
		}
	}

	/// <summary>
	/// Checkings the environment.
	/// </summary>
	/// <returns><c>true</c>, if environment was checkinged, <c>false</c> otherwise.</returns>
	private bool ObstacleChecking (Vector3 moveVector)
	{
		float rayy = 0.1f;
		bool result = false;
		Vector3 ray = new Vector3 (transform.position.x, rayy, transform.position.z);
		Vector3 rayDirection = moveVector;
		//        RaycastHit obstacleInfo;

		Debug.DrawRay (ray, rayDirection, Color.blue);

		if (Physics.Raycast (ray, rayDirection, playerObstacleDistanceLimit, environment)) {
			result = true;
		} else {
			result = false;
		}
		//Debug.Log("result: " + result);
		return result;
	}

	private IEnumerator Dashing (Vector3 moveVector)
	{
		Vector3 newPosition = Vector3.zero;
		//float scaleProp;
		float dashDone = 0;

		isDashing = true;

		while (dashDone < dashLength && isDashing && !isObstacle) {
			//Debug.Log ("isObstacle" + isObstacle);
			if (moveVector.magnitude > 0) {
				transform.localPosition += dashSpeed * Time.deltaTime * moveVector; //= new Vector3(transform.localPosition.x + dashSpeed * Time.deltaTime * moveVector.x, transform.localPosition.y, transform.localPosition.z + dashSpeed * Time.deltaTime * moveVector.z);
			} else {
				transform.localPosition += dashSpeed * Time.deltaTime * transform.forward;
			}
			//                Debug.Log(moveVector);
			dashDone += dashSpeed * Time.deltaTime;
			yield return null;
			//                DashDone += moveVector.z > 0 ? dashSpeed * Time.deltaTime * moveVector.z : moveVector.x > 0 ? dashSpeed * Time.deltaTime * moveVector.x : dashSpeed * Time.deltaTime;
		}
		AddStress (stressIncrease);
		isDashing = false;
		dashRecordedTime = Time.time;
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

	private void SetActiveWeapons (string weapon)
	{
		if (selectedWeapon != null) {
			((GameObject)weapons [selectedWeapon]).SetActive (false);
		}
		((GameObject)weapons [weapon]).SetActive (true);
		selectedWeapon = weapon;
	}

    private IEnumerator BounceColor(Color toColor)
    {
        float timer = 0;
        float timeLimit = 1;

        while (timer < timeLimit)
        {
            timer += Time.deltaTime;
			float colorGradient = Mathf.PingPong(timer * blinkColorTime, 1);
            Color bouncingColor = toColor * Mathf.LinearToGammaSpace(colorGradient);
           playerMat.SetColor("_EmissionColor", bouncingColor);

          //  Debug.Log(timer);
            yield return new WaitForEndOfFrame();
        }
        playerMat.SetColor("_EmissionColor", Color.black);
    }
}
