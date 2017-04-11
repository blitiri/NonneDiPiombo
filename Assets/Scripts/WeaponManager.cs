using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour 
{
	public int ammoMagazine=100;
	public float ratioOfFire=1.0f;
	public int weaponDamage=5;
    public float delayDeathWeapon = 5.0f;

	private OutlineShaderApply shaderApply;
	private MeshRenderer weaponMeshRenderer;

	public Shader outlineShader;
	public Shader standardShader;

	void Awake(){
		shaderApply = new OutlineShaderApply ();
		weaponMeshRenderer = this.GetComponent<MeshRenderer> ();
	}

	// Update is called once per frame
	void Update () 
	{
		shaderApply.ShaderApply (weaponMeshRenderer, this.gameObject.transform.position, outlineShader, standardShader);

		if (ammoMagazine <= 0)
		{
			Destroy(this.gameObject,delayDeathWeapon);  
		}
	}
}
