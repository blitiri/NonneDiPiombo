using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{

	public static UIManager instance;

    public PlayerControl player1;
    public PlayerControl player2;
//    public PlayerControl player3;
//    public PlayerControl player4;

	public float lifeStartValue;
//	[Range (0, 10)]
//	public float stressStartValue;

	public UILabel ammoPlayer1;
	public UILabel ammoPlayer2;

	public UISlider life1Slid;
	public UISlider stress1Slid;
	public UISlider life2Slid;
	public UISlider stress2Slid;

	void Awake ()
	{
		instance = this;
		lifeStartValue = player1.life;  // player1.life per trovare il valore di vita iniziale e massimo, che fa da dividendo nel Value dello Slider della vita
	}

	// Use this for initialization
	void Start ()
	{

	}

    void Update()
    {
        setUI();
//		Debug.Log("p1: " + player1.life);
//		Debug.Log("p2: " + player2.life);
//		Debug.Log (player2.life / lifeStartValue);
    }

    public void setUI ()
	{
		life1Slid.value = player1.life / lifeStartValue;
		stress1Slid.value = player1.stress / lifeStartValue;
		life2Slid.value = player2.life / lifeStartValue;
		stress2Slid.value = player2.stress / lifeStartValue;
		ammoPlayer1.text = player1.ammo.ToString ();
		ammoPlayer2.text = player2.ammo.ToString ();
    }
}
