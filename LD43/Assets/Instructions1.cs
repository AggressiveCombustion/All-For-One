using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions1 : MonoBehaviour {

    public GameObject ins1;
    public GameObject ins2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.R))
        {
            DoTheThing();
        }
	}

    public void DoTheThing()
    {
        ins2.SetActive(true);
        ins1.SetActive(false);
    }
}
