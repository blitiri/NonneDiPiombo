using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float dampTime = 0.2f;                
	public float screenEdgeBuffer = 4f;           
	public float minSize = 6.5f;       
    public Transform[] targets; 
	public Shader OutlineShader;

	private Camera mainCamera;                        
	private float zoomSpeed;                      
	private Vector3 moveVelocity;                 
	private Vector3 desiredPosition;              

	private void Awake ()
	{
		GameObject[] players;
		players = GameManager.instance.GetPlayers ();

		targets = new Transform[players.Length];
		for (int playerIndex=0; playerIndex < players.Length; playerIndex++) {
			targets [playerIndex] = players [playerIndex].transform;
		}
		mainCamera = GetComponentInChildren<Camera> ();
	}


	private void FixedUpdate ()
	{
		
		Move ();
		Zoom ();
		WallDetection ();
	}


	private void Move ()
	{
		
		FindAveragePosition ();


		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
	}


	private void FindAveragePosition ()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;


		for (int i = 0; i < targets.Length; i++)
		{
			
			if (targets [i].gameObject.activeSelf) {
				averagePos += targets [i].position;
				numTargets++;
			}
		}


		if (numTargets > 0)
			averagePos /= numTargets;


		averagePos.y = transform.position.y;


		desiredPosition = averagePos;
	}


	private void Zoom ()
	{
		
		float requiredSize = FindRequiredSize();
		mainCamera.orthographicSize = Mathf.SmoothDamp (mainCamera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
	}


	private float FindRequiredSize ()
	{
		
		Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);


		float size = 0f;

	
		for (int i = 0; i < targets.Length; i++)
		{

			if (targets [i].gameObject.activeSelf) {
				

		
				Vector3 targetLocalPos = transform.InverseTransformPoint (targets [i].position);


				Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;


				size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

		
				size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / mainCamera.aspect);
			}
		}


		size += screenEdgeBuffer;

	
		size = Mathf.Max (size, minSize);

		return size;
	}

	//da controllare da GameManager
	public void SetStartPositionAndSize ()
	{

		FindAveragePosition ();


		transform.position = desiredPosition;

	
		mainCamera.orthographicSize = FindRequiredSize ();
	}

	void WallDetection(){
			
			RaycastHit hit;
			GameObject[] players = new GameObject[GameManager.instance.numberOfPlayers];
			Vector3[] screenPos = new Vector3[GameManager.instance.numberOfPlayers];
			Ray[] wallRay = new Ray[GameManager.instance.numberOfPlayers];

			for(int playerIndex = 0; playerIndex < GameManager.instance.GetPlayers().Length ; playerIndex++ ){
			
			players [playerIndex] = GameManager.instance.GetPlayers () [playerIndex];
			screenPos[playerIndex] = mainCamera.ScreenToWorldPoint (players[playerIndex].transform.position);
			wallRay[playerIndex] = mainCamera.ScreenPointToRay (screenPos[playerIndex]);
			Vector3 dir = players[playerIndex].transform.position - mainCamera.transform.position;

			if (Physics.Raycast (mainCamera.transform.position, dir ,out hit )) {
				if (hit.transform.tag == "Wall") {
					Debug.Log ("Wall Found!");
					players [playerIndex].GetComponent<MeshRenderer> ().material.shader = OutlineShader;
				} 
			}
			Debug.DrawRay (mainCamera.transform.position,dir,Color.green);
		}
	}
}