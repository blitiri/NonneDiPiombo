using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	private Rigidbody rb;

	public GameObject body;

	public GameObject[] foreArms;

	public GameObject[] cannons;

	public static InputController instance;

	public float movSpeed = 25.0f;
	private float runSpeed;
	public float rotSpeed = 15.0f;
	public float rotGatlinSpeed = 15.0f;
	public float rotShieldSpeed = 1.0f;
	public float rotRocketSpeed = 35.0f;

	public float gatlinShootForce = 15.0f;

	private float lastGatlinShoot = 0;
	private float lastRocketShoot = 0;

	public float gatlingShootingRate = 0.5f;
	public float rocketShootingRate = 0.5f;
	private float improvedrocketShootingRate;

	public float minClampGatlin = -45.0f;
	public float maxClampGatlin = 45.0f;

//	public string horizontalAxis;
//	public string horizontalAimAxis;
//
//	public string veritcalAxis;
//	public string veritcalAimAxis;
//
//	public string horizontalShieldAxis;
//	public string veritcalShieldAxis;
//
//	public string horizontalRocketAxis;
//	public string veritcalRocketAxis;
//
//	public string horizontalAimGatlinAxis;

	public GameObject bulletPrefab;
	public GameObject bulletBigPrefab;
	public Transform[] gatlinLeftSpawnPoint;
	public Transform[] gatlinRightSpawnPoint;
	public Transform[] rocketSpawnPoint;

	public bool disable;
	private bool shieldActive;
	private bool run;
	public bool isDead;

	public Animator anim;

	Choices choices;

	private int movement;
	private int gun ; 
	private int cannon;
	private int shield;
//	public string veritcalAimGatlinAxis;

	public AudioManager audioManager;

	void Awake() {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		instance = this;

		GameObject choicesGo = GameObject.FindGameObjectWithTag("Choices");
		choices = choicesGo.GetComponent<Choices>();
	}

	void Start () {

		runSpeed = movSpeed * 2.0f;

		isDead = false;

		improvedrocketShootingRate = rocketShootingRate/3;

		disable = false;

		transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);

		movement = choices.movements;

		gun = choices.gun;

		cannon = choices.cannon;

		shield = choices.shield;


//		horizontalAxis += gameObject.name;
//		veritcalAxis += gameObject.name;
//
//		horizontalAimAxis += gameObject.name;
//		veritcalAimAxis += gameObject.name;
//
//		horizontalShieldAxis += gameObject.transform.GetChild(1).name;
//		veritcalShieldAxis += gameObject.transform.GetChild(1).name;
//
//		horizontalRocketAxis += gameObject.transform.GetChild(2).name;
//		veritcalRocketAxis += gameObject.transform.GetChild(2).name;
//
//		horizontalAimGatlinAxis += gameObject.transform.GetChild(0).GetChild(0).name;
//		veritcalAimGatlinAxis += gameObject.transform.GetChild(0).GetChild(0).name;
	}
	
	// Update is called onceper frame
	void Update () {
	
		if(isDead) {
			if (GetButtonDown(0, "A") || GetButtonDown(1, "A") || GetButtonDown(2, "A") || GetButtonDown(3, "A"))
				Application.LoadLevel(1);
		}


		if(!disable) {

			anim.SetBool("Run" , run); 

			anim.SetBool("Dead" , GameController.instance.playerLife <= 0);

			if(GetAxis(movement	,"ShootIper") > 0.1f){
				float moveH = GetAxis(movement,"LeftRotationH") * runSpeed;
				float moveV = GetAxis(movement,"LeftRotationV") * runSpeed;
				anim.SetFloat("SpeedH" , moveH);
				anim.SetFloat("SpeedV" , moveV);
				Move(moveH, moveV);
				run = true;

				GameController.instance.energy -= 10f*Time.deltaTime;


			} else {
				float moveH = GetAxis(movement,"LeftRotationH") * movSpeed;
				float moveV = GetAxis(movement,"LeftRotationV") * movSpeed;
				anim.SetFloat("SpeedH" , moveH);
				anim.SetFloat("SpeedV" , moveV);
				run = false;

				Move(moveH, moveV);
				if(moveH>0||moveV>0)
					GameController.instance.energy -= 1.0f*Time.deltaTime;

			}

			float aimH = GetAxis(movement,"RightRotationH") * rotSpeed;
			float aimV = GetAxis(movement,"RightRotationV") * rotSpeed;
			Aim(aimH, aimV);

			float aimGatlinH = GetAxis(gun, "RightRotationH") * rotGatlinSpeed;
			RotateGatlinGun(aimGatlinH);

			bool isShooting = false;

			if(GetAxis(gun,"Shoot") > 0.1f) {
				isShooting = true;

				if(!audioManager.gatlinShoot.isPlaying)
					audioManager.gatlinShoot.Play();

				if(((Time.time - lastGatlinShoot) > gatlingShootingRate)) {
					lastGatlinShoot = Time.time;
					GatlinShoot();
				}
			} 


			if(GetAxis(gun,"ShootIper") > 0.1f) {
				isShooting = true;

				if(!audioManager.gatlinShoot.isPlaying)
					audioManager.gatlinShoot.Play();

				if(((Time.time - lastGatlinShoot) > gatlingShootingRate)) {
					lastGatlinShoot = Time.time;
					IperGatlinShoot();
				}
			} 


			if(! isShooting) {
				if(audioManager.gatlinShoot.isPlaying)
					audioManager.gatlinShoot.Stop();
			}

			float shieldH = GetAxis(shield, "RightRotationH") * rotShieldSpeed;
			float shieldV = GetAxis(shield,"RightRotationV") * rotShieldSpeed;
			Shield(shieldH, shieldV);

			if(GetButtonDown(shield, "Shoot")) {
				if(! audioManager.activeShield.isPlaying)
					audioManager.activeShield.Play ();
				EnableShield();
			} else {
				if(audioManager.activeShield.isPlaying)
					audioManager.activeShield.Stop ();
			}


			if(shieldActive)
				GameController.instance.energy -= 1.0f*Time.deltaTime;

			if(GetButtonDown(shield,"ShootIper")) {
				if(! audioManager.heal.isPlaying)
					audioManager.heal.Play ();
				GameController.instance.playerLife += 50.0f;
				GameController.instance.energy -= 10.0f;
			} else {
				if(audioManager.heal.isPlaying)
					audioManager.heal.Stop();
			}

			float rocketH = GetAxis(cannon, "RightRotationH") * rotRocketSpeed;
			float rocketV = GetAxis(cannon,"RightRotationV") * rotRocketSpeed;
			RocketsAim(rocketH, rocketV);

			bool isShootingCannon = false;

			if(GetAxis(cannon,"Shoot") > 0.1f) {

				isShootingCannon = true;

				if(!audioManager.cannonShoot.isPlaying)
					audioManager.cannonShoot.Play();

				if(((Time.time - lastRocketShoot) > rocketShootingRate)) {
					lastRocketShoot = Time.time;
					RocketShoot();
				}
			}

			if(GetAxis(cannon,"ShootIper") > 0.1f) {

				isShootingCannon = true;

				if(!audioManager.cannonShoot.isPlaying)
					audioManager.cannonShoot.Play();

				if(((Time.time - lastRocketShoot) > improvedrocketShootingRate)) {
					lastRocketShoot = Time.time;
					IperRocketShoot();
				}
			} 

			
			if(! isShootingCannon) {
				if(audioManager.cannonShoot.isPlaying)
					audioManager.cannonShoot.Stop();
			}

		}

		else {
			rb.velocity= new Vector3(0,0,0);
		}

		/*
		if(Input.GetAxis("Run") > 0.1f){
			float moveH = Input.GetAxis(horizontalAxis) * runSpeed;
			float moveV = Input.GetAxis(veritcalAxis) * runSpeed;
			Move(moveH, moveV);
		} else {
			float moveH = Input.GetAxis(horizontalAxis) * movSpeed;
			float moveV = Input.GetAxis(veritcalAxis) * movSpeed;
			Move(moveH, moveV);
		}

		float aimH = Input.GetAxis(horizontalAimAxis) * rotSpeed;
		float aimV = Input.GetAxis(veritcalAimAxis) * rotSpeed;
		Aim(aimH, aimV);

		float aimGatlinH = Input.GetAxis(horizontalAimGatlinAxis) * rotGatlinSpeed;
//		float aimGatlinV = Input.GetAxis(veritcalAimGatlinAxis) * rotSpeed;
		RotateGatlinGun(aimGatlinH);
		if(Input.GetAxis("GatlinFire") > 0.1f) {
			if(((Time.time - lastGatlinShoot) > gatlingShootingRate)) {
				lastGatlinShoot = Time.time;
				GatlinShoot();
			}
		}

		float shieldH = Input.GetAxis(horizontalShieldAxis) * rotShieldSpeed;
		float shieldV = Input.GetAxis(veritcalShieldAxis) * rotShieldSpeed;
		Shield(shieldH, shieldV);

		float rocketH = Input.GetAxis(horizontalRocketAxis) * rotRocketSpeed;
		float rocketV = Input.GetAxis(veritcalRocketAxis) * rotRocketSpeed;
		RocketsAim(rocketH, rocketV);

		if(Input.GetAxis("RocketFire") > 0.1f) {
			if(((Time.time - lastRocketShoot) > rocketShootingRate)) {
				lastRocketShoot = Time.time;
				RocketShoot();
			}
		} else if (Input.GetAxis("ImprovedRocketFire") > 0.1f) {
			if(((Time.time - lastRocketShoot) > improvedrocketShootingRate)) {
				lastRocketShoot = Time.time;
				RocketShoot();
			}
		}
		EnableBouncingShield();
		 */

	}

	bool GetButton(int player, string name) {
		return Rewired.ReInput.players.GetPlayer(player).GetButton(name);
	}

	bool GetButtonDown(int player, string name) 
	{
		return Rewired.ReInput.players.GetPlayer(player).GetButtonDown(name);
	}

	float GetAxis(int player, string name) {
		return Rewired.ReInput.players.GetPlayer(player).GetAxis(name);
	}

	void Aim(float xAxis , float zAxis) {
		Vector3 targetDir = new Vector3(xAxis , 0 , zAxis);
		
		if (targetDir.sqrMagnitude > 0.21f) {
			// rotation of the ship
			var rotation= Quaternion.LookRotation(targetDir, Vector3.up);
			transform.GetChild(1).rotation = Quaternion.Slerp(transform.GetChild(1).rotation, rotation, Time.deltaTime * rotSpeed);
			body.transform.rotation = transform.GetChild(1).rotation;
		}
	}

	void Move(float xAxis , float zAxis) {
		rb.velocity = new Vector3(xAxis , 0 , zAxis);

		Vector3 targetDir = new Vector3(xAxis , 0 , zAxis);
		
		if (targetDir.sqrMagnitude > 0.21f) {
			// rotation of the ship
			var rotation= Quaternion.LookRotation(targetDir, Vector3.up);
			transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, rotation, Time.deltaTime * rotSpeed);
			
		}

		//GameController.instance.energy -= 1.0f/Time.deltaTime;
	}


	void RotateGatlinGun(float yAxis ) {

		transform.GetChild(1).GetChild(0).localEulerAngles = new Vector3(0, yAxis, 0);
		foreArms[0].transform.localEulerAngles = new Vector3(0, yAxis, 0);
		transform.GetChild(1).GetChild(1).localEulerAngles = new Vector3(0, yAxis , 0);
		foreArms[1].transform.localEulerAngles = new Vector3(0, -yAxis , 0);
		yAxis = Mathf.Clamp(yAxis, -minClampGatlin, maxClampGatlin);

	}

	void GatlinShoot() {


		GameObject leftBullet = Instantiate (bulletPrefab , gatlinLeftSpawnPoint[0].position , gatlinLeftSpawnPoint[0].transform.rotation) as GameObject;
		Rigidbody bulletLeftRb = leftBullet.GetComponent<Rigidbody>();

		bulletLeftRb.AddForce(leftBullet.transform.up * gatlinShootForce);

		Destroy(leftBullet , 3.0f);

		GameObject rightBullet = Instantiate (bulletPrefab , gatlinRightSpawnPoint[0].position , gatlinRightSpawnPoint[0].transform.rotation) as GameObject;
		Rigidbody bulletRightRb = rightBullet.GetComponent<Rigidbody>();
		
		bulletRightRb.AddForce(rightBullet.transform.up * gatlinShootForce);
		
		Destroy(rightBullet , 3.0f);

		GameController.instance.energy -= 1.0f*gatlingShootingRate;
	
	}

	void IperGatlinShoot() {
		
		for(int i = 0 ; i < gatlinLeftSpawnPoint.Length; i++) {
			GameObject leftBullet = Instantiate (bulletPrefab , gatlinLeftSpawnPoint[i].position , gatlinLeftSpawnPoint[i].transform.rotation) as GameObject;
			Rigidbody bulletLeftRb = leftBullet.GetComponent<Rigidbody>();
			
			bulletLeftRb.AddForce(leftBullet.transform.up * gatlinShootForce);
			
			Destroy(leftBullet , 3.0f);


		}

		for(int i = 0 ; i < gatlinRightSpawnPoint.Length; i++) {
			GameObject rightBullet = Instantiate (bulletPrefab , gatlinRightSpawnPoint[i].position , gatlinRightSpawnPoint[i].transform.rotation) as GameObject;
			Rigidbody bulletRightRb = rightBullet.GetComponent<Rigidbody>();
			
			bulletRightRb.AddForce(rightBullet.transform.up * gatlinShootForce);
			
			Destroy(rightBullet , 3.0f);
		}
		GameController.instance.energy -= 10.0f*gatlingShootingRate;

		
	}

	void EnableShield() {
		if(shieldActive) {
			transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
			shieldActive = false;
		}
		else if (!shieldActive) {
         	transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true);
			shieldActive = true;
		}

	}

	void Shield(float xAxis , float zAxis) {

		Vector3 targetDir = new Vector3(xAxis , 0 , zAxis);

		if (targetDir.sqrMagnitude > 0.21f) {
			// rotation of the ship
			var rotation= Quaternion.LookRotation(targetDir, Vector3.up);
			transform.GetChild(2).rotation = Quaternion.Slerp(transform.GetChild(2).rotation, rotation, Time.deltaTime * rotShieldSpeed);
		}

	}

	void RocketsAim(float xAxis , float zAxis) {
		
		Vector3 targetDir = new Vector3(xAxis , 0 , zAxis);
		
		if (targetDir.sqrMagnitude > 0.21f) {
			// rotation of the ship
			var rotation= Quaternion.LookRotation(targetDir, Vector3.up);
			transform.GetChild(1).GetChild(2).rotation = Quaternion.Slerp(transform.GetChild(1).GetChild(2).rotation, rotation, Time.deltaTime * rotRocketSpeed);
			cannons[0].transform.rotation = transform.GetChild(1).GetChild(2).rotation;
			transform.GetChild(1).GetChild(3).rotation = Quaternion.Slerp(transform.GetChild(1).GetChild(3).rotation, rotation, Time.deltaTime * rotRocketSpeed);
			cannons[1].transform.rotation = transform.GetChild(1).GetChild(3).rotation;
		}
	}

	void RocketShoot() {

		for ( int i = 0; i < rocketSpawnPoint.Length ; i ++ ) {
			GameObject bullet = Instantiate (bulletBigPrefab , rocketSpawnPoint[i].position , rocketSpawnPoint[i].transform.rotation) as GameObject;
			Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
			
			bulletRb.AddForce(bullet.transform.up * gatlinShootForce);
			
			Destroy(bullet , 3.0f);

		}

		GameController.instance.energy -= 1.0f*rocketShootingRate;

	}

	void IperRocketShoot() {

		for ( int i = 0; i < rocketSpawnPoint.Length ; i ++ ) {

			GameObject bullet = Instantiate (bulletBigPrefab , rocketSpawnPoint[i].position , rocketSpawnPoint[i].transform.rotation) as GameObject;
			Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
			
			bulletRb.AddForce(bullet.transform.up * gatlinShootForce);
			
			Destroy(bullet , 3.0f);
		
		}

		GameController.instance.energy -= 10.0f*rocketShootingRate;
		
	}

//	void EnableBouncingShield() {
//
//		if(Input.GetAxis("BouncingShield") > 0.1f) {
//			canDeflect = true;
//		}
//		else {
//			canDeflect = false;
//		}
//	}

}
