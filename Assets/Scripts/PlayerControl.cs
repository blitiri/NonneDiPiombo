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
    public bool otherWeapon = false;

    public bool dead = false;

   // public int startingAmmo = 60;
    public int startingLife = 100;
    public int startingStress = 0;
    public int maxLifeValue = 100;
    public int maxStressValue = 100;
    public int playerId;
    //private int ammo;
    private float life;

    public float speed = 2;
    public float rotSpeed = 2;
    public float underAttackInactivityTime = 2;
    public float dashWallDistance = 1.5f;
    public float stressDecreaseFactor;
    public float timeToDiminishStress;
    private float horizontalMovement;
    private float verticalMovement;
    [Range(0, 10)]
    public float dashTime;
    float dashRecordedTime = 0;
    [Range(0, 10)]
    public float dashDistance;
    private float stress;
    private float angleCorrection;
    public float stressIncrease = 10;
    public float dashSpeed = 10;
    public float dashLength = 5;
    [Range(0, 100)]
    public float playerObstacleDistanceLimit;

    private const string bulletTagPrefix = "Bullet";
    private const string playerTagPrefix = "Player";
//    private WeaponsManagerEnum selectedWeapon;
    private string tagActuallyWeapon;

    public LayerMask environment;
    public Texture2D crosshairCursor;
    public Vector2 cursorHotSpot = new Vector2(16, 16);

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

    public float blinkColorTime = 2;

    private OutlineShaderApply shaderApply;

    public Shader outlineShader;
    public Shader standardShader;

   

    //   [Range(0,1)]
    //   public float dashSpeed = 0.1f;


    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        shaderApply = new OutlineShaderApply();
        revolverMeshRenderer = revolver.GetComponent<MeshRenderer>();
        tagActuallyWeapon = "Revolver";
        /*weapons = new Hashtable ();
		foreach (Transform child in transform) {
			weapons.Add (child.gameObject.tag, child.gameObject);
		}
		SetActiveWeapons (defaultWeapon);*/

        if (aimTargetPrefab != null)
        {
            aimTarget = Instantiate(aimTargetPrefab) as GameObject;
        }
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        meshPlayer = GetComponent<MeshRenderer>();
        playerMat = meshPlayer.material;
       
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        
        inputManager = new InputManager(playerId, transform, angleCorrection);
        if (inputManager.HasMouse())
        {
            Cursor.SetCursor(crosshairCursor, cursorHotSpot, CursorMode.Auto);
        }
        ResetStatus();
        StartCoroutine(DiminishStress());
    }

    /// <summary>
    /// Updates the player instance.
    /// </summary>
    void Update()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        
        if (!underAttack)
        {
            if (!stopped && stopInputPlayer == false)
            {
                Move();
                DashManaging();
                Aim();
                //Shoot ();
                //Assegna Shader Outline su arma attiva
				shaderApply.ShaderApply(revolverMeshRenderer, revolver.transform.position, outlineShader, standardShader);
            }
            DropWeapon();
        }
    }



    /// <summary>
    /// Determines whether player is fit as a fiddle.
    /// </summary>
    /// <returns><c>true</c> if player is fit as a fiddle; otherwise, <c>false</c>.</returns>
    public bool IsFitAsAFiddle()
    {
        return life >= maxLifeValue;
    }

    private void DropWeapon()
    {
		
    }

    /// <summary>
    /// Resets the player status.
    /// </summary>
    public void ResetStatus()
    {
        life = startingLife;
        stress = startingStress;
        stopped = false;
        underAttack = false;
       // UpdateUI();
    }

    /// <summary>
    /// Moves the player.
    /// </summary>
    private void Move()
    {
        rb.MovePosition(rb.position + inputManager.GetMoveVector() * speed * Time.deltaTime);
        //ani.SetFloat ("Movement", horizontalMovement);
        //ani.SetFloat ("Movement", verticalMovement);
    }

    /// <summary>
    /// Aim player.
    /// </summary>
    private void Aim()
    {
        Vector3 aimVector;

        if (Time.timeScale > 0)
        {
            aimVector = inputManager.GetAimVector();

            if (aimVector != Vector3.zero)
            {

                if (inputManager.HasMouse())
                {
                    //Correzione aimvector per modello girato.
                    aimVector -= transform.position;
                    aimVector += transform.position;
                    transform.LookAt(aimVector);

                    if (aimTarget != null)
                    {
                        aimTarget.transform.position = new Vector3(aimVector.x, transform.position.y, aimVector.z);// + transform.position;
                                                                                                                   //Debug.Log ("aimVector=" + aimVector);
                    }
                }
                else
                {
                    transform.forward = Vector3.Normalize(aimVector);
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
    /// Notifies player is under attack by another.
    /// </summary>
    /// <param name="damage">Attack damage.</param>
    public void Attacked(int damage, string killerTag)
    {
        underAttack = true;
        StartCoroutine(AttackAnimation(damage, killerTag));
    }

    /// <summary>
    /// Coroutine to show attack animation.
    /// </summary>
    /// <returns>The animation.</returns>
    /// <param name="damage">Attack damage.</param>
    IEnumerator AttackAnimation(int damage, string killerTag)
    {
        Vector3 animation;
        float startTime;

        animation = new Vector3(0.2f, 0, 0);
        startTime = Time.time;
        while (Time.time - startTime < underAttackInactivityTime)
        {
            transform.Translate(animation);


            yield return null;
            animation = -animation;
        }
        AddDamage(damage, killerTag);
        underAttack = false;
    }
		
    /// <summary>
    /// Detects a collision enter with another player
    /// </summary>
    /// <param name="collision">Collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9 || collision.gameObject.layer == 10)
        {
            isDashing = false;
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
    void OnTriggerStay(Collider other)
    {
		if (inputManager.Pick ()) 
		{
			
		}
      
    }

    /// <summary>
    /// Adds the damage.
    /// </summary>
    /// <param name="damage">Damage.</param>
    /// <param name="killerTag">Killer tag.</param>
    private void AddDamage(int damage, string killerTag)
    {
        if (dead == false)
        {
            life -= damage;

            if (IsDead())
            {
                life = 0;
                GameManager.instance.PlayerKilled(GetPlayerId(gameObject.tag), GetPlayerId(killerTag));
            }
            //UpdateUI();
            StopCoroutine(BounceColor(toEmissionColor));
            StartCoroutine(BounceColor(toEmissionColor));
        }
    }

    /// <summary>
    /// Gets the player identifier from player tag.
    /// </summary>
    /// <returns>The player identifier.</returns>
    /// <param name="tag">Tag whence retrieve player identifier. Manage both player tag and bullet player tag</param>
    private int GetPlayerId(string tag)
    {
        // Retrieve player ID from bullet tag
        if (tag.StartsWith(bulletTagPrefix))
        {
            tag = tag.Substring(bulletTagPrefix.Length);
        }
        return int.Parse(tag.Substring(playerTagPrefix.Length));
    }

    /// <summary>
    /// Adds the life to the player.
    /// </summary>
    /// <param name="life">Life.</param>
    public void AddLife(int life)
    {
        this.life = Mathf.Clamp(this.life + life, 0, maxLifeValue);
       // UpdateUI();
    }

    /// <summary>
    /// Adds the stress.
    /// </summary>
    /// <param name="stressAdd">Stress to add.</param>
    public void AddStress(float stressToAdd)
    {
        stress = Mathf.Clamp(stress + stressToAdd, 0, maxStressValue);
       // UpdateUI();
    }

    /// <summary>
    /// Determines whether player is stressed.
    /// </summary>
    /// <returns><c>true</c> if player is stressed; otherwise, <c>false</c>.</returns>
    public bool IsStressed()
    {
        return stress > 0;
    }

    /// <summary>
    /// Determines whether player is collapsed.
    /// </summary>
    /// <returns><c>true</c> if player is collapsed; otherwise, <c>false</c>.</returns>
    public bool IsCollapsed()
    {
        return stress >= maxStressValue;
    }

    /// <summary>
    /// Determines whether the player is under attack.
    /// </summary>
    /// <returns><c>true</c> if this instance is under attack; otherwise, <c>false</c>.</returns>
    public bool IsUnderAttack()
    {
        return underAttack;
    }
		

    /// <summary>
    /// Refills the stress.
    /// </summary>
    /// <returns>The stress.</returns>
    IEnumerator DiminishStress()
    {
        while (stress < 100)
        {
            yield return new WaitForSeconds(timeToDiminishStress);
           
            stress = Mathf.Clamp(stress - stressDecreaseFactor, 0, maxStressValue);
            //UpdateUI();
        }
    }

    /// <summary>
    /// Starts dashing.
    /// </summary>
    private void DashManaging()
    {
        Vector3 moveVector = inputManager.GetMoveVector();
        //Debug.Log (moveVector.magnitude);
        ObstacleChecking(moveVector);
        isObstacle = ObstacleChecking(moveVector);
        if (inputManager.Dash())
        {
            if (!isObstacle && !isDashing && dashTime <= Time.time - dashRecordedTime)
            {
                StartCoroutine(Dashing(moveVector));
            }
        }
    }

    /// <summary>
    /// Checkings the environment.
    /// </summary>
    /// <returns><c>true</c>, if environment was checkinged, <c>false</c> otherwise.</returns>
    private bool ObstacleChecking(Vector3 moveVector)
    {
        float rayy = 0.1f;
        bool result = false;
        Vector3 ray = new Vector3(transform.position.x, rayy, transform.position.z);
        Vector3 rayDirection = moveVector;
        //        RaycastHit obstacleInfo;

        Debug.DrawRay(ray, rayDirection, Color.blue);

        if (Physics.Raycast(ray, rayDirection, playerObstacleDistanceLimit, environment))
        {
            result = true;
        }
        else
        {
            result = false;
        }
        //Debug.Log("result: " + result);
        return result;
    }

    private IEnumerator Dashing(Vector3 moveVector)
    {
        Vector3 newPosition = Vector3.zero;
        //float scaleProp;
        float dashDone = 0;

        isDashing = true;

        while (dashDone < dashLength && isDashing && !isObstacle)
        {
            //Debug.Log ("isObstacle" + isObstacle);
            if (moveVector.magnitude > 0)
            {
                transform.localPosition += dashSpeed * Time.deltaTime * moveVector; //= new Vector3(transform.localPosition.x + dashSpeed * Time.deltaTime * moveVector.x, transform.localPosition.y, transform.localPosition.z + dashSpeed * Time.deltaTime * moveVector.z);
            }
            else
            {
                transform.localPosition += dashSpeed * Time.deltaTime * transform.forward;
            }
            //                Debug.Log(moveVector);
            dashDone += dashSpeed * Time.deltaTime;
            yield return null;
            //                DashDone += moveVector.z > 0 ? dashSpeed * Time.deltaTime * moveVector.z : moveVector.x > 0 ? dashSpeed * Time.deltaTime * moveVector.x : dashSpeed * Time.deltaTime;
        }
        AddStress(stressIncrease);
        isDashing = false;
        dashRecordedTime = Time.time;
    }

    /// <summary>
    /// Sets the player identifier.
    /// </summary>
    /// <param name="playerId">Player identifier.</param>
    public void SetPlayerId(int playerId)
    {
        this.playerId = playerId;
        gameObject.tag = playerTagPrefix + playerId;
    }

    public void SetAngleCorrection(float angleCorrection)
    {
        this.angleCorrection = angleCorrection;
    }
		
    private IEnumerator BounceColor(Color toColor)
    {
        float timer = 0;
        float timeLimit = 0.2f;

        while (timer < timeLimit)
        {
            timer += Time.deltaTime;
            float colorGradient = Mathf.PingPong(timer /* blinkColorTime*/, 1);
            Color bouncingColor = toColor * Mathf.LinearToGammaSpace(colorGradient);
            playerMat.SetColor("_EmissionColor", bouncingColor);

            //  Debug.Log(timer);
            yield return new WaitForEndOfFrame();
        }
        playerMat.SetColor("_EmissionColor", Color.black);
    }
}