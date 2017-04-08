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
		shaderApply.ShaderApply (bulletMeshRender, this.gameObject.transform.position, outlineShader, standardShader);
	}

	void OnTriggerEnter(Collider other){
		if(!other.gameObject.tag.StartsWith("Bullet")){
		Destroy (this.gameObject);
		}
	}
}
