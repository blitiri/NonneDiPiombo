using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private OutlineShaderApply shaderApply;
	private MeshRenderer bulletMeshRender;

	public Shader outlineShader;
	public Shader standardShader;

	void Awake(){
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
			Destroy (this.gameObject);
		}


	}
}
