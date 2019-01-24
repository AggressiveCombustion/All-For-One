using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour {

    public bool pressed = false;

    public Sprite[] sprites;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<SpriteRenderer>().sprite = (pressed ? sprites[0] : sprites[1]);
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PRESS");
        if(other.transform.tag == "Player")
        {
            pressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("UN-PRESS");
        if (other.transform.tag == "Player")
        {
            pressed = false;
        }
    }
}
