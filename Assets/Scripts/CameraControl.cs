using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float dampTime = 0.2f;
	public float screenEdgeBuffer = 4f;
	public float minSize = 6.5f;
	public float maxSize;
	private float zoomSpeed;


	private Vector3 moveVelocity;
	private Vector3 desiredPosition;

	public Shader OutlineShader;
	public Shader StandardShader;

	private Camera mainCamera;

	public Transform[] targets;

	private OutlineShaderApply shaderApply;
	private MeshRenderer[] playerMeshRenderer;

	void Awake ()
	{
		shaderApply = new OutlineShaderApply ();

		GameObject[] players;
		players = GameManager.instance.GetPlayers ();

		targets = new Transform[players.Length];
		playerMeshRenderer = new MeshRenderer[players.Length];

		for (int playerIndex = 0; playerIndex < players.Length; playerIndex++) {
			targets [playerIndex] = players [playerIndex].transform;

			playerMeshRenderer [playerIndex] = players [playerIndex].GetComponent<MeshRenderer> ();
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
		transform.position = Vector3.SmoothDamp (transform.position, desiredPosition, ref moveVelocity, dampTime);
	}


	private void FindAveragePosition ()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;


		for (int i = 0; i < targets.Length; i++) {
			
			if (targets [i].gameObject.activeSelf) {
				averagePos += targets [i].position;
				numTargets++;
			}
		}


		if (numTargets > 0) {
			averagePos /= numTargets;
		}
		averagePos.y = transform.position.y;
		desiredPosition = averagePos;
	}


	private void Zoom ()
	{
		
		float requiredSize = FindRequiredSize ();
		mainCamera.fieldOfView = Mathf.SmoothDamp (mainCamera.fieldOfView, requiredSize, ref zoomSpeed, dampTime);
	}


	private float FindRequiredSize ()
	{
		Vector3 desiredLocalPos = transform.InverseTransformPoint (desiredPosition);
		float size = 0f;

		for (int i = 0; i < targets.Length; i++) {
			if (targets [i].gameObject.activeSelf) {
				Vector3 targetLocalPos = transform.InverseTransformPoint (targets [i].position);
				Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
				size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));
				size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / mainCamera.aspect);
			}
		}

		//Debug.Log ("Size Camera:" + size);
		size += screenEdgeBuffer;
		size = Mathf.Max (size, minSize);
		size = Mathf.Clamp (size, minSize, maxSize);
		return size;
	}

	//da controllare da GameManager
	public void SetStartPositionAndSize ()
	{
		FindAveragePosition ();
		transform.position = desiredPosition;
		mainCamera.orthographicSize = FindRequiredSize ();
	}

	public	void WallDetection ()
	{
		int playerNumbers = Configuration.instance.GetNumberOfPlayers ();
			

		GameObject[] players = new GameObject[playerNumbers];
		Vector3[] screenPos = new Vector3[playerNumbers];
		Ray[] wallRay = new Ray[playerNumbers];

		for (int playerIndex = 0; playerIndex < GameManager.instance.GetPlayers ().Length; playerIndex++) {
			
			players [playerIndex] = GameManager.instance.GetPlayers () [playerIndex];
			screenPos [playerIndex] = mainCamera.ScreenToWorldPoint (players [playerIndex].transform.position);
			wallRay [playerIndex] = mainCamera.ScreenPointToRay (screenPos [playerIndex]);

			Vector3 dir = players [playerIndex].transform.position - mainCamera.transform.position;
			shaderApply.ShaderApply (playerMeshRenderer [playerIndex], players [playerIndex].transform.position, OutlineShader, StandardShader);
		}
	}
}