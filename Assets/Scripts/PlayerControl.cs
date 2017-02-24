using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	private float horizontalMovement;
	private float verticalMovement;
	private Rigidbody rb;
	public float speed;
	public float rotSpeed;

	public int playerNumber;
	private string moveAxisName;
	private string turnAxisName;
	private Animator ani;
	void Awake(){
		rb = GetComponent<Rigidbody> ();
		ani = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		horizontalMovement = Input.GetAxis ("Horizontal" + playerNumber);
		verticalMovement = Input.GetAxis ("Vertical" + playerNumber);
		Move ();
	}

	public void Move()
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


}
