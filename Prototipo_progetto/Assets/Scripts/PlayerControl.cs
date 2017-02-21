using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	private float horizontalMovement;
	private float verticalMovement;
	private Rigidbody rb;
	public float speed;
	public float rotSpeed;

	void Awake(){
		rb = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
	}

	public void Move()
	{
		//DA APPROFONDIRE MEGLIO
		horizontalMovement = Input.GetAxis ("Horizontal");
		verticalMovement = Input.GetAxis ("Vertical");
		//movement
		//rb.AddForce (transform.right * horizontalMovement * speed * Time.deltaTime,ForceMode.Impulse);
		//rb.AddForce (transform.forward * verticalMovement * speed * Time.deltaTime, ForceMode.Impulse);
		Vector3 movePositionRight = transform.forward * horizontalMovement * speed * Time.deltaTime;
		rb.MovePosition (rb.position + movePositionRight);

		Vector3 movePositionForward = transform.forward * verticalMovement * speed * Time.deltaTime;
		rb.MovePosition (rb.position + movePositionForward);

		//turning
		//da vedere Quaternion.LookRotation
		Quaternion turnRotationHorizontal = Quaternion.Euler (0f, horizontalMovement * rotSpeed * Time.deltaTime, 0f);
		rb.MoveRotation (rb.rotation * turnRotationHorizontal);

	}


}
