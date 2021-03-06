﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Player control.
/// </summary>
public class PlayerControl : MonoBehaviour
{
	/// <summary>
	/// The player identifier.
	/// </summary>
	public int playerId;
    /// <summary>
    /// The controller which can command this character.
    /// </summary>
    public int controllerId;
    ///// <summary>
    ///// The player controlling this character.
    ///// </summary>
    //public Rewired.Player myPlayer;

	/// <summary>
	///Variables Managing player Life 
	/// </summary>
	private float life;
	public int startingLife = 100;
	public int maxLifeValue = 100;

	/// <summary>
	///Variables Managing player Stress 
	/// </summary>
	public int startingStress = 0;
	private float stress;
	public int maxStressValue = 100;
	public float stressIncrease = 10;
	public float stressDecreaseFactor;
	public float timeToDiminishStress;

	/// <summary>
	///  respawn
	/// </summary>
	public bool isDead = false;
	public GameObject brokenVersion;
	/// <summary>
	/// Player Movement 
	/// </summary>
	public float speedMod;
	public float speed = 2;
	public float rotSpeed = 2;
	private float horizontalMovement;
	private float verticalMovement;
	private float angleCorrection;
	private bool stopped;
	public bool stopInputPlayer = false;

	/// <summary>
	/// Player Dash
	/// </summary>
	public bool isObstacle = false;
	private bool isDashing;
	public float dashWallDistance = 1.5f;
	[Range (0, 10)]
	public float dashTime;
	float dashRecordedTime = 0;
	[Range (0, 10)]
	public float dashDistance;
	public float dashSpeed = 10;
	public float dashLength = 5;
	[Range (0, 100)]
	public float playerObstacleDistanceLimit;
	public LayerMask environment;

    /// <summary>
    /// Player Tagging
    /// </summary>
    private const string bulletTagPrefix = "Bullet";
	private const string playerTagPrefix = "Player";

	/// <summary>
	/// Components
	/// </summary>
	public Texture2D crosshairCursor;
	public Vector2 cursorHotSpot = new Vector2 (16, 16);
	public Vector3 moveVector;
	private Material playerMat;
	private MeshRenderer meshPlayer;
	private MeshRenderer revolverMeshRenderer;
	private Rigidbody playerRigidbody;
	public GameObject bulletPrefab;
	private InputManager inputManager;
	// public Animator stressAnimation;

	/// <summary>
	/// weapon
	/// </summary>
	private float timerToShoot;
	private WeaponControl weapon;

	/// <summary>
	/// VFXs
	/// </summary>
	public GameObject bloodPrefab;
	public float bloodDuration;
    public GameObject dashParticle;
    private ParticleSystem dashVFX;
    private ParticleSystem.MainModule main;

    /// <summary>
    /// The ability.
    /// </summary>
    public Abilities ability;

	public bool isImmortal;
	public float immortalTime = 0.5f;

	/// <summary>
	/// Heart variables.
	/// </summary>
	public Transform heartPos;
	public GameObject heartPrefab;
	private GameObject heart;
	private UITweener heartScale;
	private TweenRotation heartRotation;
	public float heartBit = 30;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		weapon = GetComponentInChildren<WeaponControl> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		meshPlayer = GetComponent<MeshRenderer> ();
		playerMat = meshPlayer.material;
        dashVFX = dashParticle.GetComponentInChildren<ParticleSystem>();
        main = dashVFX.main;
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		timerToShoot = weapon.ratioOfFire;
		inputManager = new InputManager (controllerId, transform, angleCorrection);
		if (inputManager.HasMouse ()) {
			Cursor.SetCursor (crosshairCursor, cursorHotSpot, CursorMode.Auto);
		}
        heart = Instantiate(heartPrefab, heartPos.position, Quaternion.identity);
        heart.GetComponentInChildren<UISprite>().SetAnchor(heartPos);
        heartScale = heart.GetComponent<TweenScale>().GetComponent<UITweener>();
        ResetStatus ();
		StartCoroutine (DiminishStress ());
        //controllerId = myPlayer.id;
	}


	void Update ()
	{
		StressHeart ();
	}

	/// <summary>
	/// Updates the player instance.
	/// </summary>
	void FixedUpdate ()
	{
		if (!GameManager.instance.IsPaused ()) {
			transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

			if (!stopped && !stopInputPlayer) {
				Move ();
				moveVector = inputManager.GetMoveVector ();
				DashManaging ();
				Aim ();

				if (timerToShoot < weapon.ratioOfFire) {
					timerToShoot += Time.deltaTime;
				} else if (inputManager.Shoot ()) {
					weapon.Shoot (this.gameObject.tag);
					//UpdateUI ();
					timerToShoot = 0.0f;
				}

				//Assegna Shader Outline su arma attiva
				//			shaderApply.ShaderApply (revolverMeshRenderer, revolver.transform.position, outlineShader, standardShader);
			}
		}
	}

	/// <summary>
	/// Resets the player status.
	/// </summary>
	public void ResetStatus ()
	{
        
		isDead = false;
		stress = startingStress;
		stopped = false;
		//UpdateUI ();
	}

	/// <summary>
	/// Moves the player.
	/// </summary>
	private void Move ()
	{
		playerRigidbody.MovePosition (playerRigidbody.position + inputManager.GetMoveVector () * (speed + speedMod) * Time.deltaTime);
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
					aimVector += transform.position;
					transform.LookAt (aimVector);

				} else {
					transform.forward = Vector3.Normalize (aimVector);
				}
			}
		}
	}

	public void ExplodeCharacter ()
	{
		GameObject brokenGran = Instantiate (brokenVersion, transform.position, transform.rotation) as GameObject;
	}

	/// <summary>
	/// Detects a trigger enter with a weapon
	/// </summary>
	/// <param name="other">Collider.</param>
	void OnCollisionEnter (Collision other)
	{
		int playertagIndex;


		playertagIndex = Utility.GetPlayerIndex (this.gameObject.tag);

		if (other.gameObject.layer == 8 || other.gameObject.layer == 9 || other.gameObject.layer == 10) {
			isDashing = false;
			playerRigidbody.velocity = Vector3.zero;
		}

		if (other.gameObject.tag.StartsWith ("Bullet") && !isImmortal) {
			
			int playerKillerId;
			playerKillerId = Utility.GetPlayerIndexFromBullet (other.gameObject.tag);

			if (playertagIndex != playerKillerId) {
                playerRigidbody.velocity = Vector3.zero;
                RespawnOnTrigger (other, playerKillerId);
			}
		}    
	}

	public void RespawnOnTrigger (Collision other, int playerKillerId)
	{
        StopCoroutine("Dashing");
		//playerRigidbody.velocity = Vector3.zero;
		isImmortal = true;
		isDead = true;
		ExplodeCharacter ();
		GameManager.instance.PlayerKilled (playerId, playerKillerId);
		GameManager.instance.CheckRespawnPlayers ();
	}

	/// <summary>
	/// Adds the stress.
	/// </summary>
	/// <param name="stressAdd">Stress to add.</param>
	public void AddStress (float stressToAdd)
	{
		stress = Mathf.Clamp (stress + stressToAdd, 0, maxStressValue);
		//UpdateUI ();
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
	/// Refills the stress.
	/// </summary>
	/// <returns>The stress.</returns>
	IEnumerator DiminishStress ()
	{
		while (stress < 100) {
			yield return new WaitForSeconds (timeToDiminishStress);
			stress = Mathf.Clamp (stress - stressDecreaseFactor, 0, maxStressValue);
			//UpdateUI ();
		}
	}

	/// <summary>
	/// Starts dashing.
	/// </summary>
	private void DashManaging ()
	{
		float timer = 5.0f;
		ObstacleChecking (moveVector);
		isObstacle = ObstacleChecking (moveVector);
		if (inputManager.Dash ()) {
			if (!isObstacle && !isDashing && dashTime <= Time.time - dashRecordedTime) {
                StartCoroutine (Dashing (moveVector));
				InitAbility (ability);
				StartCoroutine ("Ability", timer);
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
		

		Debug.DrawRay (ray, rayDirection, Color.blue);

		if (Physics.Raycast (ray, rayDirection, playerObstacleDistanceLimit, environment)) {
			result = true;
		} else {
			result = false;
		}
		
		return result;
	}

	private IEnumerator Dashing (Vector3 moveVector)
	{
		Vector3 newPosition = Vector3.zero;

        float dashDone = 0;

		isDashing = true;

		while (dashDone < dashLength && isDashing && !isObstacle && !stopInputPlayer) {
			if (moveVector.magnitude > 0) {
				transform.localPosition += dashSpeed * Time.deltaTime * moveVector;
                if (dashParticle != null)
                {
                    dashParticle.SetActive(true);
                    main.startColor = Configuration.instance.playersColors[playerId];
                }
            } else {
				transform.localPosition += dashSpeed * Time.deltaTime * transform.forward;
                if (dashParticle != null)
                {
                    dashParticle.SetActive(true);
                    main.startColor = Configuration.instance.playersColors[playerId];
                }
            }

			dashDone += dashSpeed * Time.deltaTime;
			yield return null;
            dashParticle.SetActive(false);
        }
		AddStress (stressIncrease);
        isDashing = false;
        dashRecordedTime = Time.time;
        GameManager.instance.CheckRespawnPlayers();
    }

	/// <summary>
	/// Inits the ability.
	/// </summary>
	/// <param name="ability">Ability.</param>
	private void InitAbility (Abilities ability)
	{
		if (ability != null) {
			ability.SetPlayer (gameObject, playerId);
		}
	}

	IEnumerator Ability (float timer)
	{
		ability.OnAbilityActivation ();
		foreach (Transform child in transform) {
			if (child.tag == "Nos") {
				child.gameObject.SetActive (true);
			}
		}
        
		yield return new WaitForSeconds (timer);

		foreach (Transform child in transform) {
			if (child.tag == "Nos") {
				child.gameObject.SetActive (false);
			}
		}

		speedMod = 0;
	}

	/// <summary>
	/// Sets the player identifier.
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void SetPlayerId (int playerId)
	{
		this.playerId = playerId;
		gameObject.tag = Utility.GetPlayerId (playerId);
	}

	public void SetAngleCorrection (float angleCorrection)
	{
		this.angleCorrection = angleCorrection;
	}

	/*private void UpdateUI ()
	{
		LevelUIManager.instance.SetStress (stress / maxStressValue, playerId);
	}*/

	public void StressHeart ()
	{
		if (stress >= maxStressValue / 2) {
			heart.SetActive (true);
			heartScale.duration = 0.4f;
		} 
		if (stress >= (maxStressValue - heartBit)) {
			heartScale.duration = 0.1f;
		} else if (stress < maxStressValue / 2) {
			heart.SetActive (false);
		}
	}
}