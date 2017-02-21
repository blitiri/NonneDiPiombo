using UnityEngine;
using System.Collections;

public class Player2Control : MonoBehaviour {
	public float speed;
	private Rigidbody rb;

	void Awake(){
		rb = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.J)) {
			rb.AddForce (-Vector3.right * speed * Time.deltaTime, ForceMode.Impulse);
			//childRB.transform.Translate (-Vector3.right * 0.02f * Time.deltaTime,Space.Self);

		} else if (Input.GetKey (KeyCode.L)) {
			rb.AddForce (Vector3.right * speed * Time.deltaTime, ForceMode.Impulse);
			//childRB.transform.Translate (Vector3.right * 0.02f * Time.deltaTime,Space.Self);


		}
	}
}
