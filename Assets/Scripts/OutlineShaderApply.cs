using UnityEngine;
using System.Collections;

public class OutlineShaderApply  {
	public void ShaderApply(MeshRenderer renderer, Vector3 position,Shader outline,Shader standard ){
		RaycastHit hit;
		Vector3 dir = position - Camera.main.transform.position;

		if (Physics.Raycast (Camera.main.transform.position, dir ,out hit )) {
			if (hit.transform.tag == "Wall") {
				//Debug.Log ("Wall Found!");
				renderer.material.shader = outline;
			} else {
				renderer.material.shader = standard;
			}
		}
		Debug.DrawRay (Camera.main.transform.position,dir,Color.green);
	}

}
