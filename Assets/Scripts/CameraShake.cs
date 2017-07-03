using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public bool shake = false;
    public float amplitude = 0.1f;
    public float duration = 0.5f;
    private float timer = 0.0f;

    private Vector2 initialLocalPos;


    void Start()
    {
        instance = this;
    }
	// Update is called once per frame
	void Update ()
    {
		initialLocalPos = transform.localPosition;
		if(shake == true)
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                Shake();
            }
            else
            {
                timer = 0.0f;
                shake = false;
            }
                
        }
	}

    void Shake()
    {
        transform.localPosition = initialLocalPos + Random.insideUnitCircle * amplitude;
    }
}
