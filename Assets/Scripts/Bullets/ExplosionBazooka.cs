using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBazooka : MonoBehaviour {
	private BazookaBulletManager bazooka;
	private SphereCollider explosionCollider;

	public float minRadius;
	public float maxRadius;
	private float timer;
	public float explosionTime;
	[HideInInspector]
	public bool wallCollision;

	void Start(){
		explosionCollider = GetComponent<SphereCollider> ();
	}

	void Update(){
		if (timer < explosionTime) {
			timer += Time.deltaTime;
		} else {
			explosionCollider.enabled = false;
			timer = 0;
		}
	}

	void OnCollisionEnter(Collision other){
		

		if (other.gameObject.tag.Equals ("Wall")) {
			//Debug.Log ("Collision With Wall");
			explosionCollider.radius = maxRadius;
			wallCollision = true;
		}  if (other.gameObject.tag.Equals ("LevelWall")) {
			explosionCollider.radius = maxRadius;
			wallCollision = false;
		}  if (other.gameObject.tag.StartsWith ("Player") && Utility.GetPlayerIndexFromBullet (this.gameObject.tag) == Utility.GetPlayerIndex (other.gameObject.tag)) {
			int explosionId;
			explosionId = Utility.GetPlayerIndexFromBullet (this.gameObject.tag);

			int playerId;
			playerId = Utility.GetPlayerIndex (other.gameObject.tag);

			Statistics.instance.PlayerSuicide (playerId);
			LevelUIManager.instance.SetScore (Statistics.instance.getPlayersKills() [playerId], playerId);
			other.gameObject.GetComponent<PlayerControl> ().RespawnOnTrigger (other, explosionId);
		} 
	}
}
