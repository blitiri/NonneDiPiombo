using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    [Range(0,1)]
    public float temporaryValue;

    public UISlider life1Slid;
    public UISlider stress1Slid;
    public UISlider life2Slid;
    public UISlider stress2Slid;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        UpdatingValues();
    }

    public void UpdatingValues()
    {
        life1Slid.value = temporaryValue;
        stress1Slid.value = temporaryValue;
        life2Slid.value = temporaryValue;
        stress2Slid.value = temporaryValue;
    }
}
