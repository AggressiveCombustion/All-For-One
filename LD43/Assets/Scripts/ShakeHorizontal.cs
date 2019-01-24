using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeHorizontal : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GetComponent<Animator>().SetTrigger("h");
        Destroy(this, 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
