using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public enum MovingPlatformType
    {
        pingPong,
        continuous,
        oneShot
    }

    public MovingPlatformType type;

    public PressureButton button; // the button attached to this

    public Transform target;
    public Vector2 startPos;

    public float speed = 2;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(button.pressed)
        {
            if(type == MovingPlatformType.continuous || type == MovingPlatformType.oneShot)
            {
                transform.position = Vector2.Lerp(transform.position, target.position, speed * Time.deltaTime);
            }
        }

        else
        {
            if (type == MovingPlatformType.continuous)
            {
                transform.position = Vector2.Lerp(transform.position, startPos, speed * Time.deltaTime);
            }
        }
	}
}
