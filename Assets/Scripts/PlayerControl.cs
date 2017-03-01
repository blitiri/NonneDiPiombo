using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public int ammo = 20;
	private const float maxLifeValue = 100;
	private float horizontalMovement;
	private float verticalMovement;
	private Rigidbody rb;
	public float speed;
	public float rotSpeed;
	public GameObject bulletPrefab;
	public Transform gunSpawnpoint;
	public float bulletLifeTime = 2;
	public float bulletInitialForce = 2;
	public float underAttackInactivityTime;
	public int life = 100;
	public int stress = 0;
	public int bulletDamage = 5;
	public bool stopped = false;
	public bool underAttack;
	public IList otherConnectedPlayers;
	public int playerNumber;
	private Animator ani;
	private float shot;
	private float melee;
    private float timerToShoot;
    public float maxTimeToShoot=1.0f;
    

	void Awake ()
	{
		underAttack = false;
		rb = GetComponent<Rigidbody> ();
		ani = GetComponent<Animator> ();
		otherConnectedPlayers = new ArrayList ();
	}

	// Use this for initialization
	void Start ()
    {
	
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
                    control = otherPlayer.GetComponent<PlayerControl>();
                    control.Attacked(20);
            }
		}
	}

	public void Attacked (int damage)
	{
		underAttack = true;
		StartCoroutine (AttackAnimation (damage));
        underAttack = false;
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
		//underAttack = false;
	}

	private void Shot ()
	{
		GameObject bullet;
		Rigidbody bulletRigidbody;

        if(timerToShoot<maxTimeToShoot)
        {
            timerToShoot += Time.deltaTime;
        }
		else if (shot > 0 && ammo>0)
        {
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
		//Debug.Log ("Collision exit detected: " + collision.gameObject.tag);
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
//		UIManager.instance.setLife (life / maxLifeValue, playerNumber);
	}
}
