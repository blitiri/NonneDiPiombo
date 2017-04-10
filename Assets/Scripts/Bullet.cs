using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private OutlineShaderApply shaderApply;
	private MeshRenderer bulletMeshRender;
	private Rigidbody bulletRb;

	public Shader outlineShader;
	public Shader standardShader;

	void Awake(){
		bulletRb = GetComponent<Rigidbody> ();
		shaderApply = new OutlineShaderApply ();
		bulletMeshRender = GetComponent<MeshRenderer> ();
	}

	void Update(){
		//Assegna Shader Outline
		shaderApply.ShaderApply (bulletMeshRender, this.gameObject.transform.position, outlineShader, standardShader);
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.StartsWith("Player") || other.gameObject.tag.Equals("Wall")){
			Debug.Log ("Bullet Collision With " + other);
			//finishing VFX
			bulletMeshRender.enabled = false;
			bulletRb.velocity = new Vector3 (0, 0, 0);
			Destroy (this.gameObject, 1f);
		}


	}
}
