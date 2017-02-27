using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{

	public static UIManager instance;
	[Range (0, 1)]
	public float lifeStartValue;
	[Range (0, 1)]
	public float stressStartValue;

	public UISlider life1Slid;
	public UISlider stress1Slid;
	public UISlider life2Slid;
	public UISlider stress2Slid;

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		life1Slid.value = lifeStartValue;
		stress1Slid.value = stressStartValue;
		life2Slid.value = lifeStartValue;
		stress2Slid.value = stressStartValue;
	}

	public void setLife (float life, int player)
	{
		switch (player) {
		case 1:
			life1Slid.value = life;
			break;
		case 2:
			life2Slid.value = life;
			break;
		default:
			break;
		}
	}

	public void setStress (float life, int player)
	{
		switch (player) {
		case 1:
			stress1Slid.value = life;
			break;
		case 2:
			stress2Slid.value = life;
			break;
		default:
			break;
		}
	}
}
